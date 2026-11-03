using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Converters;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Exceptions;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.MessageLog.Messages;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.Packets;
using EasyDeliveryAP.Utils;
using HarmonyLib;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Color = UnityEngine.Color;

namespace EasyDeliveryAP.Archipelago;

public class ArchipelagoClient
{
    public const string APVersion = "0.6.7";
    private const string Game = "Easy Delivery Co.";

    public static bool Authenticated;
    private bool attemptingConnection;

    public static Dictionary<long, ScoutedItemInfo> scoutedItemInfo = [];

    public static ArchipelagoData ServerData = new();
    public DeathLinkHandler DeathLinkHandler;
    public ArchipelagoSession session;

    /// <summary>
    /// call to connect to an Archipelago session. Connection info should already be set up on ServerData
    /// </summary>
    /// <returns></returns>
    public void Connect()
    {
        if (Authenticated || attemptingConnection) return;

        try
        {
            session = ArchipelagoSessionFactory.CreateSession(ServerData.Uri);
            SetupSession();
        }
        catch (Exception e)
        {
            EasyDeliveryAP.BepinLogger.LogError(e);
        }

        TryConnect();
    }

    /// <summary>
    /// add handlers for Archipelago events
    /// </summary>
    private void SetupSession()
    {
        session.MessageLog.OnMessageReceived += message => ArchipelagoConsole.LogMessage(message.ToString());
        session.Items.ItemReceived += OnItemReceived;
        session.Socket.ErrorReceived += OnSessionErrorReceived;
        session.Socket.SocketClosed += OnSessionSocketClosed;
        session.Socket.PacketReceived += OnPacketReceived;
    }

    /// <summary>
    /// attempt to connect to the server with our connection info
    /// </summary>
    private void TryConnect()
    {
        try
        {
            List<string> tags = [];
            if (EasyDeliveryAP.configDeathLink.Value)
            {
                tags.Add("DeathLink");
            }
            if (EasyDeliveryAP.configTrapLink.Value)
            {
                tags.Add("TrapLink");
            }
            if (EasyDeliveryAP.configShowPlayers.Value)
            {
                tags.Add("EasyCoBounce");
            }
            // it's safe to thread this function call but unity notoriously hates threading so do not use excessively
            ThreadPool.QueueUserWorkItem(
                _ => HandleConnectResult(
                    session.TryConnectAndLogin(
                        Game,
                        ServerData.SlotName,
                        ItemsHandlingFlags.AllItems,
                        new Version(APVersion),
                        tags: tags.ToArray(),
                        password: ServerData.Password,
                        requestSlotData: ServerData.NeedSlotData
                    )));
        }
        catch (Exception e)
        {
            EasyDeliveryAP.BepinLogger.LogError(e);
            HandleConnectResult(new LoginFailure(e.ToString()));
            attemptingConnection = false;
        }
    }

    /// <summary>
    /// handle the connection result and do things
    /// </summary>
    /// <param name="result"></param>
    private void HandleConnectResult(LoginResult result)
    {
        string outText;
        if (result.Successful)
        {
            var success = (LoginSuccessful)result;

            ServerData.SetupSession(success.SlotData, session.RoomState.Seed);
            Authenticated = true;

            DeathLinkHandler = new(session.CreateDeathLinkService(), ServerData.SlotName);
            session.Locations.CompleteLocationChecksAsync([.. ServerData.CheckedLocations]);
            outText = $"Successfully connected to {ServerData.Uri} as {ServerData.SlotName}!";

            session.DataStorage.TrackHints(APData.OnHintsReceived);

            scoutedItemInfo = session.Locations.ScoutLocationsAsync([.. session.Locations.AllLocations]).Result;
            APData.SetSlotSettings(ServerData.slotData);
            if (ScenePatches.thisScene != "TitleScreen") APData.SaveConnectionOrConnect();

            TrackerApp.TotalDeliveries();

            CarPatches.trailColor = new((byte)((APData.car_trail_color>>16) & 0xFF)/255f, (byte)((APData.car_trail_color>>8) & 0xFF)/255f, (byte)((APData.car_trail_color) & 0xFF)/255f);

            Color trail = CarPatches.trailColor;
            ArchipelagoConsole.LogMessage(outText);
        }
        else
        {
            var failure = (LoginFailure)result;
            outText = $"Failed to connect to {ServerData.Uri} as {ServerData.SlotName}.";
            outText = failure.Errors.Aggregate(outText, (current, error) => current + $"\n    {error}");

            EasyDeliveryAP.BepinLogger.LogError(outText);

            Authenticated = false;
            Disconnect();
        }

        ArchipelagoConsole.LogMessage(outText);
        attemptingConnection = false;
    }

