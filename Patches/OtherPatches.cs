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

    public static GameObject upgradedBasedProgression;
    public static GameObject shopInteriors;
    public static GameObject nodes;
    public static sCarController car;
    public static sCharacterController guy;
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
        if (ScenePatches.currentScene == 4 || ScenePatches.currentScene == 5)
        {
            shopInteriors = GameObject.Find("StoreInteriors");
            storeInteriors = [];
            foreach (Transform transform in shopInteriors.GetComponentsInChildren<Transform>(true))
            {
                storeInteriors.TryAdd(transform.name, transform);
            }
        }
        else if (ScenePatches.currentScene == 1)
        {
            shopInteriors = GameObject.Find("Store Interiors");
            storeInteriors = [];
            foreach (Transform transform in shopInteriors.GetComponentsInChildren<Transform>(true))
            {
                storeInteriors.TryAdd(transform.name, transform);
            }
            
            // Differenciate between radiostations in Mountain Town
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

        if (ScenePatches.currentScene == 1 || ScenePatches.currentScene == 4 || ScenePatches.currentScene == 5)
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
            guy = GameObject.FindObjectOfType<sCharacterController>();
        }
        else if (ScenePatches.currentScene == 6)
        {
            guy = GameObject.FindObjectOfType<sCharacterController>();
        }

        // Remove Fishing Town Gate with 3 "Radio Tower" items
        RemoveGate();

        if (ScenePatches.currentScene == 1 && EasyDeliveryAP.debug)
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

    public static void RemoveGate()
    {
        if (ScenePatches.currentScene == 4 && (APData.radio_towers == "1" || APData.radio_towers == "3") && Items.RadioTower.Received >= 3)
        {
            GameObject.Find("ResetContainer")?.SetActive(false);
            GameObject.Find("ResetVolumes")?.SetActive(false);
            GameObject.Find("Destroy Gate Check")?.SetActive(false);
            GameObject.Find("GATE movable")?.SetActive(false);
            // GameObject.Find("Destroy Gate Check").SetActive(false);
        }
    }


    // Get an instance of sHUD for ingame messages
    [HarmonyPatch(typeof(sHUD), "Start")]
    private static void Postfix(sHUD __instance)
    {
        APGUI.hud = __instance;
    }

    private static List<string> labels = [];
    private static bool doneLabel = false;
    private static int locLabel = -1;

    [HarmonyPatch(typeof(sHUD), "DrawWorldPointLabel", new Type[] {typeof(Vector3), typeof(string)})]
    [HarmonyPatch(typeof(sHUD), "DrawWorldPointLabel", new Type[] {typeof(Vector3), typeof(string), typeof(Vector2)})]
    private static void Prefix(Vector3 __0, ref string __1)
    {
        /*
        APGUI.Warning($"{__0}: {__1}");
        string label = $"{__0}";
        if (!labels.Contains(label) && (__1 == "buy"))
        {
            labels.Add(label);
            
            foreach (var item in labels)
            {
                ArchipelagoConsole.LogMessage($"{{\"{item}\", }},");
            }
        }
        */

        doneLabel = false;
        locLabel = -1;
        Dictionary<string, PositionData> PositionList = [];
        Vector3 pos = __0;

        if (__1 == "throw away") pos.y -= 1;

        if (ScenePatches.currentScene == 1) // Mountain Town
        {
            PositionList = Locations.MountainTownPositions;
        }
        else if (ScenePatches.currentScene == 4) // Fishing Town
        {
            PositionList = Locations.FishingTownPositions;
        }
        else if (ScenePatches.currentScene == 5) // Snowy Peaks
        {
            PositionList = Locations.SnowyPeaksPositions;
        }

        if (ArchipelagoClient.Authenticated && PositionList.TryGetValue($"{pos}", out PositionData posData))
        {
            locLabel = posData.LocationId;

            APGUI.Warning($"{locLabel}");
            if (archipelago.session.Locations.AllLocationsChecked.Contains(locLabel))
            {
                APGUI.Warning($"Done");
                // __1 = $"*{__1}";
                doneLabel = true;
            }
        }
    }

    private static readonly List<string> doneLabels = ["throw away", "buy"];

    [HarmonyPatch(typeof(LocalizationDictionary), "Translate")]
    private static void Postfix(string __0, ref string __result)
    {
        if (doneLabel && doneLabels.Contains(__0))
        {
            __result = $"*{__result}";
        }
        else if (doneLabels.Contains(__0) && archipelago.session.Locations.AllLocations.Contains(locLabel))
        {
            __result = $"{__result} {ArchipelagoClient.scoutedItemInfo[locLabel].ItemDisplayName}";
        }
    }

    [HarmonyPatch(typeof(sSaveSystem), "OnApplicationQuit")]
    private static void Postfix()
    {
        archipelago.Disconnect();
    }
}