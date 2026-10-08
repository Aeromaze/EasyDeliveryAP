using System;
using EasyDeliveryAP.Archipelago;
using HarmonyLib;
using UnityEngine;

namespace EasyDeliveryAP;

[HarmonyPatch]
public class CarPatches
{
    private static readonly ArchipelagoClient archipelago = EasyDeliveryAP.ArchipelagoClient;

    public static bool shrink = false;
    private static Vector3 shrinkSpeed = new(0.01f, 0.01f, 0.01f);
    private static int shrinkTimer = 0;

    public static bool iceTrap = false;
    private static int iceTimer = 0;

    public static bool teleport = false;
    private static Vector3 MountainTownTP = new(-452.94f, 328f, -325.21f);
    private static Quaternion MountainTownQt = new (0.00282f, 0.45403f, -0.00174f, 0.89098f);
    private static Vector3 SnowyPeaksTP = new(-350.28f, 288f, 388.10f);
    private static Quaternion SnowyPeaksQt = new(0.00282f, 0.45403f, -0.00174f, 0.89098f);
    private static Vector3 FishingTownTP = new(-471.51f, 124.16f, 193.76f);
    private static Quaternion FishingTownQt = new(-0.00178f, 0.75104f, -0.00163f, -0.66025f);

    public static Color trailColor = new(1, 1, 1);

    public static bool turnTrap = false;
    private static int turnTimer = 0;
    private static float randomTurn = 0;
    public static bool accelTrap = false;
    private static int accelTimer = 0;

    [HarmonyPatch(typeof(sCarController), "Update")]
    private static void Prefix(sCarController __instance)
    {
        if (!PauseSystem.paused && !__instance.guyActive)
        {
            // Shrink Trap
            if (shrink)
            {
                if (__instance.transform.localScale.x > 1)
                {
                    __instance.transform.localScale -= shrinkSpeed;
                    __instance.payloadPivot.localScale -= shrinkSpeed;

                    __instance.maxDrivePower -= 600f/150;
                    __instance.maxSpeed -= 100f/150;
                    __instance.maxSpeedScale -= 1f/150;
                    __instance.drivePowerScale -= 1f/150;
                }
                else
                {
                    shrink = false;
                    shrinkTimer += 1800;
                }
            }
            else if (shrinkTimer > 0)
            {
                shrinkTimer -= 1;
            }
            else if (__instance.transform.localScale.x < 1.5)
            {
                __instance.transform.localScale += shrinkSpeed;
                __instance.payloadPivot.localScale += shrinkSpeed;

                __instance.maxDrivePower += 600f/150;
                __instance.maxSpeed += 100f/150;
                __instance.maxSpeedScale += 1f/150;
                __instance.drivePowerScale += 1f/150;
            }
        }
        if (PauseSystem.paused && teleport && !__instance.guyActive)
        {
            // Teleports
            if (OtherPatches.currentScene == 1)
            {
                __instance.transform.SetPositionAndRotation(MountainTownTP, MountainTownQt);
            }
            if (OtherPatches.currentScene == 5)
            {
                __instance.transform.SetPositionAndRotation(SnowyPeaksTP, SnowyPeaksQt);
            }
            else if (OtherPatches.currentScene == 4)
            {
                __instance.transform.SetPositionAndRotation(FishingTownTP, FishingTownQt);
            }
            teleport = false;
        }
        __instance.dirtColor = trailColor;
        __instance.smokeColor = trailColor;
    }

    [HarmonyPatch(typeof(sCarController), "GetMaterialIndex")]
    private static bool Prefix(sCarController __instance, ref int __result)
    {
        if (!PauseSystem.paused && !__instance.guyActive)
        {
            // Ice Trap. Makes the wheels drive like on ice
            if (iceTrap)
            {
                iceTimer += 7000;
                iceTrap = false;
            }
            else if (iceTimer > 0)
            {
                __result = 3;
                iceTimer -= 1;
                return false;
            }
        }
        return true;
        
    }

    [HarmonyPatch(typeof(sCarController), "SetInput", new Type[] {typeof(Vector2)})]
    private static void Prefix(sCarController __instance, ref Vector2 __0)
    {
        // y = turning, x = acceleration
        if (!PauseSystem.paused && !__instance.guyActive)
        {
            // Lock steering for a moment
            if (turnTrap)
            {
                turnTimer += 500;
                turnTrap = false;
                // randomTurn = UnityEngine.Random.Range(-1f, 1f);
                randomTurn = __0.x;
            }
            else if (turnTimer > 0)
            {
                __0 = new(randomTurn, __0.y);
                turnTimer -= 1;
            }
            // Lock acceleration for a moment
            if (accelTrap)
            {
                accelTimer += 500;
                accelTrap = false;
            }
            else if (accelTimer > 0)
            {
                __0 = new(__0.x, 1);
                accelTimer -= 1;
            }
        }
    }
}