    /// <summary>
    /// something went wrong, or we need to properly disconnect from the server. cleanup and re null our session
    /// </summary>
    public void Disconnect()
    {
        EasyDeliveryAP.BepinLogger.LogDebug("disconnecting from server...");
        session?.Socket.DisconnectAsync();
        session = null;
        Authenticated = false;

        ServerData.Index = 0;
        ItemHandling.pendingItemIds = [];
        // TrackerApp.pages = [0, 1];
        foreach (ItemData itemData in Items.APIdToItem.Values)
        {
            itemData.Received = 0;
        }
    }

    public void SendMessage(string message)
    {
        session.Socket.SendPacketAsync(new SayPacket { Text = message });
    }

    public void SendLocation(long location)
    {
        if (!Authenticated)
        {
            ArchipelagoConsole.LogMessage("Not connected. Can't send location.");
            return;
        }
        if (session.Locations.AllMissingLocations.Contains(location))
        {
            var item = scoutedItemInfo[location];
            ArchipelagoConsole.LogMessage($"Sending location: {item.LocationDisplayName} (Id: {location})");
            session.Locations.CompleteLocationChecks(location);
            APGUI.Notification($"Sending {item.ItemDisplayName} to {item.Player}", APGUI.packageSend);
            ServerData.CheckedLocations.Add(location);
        }
    }

    public void SendLocations(long[] location)
    {
        if (!Authenticated)
        {
            ArchipelagoConsole.LogMessage("Not connected. Can't send location(s).");
            return;
        }
        List<long> locations = [];
        bool checks = false;
        
        //ArchipelagoConsole.LogMessage(location.ToString() + location[0]);
        foreach (long locID in location)
        {
            if (session.Locations.AllMissingLocations.Contains(locID))
            {
                locations.Add(locID);
                checks = true;
            }
        }
        if (checks)
        {
            session.Locations.CompleteLocationChecks([.. locations]);
            foreach (var locID in locations)
            {
                var item = scoutedItemInfo[locID];
                ArchipelagoConsole.LogMessage($"Sending location: {item.LocationDisplayName} (Id: {locID})");
                APGUI.Notification($"Sending {item.ItemDisplayName} to {item.Player}", APGUI.packageSend);
                ServerData.CheckedLocations.Add(locID);
            }
        }
        // TrackerText.UpdateTracker();
    }

    public void SendCompletion()
    {
        var statusUpdatePacket = new StatusUpdatePacket
        {
            Status = ArchipelagoClientState.ClientGoal
        };
        session.Socket.SendPacket(statusUpdatePacket);
    }

