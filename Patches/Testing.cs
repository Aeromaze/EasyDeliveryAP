using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using EasyDeliveryAP.Archipelago;
using EasyDeliveryAP.Utils;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace EasyDeliveryAP;

// [HarmonyPatch]
public class Testing
{
    private static readonly ArchipelagoClient archipelago = EasyDeliveryAP.ArchipelagoClient;

    [HarmonyPatch(typeof(sGoals), "CompleteGoal")]
    private static void Prefix(sGoals.Goal __0, sGoals __instance)
    {
        ArchipelagoConsole.LogMessage($"Goal completed: {__0.name}");
    }

    [HarmonyPatch(typeof(GenericItemCheckout), "MakePayment")]
    private static void Prefix(float __0, GenericItemCheckout __instance)
    {
        ArchipelagoConsole.LogMessage($"Buying {__instance.item.name} for {__0}$");
    }

    [HarmonyPatch(typeof(DeliveryResults), "DoSequence")]
    private static void Prefix(DeliveryResults __instance)
    {
        ArchipelagoConsole.LogMessage($"Paying: {__instance.paying}");
    }

    [HarmonyPatch(typeof(jobBoard), "GenerateJobBetter")]
    private static void Prefix(int __0, ref jobBoard __instance)
    {
        ArchipelagoConsole.LogMessage("Towns:");
        // OtherPatches.Nodes["Upton"].gameObject.SetActive(false);
        // OtherPatches.Nodes["Weston"].gameObject.SetActive(false);
        //Transform[] navTowns = [];
        foreach (Transform town in __instance.navigation.towns)
        {
            ArchipelagoConsole.LogMessage($"  {town.name}"); // valid start points?
            //if (town.name != "Upton")
            //navTowns.AddItem(town);
        }
        //__instance.navigation.towns = navTowns;
        
        foreach (ShopInfo shop in __instance.navigation.shopNodes)
        {
            ArchipelagoConsole.LogMessage($"Shop: {shop.name} Node: {shop.node.name}");
        }

        foreach (sMapNode node in __instance.navigation.allDestinationNodes)
        {
            //ArchipelagoConsole.LogMessage($"Dest Node: {node.name}"); // local destinations
        }

        foreach (TunnelNode node in __instance.tunnelNodes)
        {
            // ArchipelagoConsole.LogMessage($"Intercity Node: {node.distances}");
        }
        for (int i = __instance.jobs.Count - 1; i >=0; i--)
            ArchipelagoConsole.LogMessage($"{__instance.jobs[i].to.name}");
    }

    [HarmonyPatch(typeof(jobBoard), "GenerateJobBetter")]
    private static void Postfix(ref jobBoard.Job __result)
    {
        //if (__result.from.town.name == "Upton")
        //__result = null;
        ArchipelagoConsole.LogMessage($"");
    }

    [HarmonyPatch(typeof(sHUD), "ReceivePayment")]
    private static void Prefix(ref float __0)
    {
        ArchipelagoConsole.LogMessage($"Money Recieved: {__0}");

        //GameObject.Find("BranchBlockages").SetActive(false);
        //__0 *= 2;
    }

    [HarmonyPatch(typeof(UpgradeCheckout), "Update")]
    private static void Postfix(UpgradeCheckout __instance)
    {
        if (__instance.inventory.heldItem != null)
        {
            //ArchipelagoConsole.LogMessage($"Upgrade: {__instance.inventory.heldItem}");
        }
    }

    [HarmonyPatch(typeof(ConditionalToggle), "Toggle")]
    private static void Postfix(ConditionalToggle __instance)
    {
        ArchipelagoConsole.LogMessage($"Toggle: {__instance.name}");
    }

    [HarmonyPatch(typeof(CreditsHUD), "FrameUpdate")]
    private static void Prefix(CreditsHUD __instance)
    {
        ArchipelagoConsole.LogMessage($"Credits: {__instance.name}");
    }

    

    [HarmonyPatch(typeof(EndingManager), "SetEnding")]
    private static void Postfix(EndingManager.Ending __0,EndingManager __instance)
    {
        ArchipelagoConsole.LogMessage($"Ending: {__instance.currentEnding}");
    }

    [HarmonyPatch(typeof(FishingHole), "Catch")]
    private static void Prefix(FishingHole __instance)
    {
        if (__instance.hooked)
        {
            ArchipelagoConsole.LogMessage("Fish Caught");
        }
    }

