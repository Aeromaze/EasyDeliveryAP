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
    public static string snowcats;
    public static string blind_bags;
    public static string blocked_tunnels;
    public static string require_handheld_radio;
    public static string radio_towers;
    public static string car_upgrades;

    // Other
    public static Hint[] hints;

    public static void SetSlotSettings(Dictionary<string, object> slotData)
    {
        slotData.TryGetValue("payload_checks", out object Payload_checks);
        payload_checks = Payload_checks.ToString();
        slotData.TryGetValue("perfect_deliveries", out object Perfect_deliveries);
        perfect_deliveries = Perfect_deliveries.ToString();
        slotData.TryGetValue("snowcats", out object Snowcats);
        snowcats = Snowcats.ToString();
        slotData.TryGetValue("blind_bags", out object Blind_bags);
        blind_bags = Blind_bags.ToString();
        slotData.TryGetValue("blocked_tunnels", out object Blocked_tunnels);
        blocked_tunnels = Blocked_tunnels.ToString();
        slotData.TryGetValue("require_handheld_radio", out object Handheld_radio);
        require_handheld_radio = Handheld_radio.ToString();
        slotData.TryGetValue("radio_towers", out object Radio_towers);
        radio_towers = Radio_towers.ToString();
        slotData.TryGetValue("car_upgrades", out object Car_upgrades);
        car_upgrades = Car_upgrades.ToString();
    }

    public static void SaveConnectionOrConnect()
    {
        if (ArchipelagoClient.Authenticated)
        {
            EasyDeliveryAP.save.data.uri = ArchipelagoClient.ServerData.Uri;
            EasyDeliveryAP.save.data.slotName = ArchipelagoClient.ServerData.SlotName;
            EasyDeliveryAP.save.data.password = ArchipelagoClient.ServerData.Password;
            EasyDeliveryAP.save.data.modVersion = EasyDeliveryAP.PluginVersion;
        }
        else if (!EasyDeliveryAP.save.data.slotName.IsNullOrWhiteSpace() && EasyDeliveryAP.configAutoConnect.Value)
        {
            ArchipelagoClient.ServerData.Uri = EasyDeliveryAP.save.data.uri;
            ArchipelagoClient.ServerData.SlotName = EasyDeliveryAP.save.data.slotName;
            ArchipelagoClient.ServerData.Password = EasyDeliveryAP.save.data.password;
            archipelago.Connect();
        }
    }

    public static void UpdateHints()
    {
        if (!ArchipelagoClient.Authenticated) return;
        hints = archipelago.session.Hints.GetHints();
    }

    public static bool IsHinted(int locationId, bool hinted)
    {
        if (hinted) return true;

        var Hints = hints;

        foreach (var hint in Hints)
        {
            if (hint.FindingPlayer == archipelago.session.Players.ActivePlayer.Slot && hint.LocationId == locationId)
            {
                return true;
            }
        }

        return false;
    }
}