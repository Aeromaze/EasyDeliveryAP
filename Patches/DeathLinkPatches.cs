using EasyDeliveryAP.Archipelago;
using EasyDeliveryAP.Utils;
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
    private static void Prefix(ref DeathManager __instance)
    {
        if (__instance.deathTime >= 50)
        {
            __instance.deathTime = 30;
            // ArchipelagoConsole.LogMessage("Attempt to set deathTime");
        }
        if (!ArchipelagoClient.Authenticated) return;
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