    [HarmonyPatch(typeof(GachaPurchase), "MakePayment")]
    private static void Postfix(GachaPurchase __instance)
    {
        ArchipelagoConsole.LogMessage($"Gacha: {__instance.newItem}");
    }

    [HarmonyPatch(typeof(GarbageCan), "ThrowOut")]
    private static void Prefix(int __0, GarbageCan __instance)
    {
        ArchipelagoConsole.LogMessage($"Trashing item id: {__0}");
    }

    // Might help with fixing goals, or not
    [HarmonyPatch(typeof(GiveGoal), "Start")]
    private static void Prefix(GiveGoal __instance)
    {
        ArchipelagoConsole.LogMessage("GiveGoal");
        ArchipelagoConsole.LogMessage($"Goal: {__instance.goalIndex}");
    }

    [HarmonyPatch(typeof(GiveProgram), "Start")]
    private static void Prefix(GiveProgram __instance)
    {
        ArchipelagoConsole.LogMessage("GiveProgram");
        ArchipelagoConsole.LogMessage($"Program: {__instance.programName}");
    }

    [HarmonyPatch(typeof(HandheldRadio), "ButtonPressed")]
    private static void Prefix(HandheldRadio __instance)
    {
        ArchipelagoConsole.LogMessage("Hello");
    }

    [HarmonyPatch(typeof(PayloadManager), "GetRandomPayload")]
    private static void Postfix(ShopInfo __0, PayloadManager __instance, GameObject __result)
    {
        ArchipelagoConsole.LogMessage("RandomPayload");
        ArchipelagoConsole.LogMessage($"Payload: {__result.name} from shop: {__0.name}");
    }

    [HarmonyPatch(typeof(PayloadManager), "GetPayload")]
    private static void Postfix(GameObject __result)
    {
        ArchipelagoConsole.LogMessage($"Payload: {__result.name}");
    }

    [HarmonyPatch(typeof(BigDoor), "Start")]
    private static void Prefix(BigDoor __instance)
    {
        ArchipelagoConsole.LogMessage($"Door: {__instance.doorOpenDuration}");
    }

    [HarmonyPatch(typeof(sTeleporter), "Update")]
    private static void Prefix(sTeleporter __instance)
    {
        if (__instance.teleporting)
        {
            ArchipelagoConsole.LogMessage($"Teleporter: {__instance.name}\nLighter: {Items.Lighter.Received}");
        }

        switch (OtherPatches.currentScene)
        {
            case 5: // Snowy Peaks
                if (__instance.name == "StoreShop AutoShop" && Items.Tires.Enabled)
                {
                    Items.Tires.Toggle = true;
                    Items.Tires.Enabled = false;
                }
                else if (__instance.name == "ShopExit" && Items.Tires.Toggle)
                {
                    Items.Tires.Toggle = false;
                    Items.Tires.Enabled = true;
                }
                break;
            case 4: // Fishing Town
                if (__instance.name == "StoreShop AutoShop" && Items.Bumper.Enabled)
                {
                    Items.Bumper.Toggle = true;
                    Items.Bumper.Enabled = false;
                }
                else if (__instance.name == "ShopExit" && Items.Bumper.Toggle)
                {
                    Items.Bumper.Toggle = false;
                    Items.Bumper.Enabled = true;
                }
                if (__instance.name == "StoreShop AutoShop" && Items.Chains.Enabled)
                {
                    Items.Chains.Toggle = true;
                    Items.Chains.Enabled = false;
                }
                else if (__instance.name == "ShopExit" && Items.Chains.Toggle)
                {
                    Items.Chains.Toggle = false;
                    Items.Chains.Enabled = true;
                }
                break;
            case 8: // Dam underground
                break;
        }
    }

    [HarmonyPatch(typeof(sSceneConnection), "Update")]
    private static bool Prefix(sSceneConnection __instance)
    {
        if (__instance.connecting)
        {
            ArchipelagoConsole.LogMessage($"Scene connect: {__instance.name} {__instance.nextScene}");
            
        }
        if (__instance.nextScene != 3 || __instance.nextScene != 1)
        {
            //__instance.connecting = false;
            //__instance.nextScene = 6; // Entrance rando
            return true;
        }
        return true;
    }

    [HarmonyPatch(typeof(RoadSign), "Update")]
    private static void Postfix(RoadSign __instance)
    {
        Vector3 vector = __instance.cam.transform.position - __instance.transform.position;
        if (!(vector.magnitude > 30f) && !(Vector3.Dot(__instance.cam.transform.forward, vector.normalized) > 0f))
        {
            //ArchipelagoConsole.LogMessage($"Road sign: {__instance.name}\n {__instance.enterLabel}\n{__instance.exitLabel}");
        }
    }

