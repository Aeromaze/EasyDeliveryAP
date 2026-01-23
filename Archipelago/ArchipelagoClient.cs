using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.MessageLog.Messages;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.Packets;
using EasyDeliveryAP.Utils;
using HarmonyLib;
using UnityEngine;

namespace EasyDeliveryAP.Archipelago;

public class ArchipelagoClient
{
    public const string APVersion = "0.6.4";
    private const string Game = "Easy Delivery Co.";

    public static bool Authenticated;
    private bool attemptingConnection;

    private static Dictionary<long, ScoutedItemInfo> scoutedItemInfo = [];

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
    }

    /// <summary>
    /// attempt to connect to the server with our connection info
    /// </summary>
    private void TryConnect()
    {
        try
        {
            // it's safe to thread this function call but unity notoriously hates threading so do not use excessively
            ThreadPool.QueueUserWorkItem(
                _ => HandleConnectResult(
                    session.TryConnectAndLogin(
                        Game,
                        ServerData.SlotName,
                        ItemsHandlingFlags.AllItems,
                        new Version(APVersion),
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
            session.Locations.CompleteLocationChecksAsync(ServerData.CheckedLocations.ToArray());
            outText = $"Successfully connected to {ServerData.Uri} as {ServerData.SlotName}!";

            scoutedItemInfo = session.Locations.ScoutLocationsAsync([.. session.Locations.AllLocations]).Result;
            APData.SetSlotSettings(ServerData.slotData);

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
        
        TrackerText.UpdateTracker();
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
        foreach (ItemData itemData in Items.APIdToItem.Values)
        {
            itemData.Received = 0;
        }
        
        TrackerText.UpdateTracker();
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
            APGUI.Notification($"Sending {item.ItemDisplayName} to {item.Player}");
            ServerData.CheckedLocations.Add(location);
        }
        TrackerText.UpdateTracker();
    }

    public void SendCompletion()
    {
        var statusUpdatePacket = new StatusUpdatePacket();
        statusUpdatePacket.Status = ArchipelagoClientState.ClientGoal;
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
            case 1:
                //Items.GPS.Enabled = true;
                APGUI.Notification("Received Map");
                break;
            case 2:
                if (APData.car_upgrades == "1") Items.Tires.Enabled = true;
                APGUI.Notification("Received Snow Tires");
                break;
            case 3:
                if (APData.car_upgrades == "1") Items.Bumper.Enabled = true;
                APGUI.Notification("Received Bumper Bar");
                break;
            case 4:
                if (APData.car_upgrades == "1") Items.Chains.Enabled = true;
                APGUI.Notification("Received Ice Chains");
                break;
            case 10:
                ItemHandling.pendingMoney += 33;
                break;
            case >= 100 and <= 117: // Inventory items
                ItemHandling.pendingItemIds.Add((int)receivedItem.ItemId - 100);
                ItemHandling.pendingItems = true;
                APGUI.Notification($"Received {Items.APIdToItem[(int)receivedItem.ItemId].Name}");
                break;
            case 20 or 11 or 12 or 13:
                APGUI.Notification($"Received {Items.APIdToItem[(int)receivedItem.ItemId].Name}");
                break;
            default:
                ArchipelagoConsole.LogMessage($"Received unhandled item: {receivedItem.ItemName} Id: {receivedItem.ItemId}");
                break;
        }

        TrackerText.UpdateTracker();
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
}