    /// <summary>
    /// we received an item so reward it here
    /// </summary>
    /// <param name="helper">item helper which we can grab our item from</param>
    private void OnItemReceived(ReceivedItemsHelper helper)
    {
        var receivedItem = helper.DequeueItem();

        if (helper.Index <= ServerData.Index) return;

        ServerData.Index++;

        // Code to handle items
        if (Items.APIdToItem.ContainsKey((int)receivedItem.ItemId))
        {
            Items.APIdToItem[(int)receivedItem.ItemId].Received += 1;
        }

        if (EasyDeliveryAP.save.data.handledIndex >= ServerData.Index)
        {
            // ArchipelagoConsole.LogMessage($"{EasyDeliveryAP.save.data.handledIndex} {ServerData.Index}");
            return;
        }

        switch (receivedItem.ItemId)
        {
            // Installing car upgrades is primarily done in ItemHandling
            case 1:
                //Items.GPS.Enabled = true;
                APGUI.Notification("Received Map", APGUI.packageReceive);
                break;
            case 2:
                if (APData.car_upgrades == "1") Items.Tires.Enabled = true;
                APGUI.Notification("Received Snow Tires", APGUI.snowTires);
                break;
            case 3:
                if (APData.car_upgrades == "1") Items.Bumper.Enabled = true;
                APGUI.Notification("Received Bumper Bar", APGUI.truckBumper);
                break;
            case 4:
                if (APData.car_upgrades == "1") Items.Chains.Enabled = true;
                APGUI.Notification("Received Ice Chains", APGUI.iceChains);
                break;
            case 5:
                if (Items.ProgCarUpgrade.Received > 0)
                {
                    Items.Tires.Received = 1;
                    if (APData.car_upgrades == "1") Items.Tires.Enabled = true;
                    if (Items.ProgCarUpgrade.Received == 1)
                    {
                        APGUI.Notification("Received Snow Tires", APGUI.snowTires);
                    }
                }
                if (Items.ProgCarUpgrade.Received > 1)
                {
                    Items.Bumper.Received = 1;
                    if (APData.car_upgrades == "1") Items.Bumper.Enabled = true;
                    if (Items.ProgCarUpgrade.Received == 2)
                    {
                        APGUI.Notification("Received Bumper Bar", APGUI.truckBumper);
                    }
                }
                if (Items.ProgCarUpgrade.Received > 2)
                {
                    Items.Chains.Received = 1;
                    if (APData.car_upgrades == "1") Items.Chains.Enabled = true;
                    if (Items.ProgCarUpgrade.Received == 3)
                    {
                        APGUI.Notification("Received Ice Chains", APGUI.iceChains);
                    }
                }
                break;
            case 10:
                ItemHandling.pendingMoney += 33;
                break;
            case >= 100 and <= 117: // Inventory items
                ItemHandling.pendingItemIds.Add((int)receivedItem.ItemId - 100);
                ItemHandling.pendingItems = true;
                APGUI.Notification($"Received {Items.APIdToItem[(int)receivedItem.ItemId].Name}", APGUI.packageReceive);
                break;
            case 130: // Ice Trap
                APGUI.Notification($"Received {Items.APIdToItem[(int)receivedItem.ItemId].Name}", APGUI.warningTriangle);
                CarPatches.iceTrap = true;
                SendTrapLink("Ice Trap");
                break;
            case 131: // Shrink Trap
                APGUI.Notification($"Received {Items.APIdToItem[(int)receivedItem.ItemId].Name}", APGUI.warningTriangle);
                CarPatches.shrink = true;
                SendTrapLink("Shrink Trap");
                break;
            case 132: // Stuck Turning Trap
                APGUI.Notification($"Received {Items.APIdToItem[(int)receivedItem.ItemId].Name}", APGUI.warningTriangle);
                CarPatches.turnTrap = true;
                SendTrapLink("Frozen Steering Wheel Trap");
                break;
            case 133: // Forced Acceleration Trap
                APGUI.Notification($"Received {Items.APIdToItem[(int)receivedItem.ItemId].Name}", APGUI.warningTriangle);
                CarPatches.accelTrap = true;
                SendTrapLink("Forced Acceleration Trap");
                break;
            case 11 or 12 or 13 or (>= 30 and <= 38):
                APGUI.Notification($"Received {Items.APIdToItem[(int)receivedItem.ItemId].Name}", APGUI.packageReceive);
                break;
            case 20:
                APGUI.Notification($"Received {Items.APIdToItem[(int)receivedItem.ItemId].Name}", APGUI.radio);
                if (Items.RadioTower.Received > 2 && ScenePatches.currentScene == 4)
                {
                    OtherPatches.RemoveGate();
                }
                break;
            default:
                ArchipelagoConsole.LogMessage($"Received unhandled item: {receivedItem.ItemName} Id: {receivedItem.ItemId}");
                break;
        }
        EasyDeliveryAP.save.data.handledIndex = ServerData.Index;
    }

    /// <summary>
    /// something went wrong with our socket connection
    /// </summary>
    /// <param name="e">thrown exception from our socket</param>
    /// <param name="message">message received from the server</param>
    private void OnSessionErrorReceived(Exception e, string message)
    {
        EasyDeliveryAP.BepinLogger.LogError(e);
        ArchipelagoConsole.LogMessage(message);
        Disconnect();
    }

    /// <summary>
    /// something went wrong closing our connection. disconnect and clean up
    /// </summary>
    /// <param name="reason"></param>
    private void OnSessionSocketClosed(string reason)
    {
        EasyDeliveryAP.BepinLogger.LogError($"Connection to Archipelago lost: {reason}");
        Disconnect();
    }

    private void OnPacketReceived(ArchipelagoPacketBase packet)
    {
        if (packet.PacketType == ArchipelagoPacketType.Bounced)
        {
            BouncedPacket bouncedPacket = packet as BouncedPacket;

            if (ScenePatches.thisScene != "TitleScreen" && bouncedPacket.Tags.Contains("TrapLink") && EasyDeliveryAP.configTrapLink.Value && bouncedPacket.Data["source"].ToString() != session.Players.ActivePlayer.Name)
            {
                string trap_name = bouncedPacket.Data["trap_name"].ToString();

                if (Items.TrapLink.TryGetValue(trap_name, out string trap))
                {
                    string message = $"Received linked {trap_name} from {bouncedPacket.Data["source"]}";

                    APGUI.Notification(message, APGUI.warningTriangle);

                    switch (trap)
                    {
                        case "IceTrap":
                            CarPatches.iceTrap = true;
                            break;
                        case "FrozenSteeringWheelTrap":
                            CarPatches.turnTrap = true;
                            break;
                        case "ForcedAccelerationTrap":
                            CarPatches.accelTrap = true;
                            break;
                        case "ShrinkTrap":
                            CarPatches.shrink = true;
                            break;
                        case "DeathTrap":
                            if (EasyDeliveryAP.configDeathLink.Value)
                            {
                                DeathLinkPatches.dying = true;
                            }
                            break;
                        case "WideTrap":
                            break;
                    }
                }
            }
            
            if (ScenePatches.thisScene != "TitleScreen" && bouncedPacket.Tags.Contains("EasyCoBounce") && EasyDeliveryAP.configShowPlayers.Value)// && bouncedPacket.Data["source"].ToString() != session.ConnectionInfo.Uuid)
            {
                ReceivePosition(bouncedPacket);
            }
        }
    }

