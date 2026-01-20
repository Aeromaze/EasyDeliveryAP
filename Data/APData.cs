using System.Collections.Generic;

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
    // APWorld Settings
    public static string payload_checks;
    public static string perfect_deliveries;
    public static string blind_bags;
    public static string blocked_tunnels;
    public static string require_handheld_radio;
    public static string radio_towers;
    public static string car_upgrades;

    public static void SetSlotSettings(Dictionary<string, object> slotData)
    {
            slotData.TryGetValue("payload_checks", out object Payload_checks);
            payload_checks = Payload_checks.ToString();
            slotData.TryGetValue("perfect_deliveries", out object Perfect_deliveries);
            perfect_deliveries = Perfect_deliveries.ToString();
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
}