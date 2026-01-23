using System.Collections.Generic;
using System.Linq;
using EasyDeliveryAP.Archipelago;
using EasyDeliveryAP.Utils;
using HarmonyLib;

namespace EasyDeliveryAP;

[HarmonyPatch]
public class ItemHandling
{
    public static float pendingMoney = 0;

    [HarmonyPatch(typeof(sHUD), "Update")]
    private static void Prefix(sHUD __instance)
    {
        if (pendingMoney != 0)
        {
            ArchipelagoConsole.LogMessage($"Adding {pendingMoney}$");
            __instance.ReceivePayment(pendingMoney);
            pendingMoney = 0;
        }
    }

    public static bool pendingItems;
    public static List<int> pendingItemIds = [];

    public static List<int> radio;

    // Debug values
    public static bool pendingItem;
    public static bool pendingRemoval;
    public static int pendingItemId;

    [HarmonyPatch(typeof(sItemManager), "Update")]
    private static void Postfix(sItemManager __instance)
    {
        if (pendingItems)
        {
            foreach (int i in pendingItemIds)
            {
                ArchipelagoConsole.LogMessage($"Adding item id {i} to inventory");
                __instance.AddToInventory(i, 1);
            }
            pendingItemIds = [];
            pendingItems = false;
            EasyDeliveryAP.save.data.handledIndex = ArchipelagoClient.ServerData.Index;
        }
        
        radio = __instance.inventory;

        // Debug methods
        if (pendingItem)
        {
            ArchipelagoConsole.LogMessage($"Adding item id: {pendingItemId}");
            __instance.AddToInventory(pendingItemId, 1);
            pendingItem = false;
        }
        if (pendingRemoval)
        {
            ArchipelagoConsole.LogMessage($"Removing item id: {pendingItemId}");
            __instance.inventory.Remove(pendingItemId);
            pendingRemoval = false;
        }
    }

    // debug values
    public static bool pendingUpgrade;

    [HarmonyPatch(typeof(TruckUpgrades), "LateUpdate")]
    private static void Prefix(TruckUpgrades __instance)
    {
        if (TestData.optionStartMap)
        {
            __instance.hasGPS = Items.GPS.Enabled;
        }

        if (APData.car_upgrades == "1")
        {
            __instance.hasGPS = Items.GPS.Enabled;
            __instance.hasTires = Items.Tires.Received > 0;
            __instance.hasBumper = Items.Bumper.Received > 0;
            __instance.hasChains = Items.Chains.Received > 0;
        }

        // debug
        if (pendingUpgrade)
        {
            __instance.hasGPS = Items.GPS.Enabled;
            __instance.hasTires = Items.Tires.Enabled;
            __instance.hasBumper = Items.Bumper.Enabled;
            __instance.hasChains = Items.Chains.Enabled;
            pendingUpgrade = false;
        }
    }
}