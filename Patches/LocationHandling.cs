using System;
using System.Collections.Generic;
using System.Linq;
using Archipelago.MultiClient.Net;
using EasyDeliveryAP.Archipelago;
using EasyDeliveryAP.Utils;
using HarmonyLib;
using UnityEngine;

namespace EasyDeliveryAP;

[HarmonyPatch]
public class LocationHandling
{
    private static readonly ArchipelagoClient archipelago = EasyDeliveryAP.ArchipelagoClient;

    public static string jobStartTown;
    public static string jobStartCity;
    public static string jobStartShop;
    public static int jobDestinationIndex;
    public static string jobPayload;

    [HarmonyPatch(typeof(jobBoard), "ConfirmJob")]
    private static void Postfix(jobBoard __instance)
    {
        ArchipelagoConsole.LogMessage($"Starting delivery from: {__instance.selectedJob.shop.name} in {__instance.selectedJob.from.town.name} - {__instance.cityName} (Dest index: {__instance.selectedJob.destinationIndex})");
        jobStartTown = __instance.selectedJob.from.town.name;
        jobStartCity = __instance.selectedJob.startingCityName;
        jobStartShop = __instance.selectedJob.shop.name;
        jobDestinationIndex = __instance.selectedJob.destinationIndex;
        if (EasyDeliveryAP.debug)
        {
            ArchipelagoConsole.LogMessage($"Destination node: {__instance.selectedJob.to.name} Bool: {__instance.selectedJob.to.destination}");
            foreach (var item in __instance.selectedJob.to.connections)
            {
                ArchipelagoConsole.LogMessage($"Connections: {item.name} Bool: {item.destination}");
            }
        }
    }

    [HarmonyPatch(typeof(jobBoard), "CompleteJob")]
    private static void Prefix(jobBoard __instance)
    {
        ArchipelagoConsole.LogMessage($"Completed delivering {__instance.selectedJob.payloadPrefab.name} to: {__instance.selectedJob.to.town.name} - {__instance.cityName} (Dest index: {jobDestinationIndex})");

        List<long> locations = [];
        string delivery = $"{jobStartTown} to {__instance.selectedJob.to.town.name} Delivery";
        string deliveryCity = $"{jobStartCity} to {__instance.selectedJob.destCityName} Delivery";
        if (Locations.Deliveries.TryGetValue(delivery, out int locationId))
        {
            if (APData.perfect_deliveries == "0" || APData.perfect_deliveries == "1")
                locations.Add(locationId);
            if ((APData.perfect_deliveries == "1" || APData.perfect_deliveries == "2") && __instance.recoveries <= 0)
                locations.Add(10000 + locationId);
        }
        if (Locations.Deliveries.TryGetValue(deliveryCity, out int cityLocationId))
        {
            if (APData.perfect_deliveries == "0" || APData.perfect_deliveries == "1")
                locations.Add(cityLocationId);
            if ((APData.perfect_deliveries == "1" || APData.perfect_deliveries == "2") && __instance.recoveries <= 0)
                locations.Add(10000 + cityLocationId);
        }
        if (APData.payload_checks == "1" && Locations.PayloadDeliveries.TryGetValue(__instance.selectedJob.payloadPrefab.name, out int payloadId))
        {
            locations.Add(payloadId);
        }
        archipelago.SendLocations([.. locations]);
    }

    /*
    [HarmonyPatch(typeof(UpgradeCheckout), "InstallUpgrade")]
    private static void Prefix(UpgradeCheckout __instance)
    {
        // ArchipelagoConsole.LogMessage($"Installing Upgrade: {__instance.item.name}");
    }
    */

    // Normal goal
    [HarmonyPatch(typeof(EndingManager), "SetEnding")]
    private static void Postfix(EndingManager.Ending __0,EndingManager __instance)
    {
        // ArchipelagoConsole.LogMessage($"Ending: {__instance.currentEnding}");
        archipelago.SendCompletion();
    }

    [HarmonyPatch(typeof(GenericItemCheckout), "MakePayment")]
    private static void Prefix(float __0, GenericItemCheckout __instance)
    {
        if (EasyDeliveryAP.debug)
        {
            ArchipelagoConsole.LogMessage($"Buying {__instance.item.name} for {__0}$");
        }
        if (Locations.BlindBags.TryGetValue($"{ScenePatches.currentScene}{__instance.item.name}", out int blindBag) && APData.blind_bags == "1")
        {
            // ArchipelagoConsole.LogMessage($"Sending Blind Bag: {blindBag}");
            archipelago.SendLocation(blindBag);
        }
    }

    // Snowcat checks
    private static bool snowcat = false;

    [HarmonyPatch(typeof(SnowcatManager), "EnableSnowcat")]
    private static void Prefix()
    {
        snowcat = true;
    }

    [HarmonyPatch(typeof(SnowcatManager), "EnableSnowcat")]
    private static void Postfix()
    {
        snowcat = false;
    }

