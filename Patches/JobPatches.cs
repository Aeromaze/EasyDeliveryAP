using System.Collections.Generic;
using EasyDeliveryAP.Archipelago;
using HarmonyLib;

namespace EasyDeliveryAP;

[HarmonyPatch]
public class JobPatches
{
    private static readonly ArchipelagoClient archipelago = EasyDeliveryAP.ArchipelagoClient;

    // Show how many checks a delivery would send
    [HarmonyPatch(typeof(jobBoard), "DrawJobList")]
    private static void Postfix(jobBoard __instance)
    {
        if (!ArchipelagoClient.Authenticated) return;
        var uncheckedLocations = archipelago.session.Locations.AllMissingLocations;

        List<jobBoard.Job> jobs = __instance.jobs;
        for (int i = jobs.Count - 1; i >=0; i--)
        {
            jobBoard.Job job = jobs[i];
            int checks = 0;
            bool hinted = false;

            // Delivery locations
            if (job.isIntercity)
            {
                string location;
                if (job.to.name == "Mountain Town")
                {
                    Locations.MountainTownIndex.TryGetValue(job.destinationIndex, out NodeData node);
                    location = node.Town;
                }
                else if (job.to.name == "Snowy Peaks")
                {
                    Locations.SnowyPeaksIndex.TryGetValue(job.destinationIndex, out NodeData node);
                    location = node.Town;
                }
                else if (job.to.name == "Fishing Town")
                {
                    Locations.FishingTownIndex.TryGetValue(job.destinationIndex, out NodeData node);
                    location = node.Town;
                }
                else
                {
                    location = "";
                }

                __instance.R.put(location, 25f, (24 + (6f + (i * 4)) * 8));

                if (Locations.Deliveries.TryGetValue($"{job.from.town.name} to {location} Delivery", out int deliveryId))                
                {
                    if (uncheckedLocations.Contains(deliveryId) && (APData.perfect_deliveries == "0" || APData.perfect_deliveries == "1"))
                    {
                        checks += 1;
                        hinted = APData.IsHinted(deliveryId, hinted);
                    }
                    if (uncheckedLocations.Contains(deliveryId + 10000) && (APData.perfect_deliveries == "1" || APData.perfect_deliveries == "2"))
                    {
                        checks += 1;
                        hinted = APData.IsHinted(deliveryId + 10000, hinted);
                    }
                }
                //else ArchipelagoConsole.LogMessage($"{job.from.town.name} to {location} Delivery");
            }
            else
            {
                if (Locations.Deliveries.TryGetValue($"{job.from.town.name} to {job.to.town.name} Delivery", out int deliveryId))
                {
                    if (uncheckedLocations.Contains(deliveryId) && (APData.perfect_deliveries == "0" || APData.perfect_deliveries == "1"))
                    {
                        checks += 1;
                        hinted = APData.IsHinted(deliveryId, hinted);
                    }
                    if (uncheckedLocations.Contains(deliveryId + 10000) && (APData.perfect_deliveries == "1" || APData.perfect_deliveries == "2"))
                    {
                        checks += 1;
                        hinted = APData.IsHinted(deliveryId + 10000, hinted);
                    }
                }
                //else ArchipelagoConsole.LogMessage($"{job.from.town.name} to {job.to.town.name} Delivery");

            }
            if (Locations.Deliveries.TryGetValue($"{job.startingCityName} to {job.destCityName} Delivery", out int deliveryIdCity))                
            {
                if (uncheckedLocations.Contains(deliveryIdCity) && (APData.perfect_deliveries == "0" || APData.perfect_deliveries == "1"))
                {
                    checks += 1;
                    hinted = APData.IsHinted(deliveryIdCity, hinted);
                }
                if (uncheckedLocations.Contains(deliveryIdCity + 10000) && (APData.perfect_deliveries == "1" || APData.perfect_deliveries == "2"))
                {
                    checks += 1;
                    hinted = APData.IsHinted(deliveryIdCity + 10000, hinted);
                }
            }

            // Payload locations
            if (Locations.PayloadDeliveries.TryGetValue(job.payloadPrefab.name, out int payload))
            {
                if (uncheckedLocations.Contains(payload) && APData.payload_checks == "1")
                {
                    checks += 1;
                    hinted = APData.IsHinted(payload, hinted);
                }
            }
            //else ArchipelagoConsole.LogMessage($"{job.payloadPrefab.name} is not registered");

            if (checks != 0)
            {
                if (hinted)
                {
                    __instance.R.fput("!", 120f, (24 + (6f + (i * 4)) * 8));
                }
                __instance.R.fput(checks.ToString(), 128f, (24 + (6f + (i * 4)) * 8));
            }
        }
    }
}