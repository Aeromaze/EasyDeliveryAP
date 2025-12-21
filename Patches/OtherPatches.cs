using System;
using System.Collections.Generic;
using EasyDeliveryAP.Archipelago;
using EasyDeliveryAP.Utils;
using HarmonyLib;
using UnityEngine;

namespace EasyDeliveryAP;

[HarmonyPatch]
public class OtherPatches
{
    private static readonly ArchipelagoClient archipelago = EasyDeliveryAP.ArchipelagoClient;

    // Enable drinking energy drinks without GPS
    [HarmonyPatch(typeof(EnergyDrink), "ButtonPressed")]
    private static void Prefix(ref EnergyDrink __instance, ref bool __state)
    {
        if (!Items.GPS.Enabled && !__instance.upgrades.hadGPS)
        {
            __instance.upgrades.hasGPS = true;
            __state = true;
        }
    }

    [HarmonyPatch(typeof(EnergyDrink), "ButtonPressed")]
    private static void Postfix(ref EnergyDrink __instance, ref bool __state)
    {
        if (__state)
        {
            __instance.upgrades.hasGPS = false;
            __state = false;
        }
    }

    public static int currentScene;

    [HarmonyPatch(typeof(SceneTransition), "LoadScene", new Type[] {typeof(int)})]
    private static bool Prefix(int __0)
    {
        // ArchipelagoConsole.LogMessage($"Scene id: {__0}");
        currentScene = __0;
        if (__0 == 3)
        {
            ArchipelagoConsole.LogMessage("Starting new save");
            ItemHandling.pendingItemIds = [];
            foreach (ItemData item in Items.APIdToItem.Values)
            {
                switch (item.Id)
                {
                    case -1:
                        break;
                    case >= 0 and <= 17:
                        for (int i = item.Received; i > 0; i--)
                        {
                            ItemHandling.pendingItemIds.Add(item.Id);
                        }
                        break;
                }
            }
            return true;
        }
        return true;
    }

    public static GameObject upgradedBasedProgression;
    public static GameObject shopInteriors;
    public static Dictionary<string, Transform> transform = [];
    public static Dictionary<string, Transform> storeInteriors = [];

    // A good place to get neccessary gameobjects on scene change
    
    [HarmonyPatch(typeof(SceneTransition), "fadeIn")]
    private static void Prefix(SceneTransition __instance)
    {
        if (currentScene == 4 || currentScene == 5)
        {
            shopInteriors = GameObject.Find("StoreInteriors");
            storeInteriors = [];
            foreach (Transform tform in shopInteriors.GetComponentsInChildren<Transform>(true))
            {
                storeInteriors.TryAdd(tform.name, tform);
            }
            
        }
        if (currentScene == 1)
        {
            shopInteriors = GameObject.Find("Store Interiors");
            storeInteriors = [];
            foreach (Transform tform in shopInteriors.GetComponentsInChildren<Transform>(true))
            {
                storeInteriors.TryAdd(tform.name, tform);
            }
            
        }

        /*
        if (currentScene == 1 || currentScene == 4 || currentScene == 5)
        {
            upgradedBasedProgression = GameObject.Find("UpgradeBasedProgression");
            transform = [];
            //upgradedBasedProgression = GameObject.Find("UpgradeBasedProgression").GetComponentsInChildren<Transform>(true);
            foreach (Transform gameObject in upgradedBasedProgression.GetComponentsInChildren<Transform>(true))
            {
                ArchipelagoConsole.LogMessage($"{gameObject.name}");
                transform.TryAdd(gameObject.name, gameObject);
            }
        }
        if (currentScene == 4 && TestData.optionGate == "open")
        {
            transform["ResetContainer"].gameObject.SetActive(false);
            transform["ResetVolumes"].gameObject.SetActive(false);
            transform["Destroy Gate Check"].gameObject.SetActive(false);
            GameObject.Find("GATE movable").SetActive(false);
            // GameObject.Find("Destroy Gate Check").SetActive(false);
        }
        if (currentScene == 1)
        {
            // transform["BranchBlockages"].gameObject.SetActive(false); // needs a delay to work
        }
        */
    }
    

    // Get an instance of sHUD for ingame messages
    [HarmonyPatch(typeof(sHUD), "Start")]
    private static void Postfix(sHUD __instance)
    {
        APGUI.hud = __instance;
    }

    [HarmonyPatch(typeof(InteractionPoint), "DoAction", new Type[] {})]
    [HarmonyPatch(typeof(InteractionPoint), "DoAction", new Type[] {typeof(int)})]
    private static bool Prefix(InteractionPoint __instance)
    {
        if (__instance.name == "Snow Tires" && Items.Tires.Received == 0)
        {
            APGUI.Inform("Snow Tires can't\nbe bought yet");
            return false;
        }
        else if (__instance.name == "Bumper Bar" && Items.Bumper.Received == 0)
        {
            APGUI.Inform("Bumper Bar can't\nbe bought yet");
            return false;
        }
        else if (__instance.name == "Ice Chains" && Items.Chains.Received == 0)
        {
            APGUI.Inform("Ice Chains can't\nbe bought yet");
            return false;
        }
        else if (__instance.listener.name == "Lighter" && Items.Lighter.Received == 0)
        {
            APGUI.Inform("Lighter can't\nbe bought yet");
            return false;
        }
        else if (__instance.listener.name == "FishingRod" && Items.FishingRod.Received == 0)
        {
            //APGUI.Inform("Fishing Rod can't\nbe bought yet");
            //return false;
        }
        else if (__instance.listener.name == "CookingPot" && Items.CookingPot.Received == 0)
        {
            //APGUI.Inform("Cooking Pot can't\nbe bought yet");
            //return false;
        }
        //__instance.listener.SetActive(false);
        return true;
    }

    private static bool dead;
    public static bool dying;

    [HarmonyPatch(typeof(DeathManager), "Update")]
    private static void Prefix(DeathManager __instance)
    {
        if (dead != __instance.dying)
        {
            dead = __instance.dying;
            if (__instance.dying && !dying)
            {
                archipelago.DeathLinkHandler.SendDeathLink();
                // ArchipelagoConsole.LogMessage("");
            }
            dying = false;
        }
        if (dying && !dead && !__instance.dying)
        {
            // ArchipelagoConsole.LogMessage("Dying");
            __instance.dying = true;
            // dying = false;
        }
    }

    [HarmonyPatch(typeof(sSaveSystem), "OnApplicationQuit")]
    private static void Postfix()
    {
        archipelago.Disconnect();
    }

    [HarmonyPatch(typeof(sTeleporter), "Teleport")]
    private static void Prefix()
    {
        if (currentScene == 5)
        {
            try
            {
                storeInteriors["tableAndUpgrade"].gameObject.SetActive(true);
            }
            catch { }
        }
    }

    private static GameObject screen;
    private static string lastscreen = "";

    [HarmonyPatch(typeof(MenuScreenTransition), "Update")]
    private static void Postfix(MenuScreenTransition __instance)
    {
        screen = __instance.screen;
        if (screen.scene.name != lastscreen && screen.scene.name == "TitleScreen")
        {
            EasyDeliveryAP.save.data.handledIndex = 0;
        }
        lastscreen = screen.scene.name;
    }
}