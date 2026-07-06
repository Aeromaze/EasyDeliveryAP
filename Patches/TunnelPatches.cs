using BepInEx;
using EasyDeliveryAP.Archipelago;
using EasyDeliveryAP.Utils;
using HarmonyLib;
using UnityEngine;

namespace EasyDeliveryAP;

[HarmonyPatch]
public class TunnelPatches
{
    [HarmonyPatch(typeof(sSceneConnection), "Update")]
    private static bool Prefix(sSceneConnection __instance)
    {
        string blocked_tunnels = APData.blocked_tunnels;
        
        if (blocked_tunnels == "0" || __instance.justConnected || __instance.nextScene == 1 || blocked_tunnels.IsNullOrWhiteSpace())
            return true;
        if (__instance.nextScene == 5)
            if ((Items.TunnelSP.Received > 0 && blocked_tunnels == "1") || blocked_tunnels != "1")
                return true;
        if (__instance.nextScene == 4)
            if ((Items.TunnelFT.Received > 0 && blocked_tunnels == "1") || blocked_tunnels != "1")
                return true;
        if (__instance.nextScene == 6)
            if (Items.TunnelFactory.Received > 0)
                return true;
        
        // Indicate that a tunnel is inactive when the player is close
        if (Vector3.Distance(__instance.gameObject.transform.position, __instance.car.transform.position) < 25f)
            APGUI.Warning("tunnel under construction");
        return false;
    }
}