using EasyDeliveryAP.Archipelago;
using HarmonyLib;

namespace EasyDeliveryAP;

[HarmonyPatch]
public class GameStatePatches
{
    private static readonly ArchipelagoClient archipelago = EasyDeliveryAP.ArchipelagoClient;

    // Make sure Snow Tires can be bought
    [HarmonyPatch(typeof(sTeleporter), "Teleport")]
    private static void Prefix()
    {
        if (ScenePatches.currentScene == 5)
        {
            try
            {
                OtherPatches.storeInteriors["tableAndUpgrade"].gameObject.SetActive(true);
            }
            catch { }
        }
    }

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
}