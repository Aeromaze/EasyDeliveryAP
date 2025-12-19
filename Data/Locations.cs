using System.Collections.Generic;
using HarmonyLib;

namespace EasyDeliveryAP;

public class Locations
{
    public static Dictionary<string, int> Town = new Dictionary<string, int>()
    {
        {"Upton", 11},
        {"Weston", 12},
        {"Easton", 13},
        {"Winton", 21},
        {"Munton", 22},
        {"Lopton", 23},
        {"Clifton", 31},
        {"Damton", 32},
        {"Smalton", 33},
    };

    public static Dictionary<string, int> GetDeliveries()
    {
        Dictionary<string, int> deliveries = [];

        foreach (KeyValuePair<string, int> startTown in Town)
        {
            foreach(KeyValuePair<string, int> endTown in Town)
            {
                deliveries.Add($"{startTown.Key} to {endTown.Key} Delivery", int.Parse($"{startTown.Value}{endTown.Value}"));
            }
        }

        return deliveries;
    }
}
