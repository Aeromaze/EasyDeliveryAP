using System.Collections.Generic;
using System.Linq;
using EasyDeliveryAP.Archipelago;
using EasyDeliveryAP.Utils;
using HarmonyLib;
using UnityEngine;

namespace EasyDeliveryAP;

[HarmonyPatch]
public class APJobGenerator
{
    [HarmonyPatch(typeof(jobBoard), "GenerateJobBetter")]
    private static bool Prefix(int __0, jobBoard __instance, ref jobBoard.Job __result)
    {
        if (!ArchipelagoClient.Authenticated)
        {
            __result = null;
            // return false;
        }

        // Find active local towns
        List<Transform> towns = [];
        for (int i = 0; i < __instance.navigation.towns.Length; i++)
        {
            // TODO: Add check for unlocked towns
            if (__instance.navigation.towns[i].gameObject.activeSelf)
            {
                towns.Add(__instance.navigation.towns[i]);
            }
        }

        // return null if no valid town
        if (towns.Count == 0)
        {
            __result = null;
            return false;
        }

        // Find closest town
        int closestTownIndex = -1;
        for (int j = 0; j < towns.Count; j++)
        {
            if (closestTownIndex == -1)
            {
                closestTownIndex = j;
            }
            float num2 = Vector3.Distance(towns[j].position, __instance.navigation.car.transform.position);
            float num3 = Vector3.Distance(towns[closestTownIndex].position, __instance.navigation.car.transform.position);
            if (num2 < num3)
            {
                closestTownIndex = j;
            }
        }

        // Decide job start and end town
        Transform startTown = towns[0];
        Transform endTown = towns[0];
        if (towns.Count == 2 || __instance.intercityJobs)
        {
            int num4 = UnityEngine.Random.Range(0, towns.Count);
            int num41 = UnityEngine.Random.Range(0, towns.Count);
            startTown = towns[num4];
            endTown = towns[num41];
        }
        else if (towns.Count >= 3)
        {
            switch (__0)
            {
                case 0:
                    startTown = towns[closestTownIndex];
                    endTown = towns[closestTownIndex];
                    break;
                case 1:
                    startTown = towns[closestTownIndex];
                    endTown = towns[(closestTownIndex + 1) % towns.Count];
                    break;
                case 2:
                    startTown = towns[closestTownIndex];
                    endTown = towns[(closestTownIndex + 2) % towns.Count];
                    break;
                case 3:
                    startTown = towns[(closestTownIndex + 1) % towns.Count];
                    endTown = towns[(closestTownIndex + 2) % towns.Count];
                    break;
                case 4:
                    startTown = towns[(closestTownIndex + 2) % towns.Count];
                    endTown = towns[(closestTownIndex + 1) % towns.Count];
                    break;
            }
        }
        // Decide starting shopnode and check that the shopnode is in the start town
        int num5 = 10000;
        ShopInfo shopInfo;
        do
        {
            shopInfo = __instance.navigation.shopNodes[UnityEngine.Random.Range(0, __instance.navigation.shopNodes.Count)];
            num5--;
        }
        while (num5 >= 0 && shopInfo.node.town != startTown);
        // Decide destination node
        TunnelNode tunnelNode = null;
        int destinationIndex = -1;
        num5 = 10000;
        sMapNode destinationMapNode;
        if (!__instance.intercityJobs)
        {
            while (true)
            {
                int num6 = UnityEngine.Random.Range(0, __instance.navigation.destinationNodes.Count);
                destinationMapNode = __instance.navigation.destinationNodes[num6];
                num5--;
                if (num5 < 0)
                {
                    break;
                }
                if (!(destinationMapNode == shopInfo.node) && (__instance.sameTownDeliveries || !(destinationMapNode.town == shopInfo.node.town)) && towns.Contains(destinationMapNode.town) && !(destinationMapNode.town != endTown))
                {
                    destinationIndex = num6;
                    ArchipelagoConsole.LogMessage(destinationMapNode.name);
                    break;
                }
            }
        }
        else
        {
            tunnelNode = __instance.tunnelNodes[UnityEngine.Random.Range(0, __instance.tunnelNodes.Length)];
            string tunnel = tunnelNode.node.name;
            if (!__instance.navigation.nodes.Contains(tunnelNode.node) || (tunnel == "Snowy Peaks" && !APLogic.CanReachSnowyPeaks()) || (tunnel == "Fishing Town" && !APLogic.CanReachFishingTown()))// || (Items.TunnelToItem[tunnelNode.node.name]?.Received < 1 && APData.blocked_tunnels == "1"))
            {
                __result = null;
                return false;
            }
            destinationMapNode = tunnelNode.node;
        }

        APData.UpdateHints();

        jobBoard.Job job = new(shopInfo, destinationMapNode);
        job.path = __instance.navigation.FindPath(job.from, job.to);
        job.distance = sPathFinder.PathLength(job.path)/1000f;
        job.startingCityName = __instance.cityName;
        job.destCityName = __instance.cityName;
        job.destinationIndex = destinationIndex;
        job.bonusDistance = 0f;

        if((bool)tunnelNode)
        {
            int intercityDestIndex = UnityEngine.Random.Range(0, tunnelNode.distances.Length);
            while (tunnelNode.distances[intercityDestIndex] >= tunnelNode.distanceCutoff)
            {
                intercityDestIndex = UnityEngine.Random.Range(0, tunnelNode.distances.Length);
            }
            job.destinationIndex = intercityDestIndex;
            job.destCityName = tunnelNode.name;
            job.distance += (tunnelNode.distances[intercityDestIndex] + tunnelNode.distanceToOtherTown)/1000f;
            job.bonusDistance = sPathFinder.PathLength(__instance.navigation.FindPath(__instance.navigation.currentClosestNode, tunnelNode.node)) / 1000f;
            job.isIntercity = true;
        }
        else
        {
            job.distance = sPathFinder.PathLength(__instance.navigation.FindPath(__instance.navigation.currentClosestNode, job.shop.node))/1000f;
        }
        job.price = __instance.JobPrice(job, job.bonusDistance);

        __result = job;

        return false;
    }
}