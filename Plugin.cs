using BepInEx;
using BepInEx.Logging;
using EasyDeliveryAP.Archipelago;
using EasyDeliveryAP.Utils;
using UnityEngine;
using HarmonyLib;
using System;
using EasyDeliveryAPI;
using BepInEx.Configuration;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;

namespace EasyDeliveryAP;

[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
public class EasyDeliveryAP : BaseUnityPlugin
{
    public const string PluginGUID = "com.aeromaze.easyDeliveryAP";
    public const string PluginName = "EasyDeliveryAP";
    public const string PluginVersion = "0.2.3";

    public const string ModDisplayInfo = $"{PluginName} v{PluginVersion}";
    private const string APDisplayInfo = $"Archipelago v{ArchipelagoClient.APVersion}";
    public static ManualLogSource BepinLogger;
    public static ArchipelagoClient ArchipelagoClient;

    public static bool deathLink = false;
    internal static ModdedSaveSystem<APSaveFile> save = new("Archipelago");

    // Configs
    public static ConfigEntry<bool> configConsole;
    public static ConfigEntry<bool> configDeathLink;
    public static ConfigEntry<bool> configAutoConnect;
    public static ConfigEntry<bool> configDebug;


    public static DesktopDotExe.File TrackerFile;
    public static Vector2 trackerPosition;

    // Debug vars
    public static bool debug = false;
    string itemId = "";
    string obj = "";
    bool upton = true;
    bool weston = true;
    bool easton = true;
    // GameObject obj2;

    private void Awake()
    {
        // Plugin startup logic
        BepinLogger = Logger;
        ArchipelagoClient = new ArchipelagoClient();
        ArchipelagoConsole.Awake();

        new Harmony(PluginGUID).PatchAll();

        configDeathLink = Config.Bind("General",
                                    "DeathLink",
                                    false,
                                    "Turn on DeathLink");

        configConsole = Config.Bind("General",
                                    "ShowConsole",
                                    false,
                                    "Show the ingame console");

        configAutoConnect = Config.Bind("General",
                                    "AutoConnect",
                                    true,
                                    "Automatically connect to the previous connection when opening a save");
        
        if (debug)
        {
            configDebug = Config.Bind("Debug",
                                        "EnableDebug",
                                        true,
                                        "Enable debug buttons");
        }

        EasyAPI.AddConfig("Archipelago", Config);

        //DesktopDotExe.File trackerAppFile = EasyAPI.InstantiateFile();


        //EasyAPI.AddListener<ScreenProgram>("Archipelago");
        TrackerFile = EasyAPI.InstantiateFile();
        TrackerFile.name = "Tracker";
        TrackerFile.type = DesktopDotExe.FileType.txt;
        TrackerFile.icon = 3;
        TrackerFile.iconHover = 4;
        TrackerFile.data = "listener_" + TrackerFile.name;
        trackerPosition.x = 0.35f;
        trackerPosition.y = 3.15f;
        TrackerFile.position = trackerPosition;
        EasyAPI.AddListener<TrackerApp>(TrackerFile.name);
        EasyAPI.AddFile(EasyAPI.DesktopLocation.Main, TrackerFile);
        //EasyAPI.AddProgram(new TrackerDotExe());

        Logger.LogMessage($"{ModDisplayInfo} loaded!");

        Items.GPS.Enabled = true;
    }

    private void Update()
    {
        if (DeathLinkHandler.deathLinkEnabled != configDeathLink.Value && ArchipelagoClient.Authenticated)
        {
            ArchipelagoClient.DeathLinkHandler.ToggleDeathLink();
        }

        if (!ArchipelagoClient.Authenticated)
        {
            APGUI.Warning("Not Connected");
        }

        debug = configDebug?.Value ?? debug;
    }

    private void OnGUI()
    {
        // show the mod is currently loaded in the corner
        GUI.Label(new Rect(16, 16, 300, 20), ModDisplayInfo);
        ArchipelagoConsole.OnGUI();

        string statusMessage;
        // show the Archipelago Version and whether we're connected or not
        if (ArchipelagoClient.Authenticated)
        {
            // if your game doesn't usually show the cursor this line may be necessary
            // Cursor.visible = false;

            statusMessage = " Status: Connected";
            GUI.Label(new Rect(16, 50, 300, 20), APDisplayInfo + statusMessage);
            
            /*
            if (GUI.Button(new Rect(16, 90, 150, 20), deathLink ? "Disable DeathLink" : "Enable DeathLink"))
            {
                ArchipelagoClient.DeathLinkHandler.ToggleDeathLink();
                deathLink = !deathLink;
            }
            */
        }
        else
        {
            // if your game doesn't usually show the cursor this line may be necessary
            Cursor.visible = true;

            statusMessage = " Status: Disconnected";
            GUI.Label(new Rect(16, 50, 300, 20), APDisplayInfo + statusMessage);
            GUI.Label(new Rect(16, 70, 150, 20), "Host: ");
            GUI.Label(new Rect(16, 90, 150, 20), "Player Name: ");
            GUI.Label(new Rect(16, 110, 150, 20), "Password: ");

            ArchipelagoClient.ServerData.Uri = GUI.TextField(new Rect(150, 70, 150, 20),
                ArchipelagoClient.ServerData.Uri);
            ArchipelagoClient.ServerData.SlotName = GUI.TextField(new Rect(150, 90, 150, 20),
                ArchipelagoClient.ServerData.SlotName);
            ArchipelagoClient.ServerData.Password = GUI.TextField(new Rect(150, 110, 150, 20),
                ArchipelagoClient.ServerData.Password);

            // requires that the player at least puts *something* in the slot name
            if (GUI.Button(new Rect(16, 130, 100, 20), "Connect") &&
                !ArchipelagoClient.ServerData.SlotName.IsNullOrWhiteSpace())
            {
                ArchipelagoClient.Connect();
            }
        }

        // this is a good place to create and add a bunch of debug buttons
        if (debug)
        {
            if (GUI.Button(new Rect(16, 150, 100, 20), "Money"))
            {
                ItemHandling.pendingMoney += 20;
            }
            GUI.Label(new Rect(16, 170, 150, 20), "ItemId: ");
            itemId = GUI.TextField(new Rect(150, 170, 150, 20), itemId);
            if (GUI.Button(new Rect(16, 190, 100, 20), "Add item"))
            {
                ItemHandling.pendingItemId = int.Parse(itemId);
                ItemHandling.pendingItem = true;
            }
            if (GUI.Button(new Rect(16, 210, 100, 20), "Remove item"))
            {
                ItemHandling.pendingItemId = int.Parse(itemId);
                ItemHandling.pendingRemoval = true;
            }
            if (GUI.Button(new Rect(16, 230, 75, 20), "Map"))
            {
                Items.GPS.Enabled = !Items.GPS.Enabled;
                ItemHandling.pendingUpgrade = true;
            }
            if (GUI.Button(new Rect(95, 230, 75, 20), "Tires"))
            {
                Items.Tires.Enabled = !Items.Tires.Enabled;
                ItemHandling.pendingUpgrade = true;
            }
            if (GUI.Button(new Rect(16, 250, 75, 20), "Bumper"))
            {
                Items.Bumper.Enabled = !Items.Bumper.Enabled;
                ItemHandling.pendingUpgrade = true;
            }
            if (GUI.Button(new Rect(95, 250, 75, 20), "Chains"))
            {
                Items.Chains.Enabled = !Items.Chains.Enabled;
                ItemHandling.pendingUpgrade = true;
            }
            GUI.Label(new Rect(16, 290, 150, 20), "GameObject: ");
            obj = GUI.TextField(new Rect(150, 290, 150, 20), obj);
            if (GUI.Button(new Rect(16, 310, 100, 20), "Deactivate Object"))
            {
                // obj2 = GameObject.Find(obj);
                // ArchipelagoConsole.LogMessage($"{obj2.GetInstanceID()}");
                OtherPatches.progression[obj].gameObject.SetActive(false);

            }
            if (GUI.Button(new Rect(120, 310, 100, 20), "Activate Object"))
            {
                OtherPatches.progression[obj].gameObject.SetActive(true);
            }
            if (GUI.Button(new Rect(16, 333, 100, 20), "Print Objects"))
            {
                foreach (Transform gameObject in GameObject.Find("UpgradeBasedProgression").GetComponentsInChildren<Transform>(true))
                {
                    ArchipelagoConsole.LogMessage($"{gameObject.name}");
                }
            }
            if (GUI.Button(new Rect(16, 353, 100, 20), "Disconnect"))
            {
                ArchipelagoClient.Disconnect();
            }
            if (GUI.Button(new Rect(16, 373, 100, 20), "TrackerText"))
            {
                TrackerText.UpdateTracker();
            }
            if (GUI.Button(new Rect(16, 393, 100, 20), "Upton") && OtherPatches.currentScene == 1)
            {
                OtherPatches.Nodes["Upton"].gameObject.SetActive(upton);
                upton = !upton;
            }
            if (GUI.Button(new Rect(16, 413, 100, 20), "Weston") && OtherPatches.currentScene == 1)
            {
                OtherPatches.Nodes["Weston"].gameObject.SetActive(weston);
                weston = !weston;
            }
            if (GUI.Button(new Rect(16, 433, 100, 20), "Easton") && OtherPatches.currentScene == 1)
            {
                OtherPatches.Nodes["Easton"].gameObject.SetActive(easton);
                easton = !easton;
            }
            if (GUI.Button(new Rect(16, 453, 100, 20), "Death"))
            {
                DeathLinkPatches.dying = true;
                APGUI.Notification("Dying");
            }
        }
    }
}