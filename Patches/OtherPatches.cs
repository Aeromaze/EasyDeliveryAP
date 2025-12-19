using System;
using EasyDeliveryAP.Archipelago;
using EasyDeliveryAP.Utils;
using HarmonyLib;
using UnityEngine;

namespace EasyDeliveryAP;

[HarmonyPatch]
public class OtherPatches
{
    private static readonly ArchipelagoClient archipelago = Plugin.ArchipelagoClient;

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
        else if (__0 == 1)
        {
            return true;
        }
        return true;
    }

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
            APGUI.Inform("Fishing Rod can't\nbe bought yet");
            return false;
        }
        else if (__instance.listener.name == "CookingPot" && Items.CookingPot.Received == 0)
        {
            APGUI.Inform("Cooking Pot can't\nbe bought yet");
            return false;
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
}