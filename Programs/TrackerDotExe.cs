using EasyDeliveryAP.Archipelago;
using EasyDeliveryAPI;
using UnityEngine;

namespace EasyDeliveryAP;

public class TrackerDotExe : ScreenProgram
{
    GamepadNavigation nav;
    private AudioSource audioSource;
    public AudioClip select;

    public override void Setup()
    {
        this.mouseIcon = 112;
        nav = new GamepadNavigation(this, this.audioSource, this.select);
    }

    public override void Resume()
    {
        
    }

    public override void Draw()
    {
        UIUtil Util = new(R, this, nav);
        Util.drawBox(1f, 2f, 3f, 4f, false);
        if (this.backButtonDown)
        {
            this.screenSystem.SetMenu(1);
            this.screenSystem.OpenMenu();
        }
    }
}

public class TrackerText
{
    private static readonly ArchipelagoClient archipelago = EasyDeliveryAP.ArchipelagoClient;
    static string trackerItemsText;
    static string trackerChecksText;
    static int donePayloads;
    static int deliveries;
    static int blindBags;
    static int snowcats;
    static int radioTowers;

    public static void UpdateTracker()
    {
        DesktopDotExe.File file = EasyDeliveryAP.TrackerFile;
        trackerItemsText = "Items received:\n";
        trackerChecksText = "Locations checked:\n";
        donePayloads = 0;
        deliveries = 0;
        blindBags = 0;
        snowcats = 0;
        radioTowers = 0;

        if (!ArchipelagoClient.Authenticated)
        {
            file.data = "Not connected";
            return;
        }

        var checkedLocations = archipelago.session.Locations.AllLocationsChecked;

        trackerItemsText += ($"{(APData.car_upgrades == "0" ? "  (Buy upgrades)" : "(Direct upgrades)")}\n" +
                             $"  Snow Tires: {(Items.Tires.Received > 0 ? "Received" : "Missing")}\n" +
                             $"  Bumper Bar: {(Items.Bumper.Received > 0 ? "Received" : "Missing")}\n" +
                             $"  Ice Chains: {(Items.Chains.Received > 0 ? "Received" : "Missing")}\n");

        if (APData.blocked_tunnels != "0")
        {
            trackerItemsText += $"  Tunnels: {(Items.TunnelSP.Received > 0 ? "Snowy Peaks, " : "")}{(Items.TunnelFT.Received > 0 ? "Fishing Town, " : "")}{(Items.TunnelFactory.Received > 0 ? "Factory" : "")}\n";
        }

        foreach (int id in Locations.Deliveries.Values)
        {
            if (checkedLocations.Contains(id)) deliveries += 1;
            if (checkedLocations.Contains(id + 10000)) deliveries += 1;
        }

        trackerChecksText += $"  {(APData.perfect_deliveries == "2" ? "Perfect " : "")}Deliveries: {deliveries}/{(APData.perfect_deliveries == "1" ? "162" : "81")}\n";

        if (APData.payload_checks == "1")
        {
            foreach (int id in Locations.PayloadDeliveries.Values)
            {
                if (checkedLocations.Contains(id)) donePayloads += 1;
            }
            trackerChecksText += $"  Payload Deliveries: {donePayloads}/17\n";
        }

        switch (APData.radio_towers)
        {
            case "1":
                trackerItemsText += $"  Radio Towers: {Items.RadioTower.Received}\n";
                for (int id = 60; id < 64; id++)
                {
                    if (checkedLocations.Contains(id)) radioTowers += 1;
                }
                trackerChecksText += $"  Radio Towers: {radioTowers}/4\n";
                break;
            case "2":
                for (int id = 60; id < 64; id++)
                {
                    if (checkedLocations.Contains(id)) radioTowers += 1;
                }
                trackerChecksText += $"  Radio Towers: {radioTowers}/4\n";
                break;
            case "3":
                trackerItemsText += $"  Radio Towers: {Items.RadioTower.Received}\n";
                break;
        }

        if (APData.blind_bags == "1")
        {
            foreach (int id in Locations.BlindBags.Values)
            {
                if (checkedLocations.Contains(id)) blindBags += 1;
            }
            trackerChecksText += $"  Blind Bags: {blindBags}/12\n";
        }

        if (APData.snowcats == "1" || APData.snowcats == "2")
        {
            for (int id = 40; id < 53; id++)
            {
                if (checkedLocations.Contains(id)) snowcats += 1;
            }
            trackerChecksText += $"  Snowcats: {snowcats}/{(APData.snowcats == "1" ? "13" : "12")}\n";
        }

        file.data = trackerItemsText + trackerChecksText;
    }
}