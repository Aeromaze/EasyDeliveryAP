using System;
using System.Collections.Generic;
using System.Linq;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Packets;
using EasyDeliveryAP.Archipelago;
using EasyDeliveryAP.Utils;
using HarmonyLib;
using UnityEngine;

namespace EasyDeliveryAP;

[HarmonyPatch]
public class OtherPatches
{
    private static readonly ArchipelagoClient archipelago = EasyDeliveryAP.ArchipelagoClient;

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

    public static int currentScene;

    [HarmonyPatch(typeof(SceneTransition), "LoadScene", new Type[] {typeof(int)})]
    private static bool Prefix(int __0)
    {
        // ArchipelagoConsole.LogMessage($"Scene id: {__0}");
        currentScene = __0;
        if (__0 == 3)
        {
            ArchipelagoConsole.LogMessage("Starting new save");
            ItemHandling.pendingItemIds = [];
            foreach (ItemData item in Items.APIdToItem.Values)
            {
                switch (item.Id)
                {
                    case -1:
                        break;
                    case >= 0 and <= 17:
                        for (int i = item.Received; i > 0; i--)
                        {
                            ItemHandling.pendingItemIds.Add(item.Id);
                        }
                        break;
                }
            }
            return true;
        }
        return true;
    }

    public static GameObject upgradedBasedProgression;
    public static GameObject shopInteriors;
    public static GameObject nodes;
    public static Transform[] radios;
    public static GameObject radio1;
    public static GameObject radio2;
    public static Dictionary<string, Transform> progression = [];
    public static Dictionary<string, Transform> storeInteriors = [];
    public static Dictionary<string, Transform> Nodes = [];
    public static Dictionary<string, sMapNode> MapNodes = [];


    // A good place to get neccessary gameobjects on scene change
    
    [HarmonyPatch(typeof(SceneTransition), "fadeIn")]
    private static void Prefix(SceneTransition __instance)
    {
        if (currentScene == 4 || currentScene == 5)
        {
            shopInteriors = GameObject.Find("StoreInteriors");
            storeInteriors = [];
            foreach (Transform transform in shopInteriors.GetComponentsInChildren<Transform>(true))
            {
                storeInteriors.TryAdd(transform.name, transform);
            }
        }
        else if (currentScene == 1)
        {
            shopInteriors = GameObject.Find("Store Interiors");
            storeInteriors = [];
            foreach (Transform transform in shopInteriors.GetComponentsInChildren<Transform>(true))
            {
                storeInteriors.TryAdd(transform.name, transform);
            }
            
            radios = GameObject.Find("RadioStationManager").GetComponentsInChildren<Transform>(true);
            foreach (Transform radio in radios)
            {
                switch (radio.gameObject.name)
                {
                    case "RadioStationOneActive":
                        radio1 = radio.gameObject;
                        break;
                    case "RadioStationTwoActive":
                        radio2 = radio.gameObject;
                        break;
                }
            }
        }

        if (currentScene == 1 || currentScene == 4 || currentScene == 5)
        {
            nodes = GameObject.Find("NavigationNodes");
            Nodes = [];
            foreach (Transform transform in nodes.GetComponentsInChildren<Transform>(true))
            {
                // if (transform.name != "Bar")
                Nodes.TryAdd(transform.name, transform);
            }
            MapNodes = [];
            foreach (sMapNode mapNode in nodes.GetComponentsInChildren<sMapNode>(true))
            {
                MapNodes.TryAdd(mapNode.name, mapNode);
            }
        }


        if (currentScene == 4 && (APData.radio_towers == "1" || APData.radio_towers == "3") && Items.RadioTower.Received >= 3)
        {
            GameObject.Find("ResetContainer")?.SetActive(false);
            GameObject.Find("ResetVolumes")?.SetActive(false);
            GameObject.Find("Destroy Gate Check")?.SetActive(false);
            GameObject.Find("GATE movable")?.SetActive(false);
            // GameObject.Find("Destroy Gate Check").SetActive(false);
        }

        if (currentScene == 1 && EasyDeliveryAP.debug)
        {
            foreach (string node in MapNodes.Keys)
            {
                //ArchipelagoConsole.LogMessage($"{node} - D: {MapNodes[node].destination} - E: {MapNodes[node].enabled}");
                if (Locations.MountainTownNode.TryGetValue(MapNodes[node].name, out NodeData nodeData))
                    if (nodeData.Town == "Upton")
                    {
                        // MapNodes[node].destination = false;
                    }
                ArchipelagoConsole.LogMessage($"{node} - D: {MapNodes[node].destination} - E: {MapNodes[node].enabled}");
            }
            //Nodes["Upton"].gameObject.SetActive(false);
        }

        /*
        if (currentScene == 1 || currentScene == 4 || currentScene == 5)
        {
            upgradedBasedProgression = GameObject.Find("UpgradeBasedProgression");
            transform = [];
            //upgradedBasedProgression = GameObject.Find("UpgradeBasedProgression").GetComponentsInChildren<Transform>(true);
            foreach (Transform gameObject in upgradedBasedProgression.GetComponentsInChildren<Transform>(true))
            {
                ArchipelagoConsole.LogMessage($"{gameObject.name}");
                transform.TryAdd(gameObject.name, gameObject);
            }
        }
        if (currentScene == 1)
        {
            // transform["BranchBlockages"].gameObject.SetActive(false); // needs a delay to work
        }
        */
    }
    