    [HarmonyPatch(typeof(SnowcatManager), "ClosestIndex")]
    private static void Postfix(int __result)
    {
        ArchipelagoConsole.LogMessage($"SnowcatBobbleIndex: {__result}");
        if (snowcat) archipelago.SendLocation(__result + 40);
    }

    [HarmonyPatch(typeof(InteractionPoint), "DoAction", new Type[] {})]
    [HarmonyPatch(typeof(InteractionPoint), "DoAction", new Type[] {typeof(int)})]
    private static bool Prefix(InteractionPoint __instance, ref string __state)
    {
        __state = __instance.name;
        // __instance.listener.transform.GetLocalPositionAndRotation(out Vector3 pos, out Quaternion _);
        Vector3 pos = __instance.listener.transform.position;

        // Vending Machines and Bins
        if (__instance.name == "throw away" || __instance.name == "take" || __instance.name == "buy")
        {
            Dictionary<string, PositionData> PositionList = [];
            
            if (ScenePatches.currentScene == 1) // Mountain Town
            {
                PositionList = Locations.MountainTownPositions;
            }
            else if (ScenePatches.currentScene == 4) // Fishing Town
            {
                PositionList = Locations.FishingTownPositions;
            }
            else if (ScenePatches.currentScene == 5) // Snowy Peaks
            {
                PositionList = Locations.SnowyPeaksPositions;
            }

            if (PositionList.TryGetValue(pos.ToString(), out PositionData posData))
            {
                ArchipelagoConsole.LogMessage($"VendingBin: {posData.Name}", false);
                // APGUI.Notification(posData.Name);
                if ((APData.lock_towns == "0" || (APData.lock_towns == "1" && Items.NameToItem[posData.Town]?.Received > 0)))
                {
                    if (!(__instance.name == "buy"))
                    {
                        archipelago.SendLocations([posData.LocationId]);
                    }
                }
                else if (__instance.name == "throw away")
                {
                    APGUI.Inform("Full");
                    return false;
                }
                else if (__instance.name == "buy")
                {
                    APGUI.Inform("Out of order");
                    return false;
                }
            }
            else
            {
                ArchipelagoConsole.LogMessage($"VendingBin: {pos}", false);
                // APGUI.Notification(pos.ToString());
            }
        }
        if (ScenePatches.currentScene == 6)
        {
            if (APData.require_handheld_radio == "1" && __instance.name == "enter" && !ItemHandling.radio.Contains(17))
            {
                APGUI.Inform("Cannot enter without a Handheld Radio");
                return false;
            }
        }
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
        else if (ScenePatches.currentScene == 4 && __instance.listener.name == "StationEntrance" && (APData.radio_towers == "1" || APData.radio_towers == "3") && Items.RadioTower.Received < 3)
        {
            APGUI.Inform("This door lacks radio signal");
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

    [HarmonyPatch(typeof(InteractionPoint), "DoAction", new Type[] {})]
    [HarmonyPatch(typeof(InteractionPoint), "DoAction", new Type[] {typeof(int)})]
    private static void Postfix(InteractionPoint __instance, string __state)
    {
        if (__state == "turn on")
        {
            if (ScenePatches.currentScene == 1)
            {
                if (OtherPatches.radio1.activeSelf)
                {
                    //radio Upton
                    archipelago.SendLocation(60);
                    //ArchipelagoConsole.LogMessage("Radio 1");
                }
                if (OtherPatches.radio2.activeSelf)
                {
                    //radio Easton
                    archipelago.SendLocation(61);
                    //ArchipelagoConsole.LogMessage("Radio 2");
                }
            }
            else if (ScenePatches.currentScene == 5)
            {
                //radio Snowy Peaks
                archipelago.SendLocation(62);
                //ArchipelagoConsole.LogMessage("Radio 3");
            }
            else if (ScenePatches.currentScene == 4)
            {
                //radio Fishing Town
                archipelago.SendLocation(63);
                //ArchipelagoConsole.LogMessage("Radio 4");
            }
        }
    }

    [HarmonyPatch(typeof(RoadSign), "Update")]
    private static void Postfix(RoadSign __instance)
    {
        Vector3 vector = __instance.cam.transform.position - __instance.transform.position;
        if (ArchipelagoClient.Authenticated && !(vector.magnitude > 30f) && !(Vector3.Dot(__instance.cam.transform.forward, vector.normalized) > 0f))
        {
            Dictionary<string, PositionData> positions = [];
            switch (ScenePatches.currentScene)
            {
                case 1:
                    positions = Locations.MountainTownPositions;
                    break;
                case 4:
                    positions = Locations.FishingTownPositions;
                    break;
                case 5:
                    positions = Locations.SnowyPeaksPositions;
                    break;
            }
            // int? locationId = positions[__instance.gameObject.transform.position.ToString()]?.LocationId;
            if (positions.TryGetValue(__instance.gameObject.transform.position.ToString(), out PositionData positionData))
            {
                archipelago.SendLocations([(int)positionData.LocationId]);
            }
            else
            {
                ArchipelagoConsole.LogMessage($"Unregistered or knocked over sign: {__instance.gameObject.transform.position}");
            }
        }
    }
}