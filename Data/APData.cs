using System.Collections.Generic;
using Archipelago.MultiClient.Net.Models;
using BepInEx;
using EasyDeliveryAP.Archipelago;

namespace EasyDeliveryAP;

struct APSaveFile
{
    public int handledIndex;
    public string APSeed;
    public string uri;
    public string slotName;
    public string password;
    public string modVersion;
}

public class APData
{
    private static readonly ArchipelagoClient archipelago = EasyDeliveryAP.ArchipelagoClient;

    // APWorld Settings
    public static string payload_checks;
    public static string perfect_deliveries;
    public static string intercity_deliveries;
    public static string snowcats;
    public static string blind_bags;
    public static string lock_towns;
    public static string blocked_tunnels;
    public static string require_handheld_radio;
    public static string radio_towers;
    public static string car_upgrades;
    public static string progressive_car_upgrades;
    public static int car_trail_color;
    public static string vending_machines;
    public static string trash_bins;
    public static string signs;

    // Other
    public static Hint[] hints;

    public static void SetSlotSettings(Dictionary<string, object> slotData)
    {
        slotData.TryGetValue("payload_checks", out object Payload_checks);
        payload_checks = Payload_checks.ToString();
        slotData.TryGetValue("perfect_deliveries", out object Perfect_deliveries);
        perfect_deliveries = Perfect_deliveries.ToString();
        slotData.TryGetValue("intercity_deliveries", out object Intercity_deliveries);
        intercity_deliveries = Intercity_deliveries.ToString();
        slotData.TryGetValue("snowcats", out object Snowcats);
        snowcats = Snowcats.ToString();
        slotData.TryGetValue("blind_bags", out object Blind_bags);
        blind_bags = Blind_bags.ToString();
        slotData.TryGetValue("lock_towns", out object Lock_towns);
        lock_towns = Lock_towns.ToString();
        slotData.TryGetValue("blocked_tunnels", out object Blocked_tunnels);
        blocked_tunnels = Blocked_tunnels.ToString();
        slotData.TryGetValue("require_handheld_radio", out object Handheld_radio);
        require_handheld_radio = Handheld_radio.ToString();
        slotData.TryGetValue("radio_towers", out object Radio_towers);
        radio_towers = Radio_towers.ToString();
        slotData.TryGetValue("car_upgrades", out object Car_upgrades);
        car_upgrades = Car_upgrades.ToString();
        if (slotData.TryGetValue("progressive_car_upgrades", out object Progressive_car_upgrades))
        {
            progressive_car_upgrades = Progressive_car_upgrades.ToString();
        }
        else
        {
            progressive_car_upgrades = "0";
        }
        if (slotData.TryGetValue("randomize_trail_color", out object Car_trail_color))
        {
            car_trail_color = int.Parse(Car_trail_color.ToString());
        }
        else
        {
            car_trail_color = 0xFFFFFF;
        }
        if (slotData.TryGetValue("vending_machines", out object Vending_machines))
        {
            vending_machines = Vending_machines.ToString();
        }
        else
        {
            vending_machines = "0";
        }
        if (slotData.TryGetValue("trash_bins", out object Trash_bins))
        {
            trash_bins = Trash_bins.ToString();
        }
        else
        {
            trash_bins = "0";
        }
        if (slotData.TryGetValue("signs", out object Signs))
        {
            signs = Signs.ToString();
        }
        else
        {
            signs = "0";
        }


        if (intercity_deliveries == "2" || intercity_deliveries == "3")
        {
            TrackerApp.pages.Add(2);
        }
        if (payload_checks == "1")
        {
            TrackerApp.pages.Add(3);
        }
        if (snowcats == "1" || snowcats == "2")
        {
            TrackerApp.pages.Add(4);
        }
        if (blind_bags == "1")
        {
            TrackerApp.pages.Add(5);
        }
        if (radio_towers == "1" || radio_towers == "2")
        {
            TrackerApp.pages.Add(6);
        }
        if (vending_machines == "1")
        {
            TrackerApp.pages.Add(7);
        }
        if (trash_bins == "1")
        {
            TrackerApp.pages.Add(8);
        }
        if (signs == "1")
        {
            TrackerApp.pages.Add(9);
        }
    }

    public static void SaveConnectionOrConnect()
    {
        if (ArchipelagoClient.Authenticated)
        {
            EasyDeliveryAP.save.data.uri = ArchipelagoClient.ServerData.Uri;
            EasyDeliveryAP.save.data.slotName = ArchipelagoClient.ServerData.SlotName;
            EasyDeliveryAP.save.data.password = ArchipelagoClient.ServerData.Password;
            EasyDeliveryAP.save.data.modVersion = EasyDeliveryAP.PluginVersion;
            EasyDeliveryAP.save.data.APSeed = ArchipelagoClient.ServerData.seed;
        }
        else if (!EasyDeliveryAP.save.data.slotName.IsNullOrWhiteSpace() && EasyDeliveryAP.configAutoConnect.Value)
        {
            ArchipelagoClient.ServerData.Uri = EasyDeliveryAP.save.data.uri;
            ArchipelagoClient.ServerData.SlotName = EasyDeliveryAP.save.data.slotName;
            ArchipelagoClient.ServerData.Password = EasyDeliveryAP.save.data.password;
            archipelago.Connect();
        }
    }

    public static void OnHintsReceived(Hint[] hint)
    {
        hints = hint;
    }

    public static bool IsHinted(int locationId, bool hinted = false)
    {
        if (hinted) return true;

        foreach (var hint in hints)
        {
            if (hint.FindingPlayer == archipelago.session.Players.ActivePlayer.Slot && hint.LocationId == locationId)
            {
                return true;
            }
        }

        return false;
    }
}