    // Get an instance of sHUD for ingame messages
    [HarmonyPatch(typeof(sHUD), "Start")]
    private static void Postfix(sHUD __instance)
    {
        APGUI.hud = __instance;
    }

    [HarmonyPatch(typeof(InteractionPoint), "DoAction", new Type[] {})]
    [HarmonyPatch(typeof(InteractionPoint), "DoAction", new Type[] {typeof(int)})]
    private static bool Prefix(InteractionPoint __instance, ref string __state)
    {
        __state = __instance.name;
        if (currentScene == 6)
        {
            if (APData.require_handheld_radio == "1" && __instance.name == "enter" && !ItemHandling.radio.Contains(17))
            {
                APGUI.Inform("Cannot enter without a Handheld Radio");
                return false;
            }
        }
        if (__instance.name == "Snow Tires" && Items.Tires.Received == 0)
        {
            APGUI.Inform("Snow Tires can't\nbe bought yet");
            return false;
        }
        else if (__instance.name == "Bumper Bar" && Items.Bumper.Received == 0)
        {
            APGUI.Inform("Bumper Bar can't\nbe bought yet");
            return false;
        }
        else if (__instance.name == "Ice Chains" && Items.Chains.Received == 0)
        {
            APGUI.Inform("Ice Chains can't\nbe bought yet");
            return false;
        }
        else if (__instance.listener.name == "Lighter" && Items.Lighter.Received == 0)
        {
            APGUI.Inform("Lighter can't\nbe bought yet");
            return false;
        }
        else if (__instance.listener.name == "FishingRod" && Items.FishingRod.Received == 0)
        {
            //APGUI.Inform("Fishing Rod can't\nbe bought yet");
            //return false;
        }
        else if (__instance.listener.name == "CookingPot" && Items.CookingPot.Received == 0)
        {
            //APGUI.Inform("Cooking Pot can't\nbe bought yet");
            //return false;
        }
        //__instance.listener.SetActive(false);
        return true;
    }

    [HarmonyPatch(typeof(InteractionPoint), "DoAction", new Type[] {})]
    [HarmonyPatch(typeof(InteractionPoint), "DoAction", new Type[] {typeof(int)})]
    private static void Postfix(InteractionPoint __instance, string __state)
    {
        if (__state == "turn on")
        {
            if (currentScene == 1)
            {
                if (radio1.activeSelf)
                {
                    //radio Upton
                    archipelago.SendLocation(60);
                    //ArchipelagoConsole.LogMessage("Radio 1");
                }
                if (radio2.activeSelf)
                {
                    //radio Easton
                    archipelago.SendLocation(61);
                    //ArchipelagoConsole.LogMessage("Radio 2");
                }
            }
            else if (currentScene == 5)
            {
                //radio Snowy Peaks
                archipelago.SendLocation(62);
                //ArchipelagoConsole.LogMessage("Radio 3");
            }
            else if (currentScene == 4)
            {
                //radio Fishing Town
                archipelago.SendLocation(63);
                //ArchipelagoConsole.LogMessage("Radio 4");
            }
        }
    }

    [HarmonyPatch(typeof(sSaveSystem), "OnApplicationQuit")]
    private static void Postfix()
    {
        archipelago.Disconnect();
    }

    // Make sure Snow Tires can be bought
    [HarmonyPatch(typeof(sTeleporter), "Teleport")]
    private static void Prefix()
    {
        if (currentScene == 5)
        {
            try
            {
                storeInteriors["tableAndUpgrade"].gameObject.SetActive(true);
            }
            catch { }
        }
    }

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