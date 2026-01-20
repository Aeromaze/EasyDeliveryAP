using EasyDeliveryAP.Archipelago;
using HarmonyLib;

namespace EasyDeliveryAP;

[HarmonyPatch]
public class DeathLinkPatches
{
    private static readonly ArchipelagoClient archipelago = EasyDeliveryAP.ArchipelagoClient;

    // DeathLink handling
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
}