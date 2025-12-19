using Archipelago.MultiClient.Net;
using EasyDeliveryAP.Archipelago;
using EasyDeliveryAP.Utils;
using HarmonyLib;

namespace EasyDeliveryAP;

[HarmonyPatch]
public class LocationHandling
{
    private static readonly ArchipelagoClient archipelago = Plugin.ArchipelagoClient;

    public static string jobStartTown;
    public static string jobStartShop;
    public static int jobDestinationIndex;
    public static string jobPayload;

    [HarmonyPatch(typeof(jobBoard), "ConfirmJob")]
    private static void Postfix(jobBoard __instance)
    {
        ArchipelagoConsole.LogMessage($"Starting delivery from: {__instance.selectedJob.shop.name} in {__instance.selectedJob.from.town.name} - {__instance.cityName} (Dest index: {__instance.selectedJob.destinationIndex})");
        // ArchipelagoConsole.LogMessage($"Tunnel amount: {__instance.tunnelNodes.Length} GPS: {__instance.GPSEnabled}");
        
        jobStartTown = __instance.selectedJob.from.town.name;
        jobStartShop = __instance.selectedJob.shop.name;
        jobDestinationIndex = __instance.selectedJob.destinationIndex;
    }

    [HarmonyPatch(typeof(jobBoard), "CompleteJob")]
    private static void Prefix(jobBoard __instance)
    {
        ArchipelagoConsole.LogMessage($"Completed delivering {__instance.selectedJob.payloadPrefab.name} to: {__instance.selectedJob.to.town.name} - {__instance.cityName} (Dest index: {jobDestinationIndex})");

        string delivery = $"{jobStartTown} to {__instance.selectedJob.to.town.name} Delivery";
        if (Locations.GetDeliveries().TryGetValue(delivery, out int locationId))
        {
            ArchipelagoConsole.LogMessage($"Sending Location: {delivery}\nId: {locationId}");
            archipelago.SendLocation(locationId);
        }
        // jobPayload = __instance.selectedJob.payloadPrefab.name;
        // APGUI.Inform(jobPayload);
    }

    [HarmonyPatch(typeof(UpgradeCheckout), "InstallUpgrade")]
    private static void Prefix(UpgradeCheckout __instance)
    {
        ArchipelagoConsole.LogMessage($"Installing Upgrade: {__instance.item.name}");
    }

    [HarmonyPatch(typeof(EndingManager), "SetEnding")]
    private static void Postfix(EndingManager.Ending __0,EndingManager __instance)
    {
        ArchipelagoConsole.LogMessage($"Ending: {__instance.currentEnding}");
        archipelago.SendCompletion();
    }
}