    [HarmonyPatch(typeof(InteractiveToggle), "ToggleOn")]
    private static void Prefix(InteractiveToggle __instance)
    {
        ArchipelagoConsole.LogMessage($"Interactive toggle: {__instance.name}");
    }

    [HarmonyPatch(typeof(InteractionPoint), "DoAction", new Type[] {})]
    [HarmonyPatch(typeof(InteractionPoint), "DoAction", new Type[] {typeof(int)})]
    private static void Prefix(object[] __args, InteractionPoint __instance)
    {
        if (__args.Length != 0)
        {
            ArchipelagoConsole.LogMessage($"DoAction: {__instance.name} int: {__args[0]}");
        }
        else
        {
            ArchipelagoConsole.LogMessage($"DoAction: {__instance.name} listener: {__instance.listener.name}");
        }
        //__instance.listener.SetActive(false); // LOL
    }

    [HarmonyPatch(typeof(PurchasableItem), "OnEnable")]
    private static void Prefix(PurchasableItem __instance)
    {
        ArchipelagoConsole.LogMessage($"PurchasableItem: {__instance.pickup.listener.name}");
    }

    [HarmonyPatch(typeof(PurchaseItem), "Start")]
    private static void Prefix(PurchaseItem __instance)
    {
        ArchipelagoConsole.LogMessage($"Purchase Item: {__instance.name}");
    }

    public static GameObject tiresUpgrade;
    public static GameObject bumperUpgrade;
    public static GameObject chainsUpgrade;

    [HarmonyPatch(typeof(PurchaseUpgrade), "Start")]
    private static void Postfix(PurchaseUpgrade __instance)
    {
        ArchipelagoConsole.LogMessage($"Purchase Upgrade: {__instance.pickup.interactPointsObject.name}");
        switch (__instance.upgradeToggle.name)
        {
            case "TiresUpgrade":
                tiresUpgrade = __instance.pickup.interactPointsObject;
                break;
            case "BumperUpgrade":
                bumperUpgrade = __instance.pickup.interactPointsObject;
                break;
            case "IceChains":
                chainsUpgrade = __instance.pickup.interactPointsObject;
                break;
        }
    }

    [HarmonyPatch(typeof(InventoryWatcher), "Start")]
    private static void Prefix(InventoryWatcher __instance)
    {
        ArchipelagoConsole.LogMessage($"InventoryWatcher: {__instance.pairs[0].hasItem}");
        foreach (InventoryWatcher.itemObjectPair pair in __instance.pairs)
        {
            ArchipelagoConsole.LogMessage($"{pair.hasItem.name}");
        }
    }

    [HarmonyPatch(typeof(SnowTires), "Start")]
    private static void Postfix(SnowTires __instance)
    {
        ArchipelagoConsole.LogMessage($"Snow Tires: {__instance}");
    }
    
    [HarmonyPatch(typeof(TunnelNode), "Start")]
    private static void Postfix(TunnelNode __instance)
    {
        ArchipelagoConsole.LogMessage($"TunnelNode: {__instance.name}");
    }
    
    [HarmonyPatch(typeof(ToggleMapNode), "SetState")]
    private static void Prefix(ToggleMapNode __instance)
    {
        ArchipelagoConsole.LogMessage($"ToggleMapNode: {__instance.nodeToToggle.name}");
    }

    // change currentscene check to use this?
    private static GameObject screen;
    private static string lastscreen = "";

    [HarmonyPatch(typeof(MenuScreenTransition), "Update")]
    private static void Postfix(MenuScreenTransition __instance)
    {
        screen = __instance.screen;
        if (screen.scene.name != lastscreen)
        {
            ArchipelagoConsole.LogMessage($"Menu Screen: {__instance.screen.scene.name}");
        }
        lastscreen = screen.scene.name;
    }

    [HarmonyPatch(typeof(SnowcatManager), "EnableSnowcat")]
    private static void Prefix(SnowcatManager __instance)
    {
        ArchipelagoConsole.LogMessage($"Snowcat: {__instance}");
    }

    [HarmonyPatch(typeof(SnowcatManager), "ClosestIndex")]
    private static void Postfix(int __result, SnowcatManager __instance)
    {
        ArchipelagoConsole.LogMessage($"SnowcatBobbleIndex: {__result}");
    }
}