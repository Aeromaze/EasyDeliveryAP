using System.Collections.Generic;
using System.Linq;
using Archipelago.MultiClient.Net;
using EasyDeliveryAP.Archipelago;
using EasyDeliveryAP.Utils;
using HarmonyLib;

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

    [HarmonyPatch(typeof(UpgradeCheckout), "InstallUpgrade")]
    private static void Prefix(UpgradeCheckout __instance)
    {
        // ArchipelagoConsole.LogMessage($"Installing Upgrade: {__instance.item.name}");
    }

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
            ArchipelagoConsole.LogMessage($"Buying {__instance.item.name} for {__0}$");
        if (Locations.BlindBags.TryGetValue($"{OtherPatches.currentScene}{__instance.item.name}", out int blindBag) && APData.blind_bags == "1")
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
}