    public void SendTrapLink(string trap)
    {
        try
        {
            if (!EasyDeliveryAP.configTrapLink.Value)
            {
                return;
            }

            BouncePacket bouncePacket = new BouncePacket
            {
                Tags = new List<string> {"TrapLink"},
                Data = new Dictionary<string, JToken>
                {
                    { "time", DateTime.UtcNow.ToUnixTimeStamp() },
                    { "source", session.Players.ActivePlayer.Name },
                    { "trap_name", trap }
                }
            };

            session.Socket.SendPacketAsync(bouncePacket);
        }
        catch (ArchipelagoSocketClosedException)
        {
            Disconnect();
        }
    }

    public void SendPosition(Vector3 position, int scene)
    {
        try
        {
            if (!EasyDeliveryAP.configShowPlayers.Value || !ArchipelagoClient.Authenticated)
            {
                return;
            }

            BouncePacket bouncePacket = new BouncePacket
            {
                Tags = new List<string> {"EasyCoBounce"},
                Data = new Dictionary<string, JToken>
                {
                    { "time", DateTime.UtcNow.ToUnixTimeStamp() },
                    { "source", session.ConnectionInfo.Uuid },
                    { "player", session.Players.ActivePlayer.Name },
                    { "posx", position.x },
                    { "posy", position.y },
                    { "posz", position.z },
                    { "scene", scene }
                }
            };

            session.Socket.SendPacketAsync(bouncePacket);
        }
        catch (ArchipelagoSocketClosedException)
        {
            Disconnect();
        }
    }

    public void ReceivePosition(BouncedPacket bouncedPacket)
    {
        string source;
        if (bouncedPacket.Data["source"] != null)
        {
            source = bouncedPacket.Data["source"].ToString();
        }
        else
        {
            source = bouncedPacket.Data["player"].ToString();
        }
        string name = bouncedPacket.Data["player"].ToString();
        float x = (float)bouncedPacket.Data["posx"];
        float y = (float)bouncedPacket.Data["posy"];
        float z = (float)bouncedPacket.Data["posz"];
        int scene = (int)bouncedPacket.Data["scene"];
        double time = (int)bouncedPacket.Data["time"];

        List<string> remove = [];

        Vector3 position = new(x, y, z);
        Dictionary<string, PlayerPosition> playerPositions = PlayerLabels.playerPositions;

        foreach (var item in playerPositions)
        {
            if (Time.fixedUnscaledTime - item.Value.LocalTime > 10)
            {
                remove.Add(item.Key);
            }
        }

        foreach (string item in remove)
        {
            playerPositions.Remove(item);
        }

        if (!playerPositions.ContainsKey(source) && playerPositions.Count < 20)
        {
            APGUI.Notification("New Player");
            playerPositions.Add(source, new(name, position, scene, time, Time.fixedUnscaledTime));
        }
        else if (playerPositions.ContainsKey(source))
        {
            playerPositions[source].Name = name;
            playerPositions[source].PreviousPosition = playerPositions[source].TargetPosition;
            playerPositions[source].TargetPosition = position;
            playerPositions[source].Scene = scene;
            playerPositions[source].LastTime = playerPositions[source].Time;
            playerPositions[source].Time = time;
            playerPositions[source].LocalTime = Time.fixedUnscaledTime;
        }
    }

	public void EnableTag(string tag)
	{
		if (Array.IndexOf(session.ConnectionInfo.Tags, tag) == -1)
		{
			session.ConnectionInfo.UpdateConnectionOptions(session.ConnectionInfo.Tags.Concat(new string[1] { tag }).ToArray());
		}
        
	}

	public void DisableTag(string tag)
	{
		if (Array.IndexOf(session.ConnectionInfo.Tags, tag) != -1)
		{
			session.ConnectionInfo.UpdateConnectionOptions(session.ConnectionInfo.Tags.Where((string t) => t != tag).ToArray());
		}
	}
}