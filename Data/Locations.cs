using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

namespace EasyDeliveryAP;

public class Locations
{
    public static readonly Dictionary<string, int> Town = new()
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

    public static readonly Dictionary<string, int> PayloadDeliveries = new()
    {
        {"PAYLOAD Big Box", 1},
        {"PAYLOAD BigBoxStack", 2},
        {"PAYLOAD BoxBunch", 3},
        {"PAYLOAD BoxStack", 4},
        {"PAYLOAD Crate", 5},
        {"PAYLOAD CrateOfDrinks", 6},
        {"PAYLOAD CrateStack", 7},
        {"PAYLOAD LotsOfCratesOfDrinks", 8},
        {"PAYLOAD PizzaStack", 9},
        {"PAYLOAD PizzaStackMega", 10},
        {"PAYLOAD PlantPot", 11},
        {"PAYLOAD PlantPotBunch", 12},
        {"PAYLOAD PlantPotStack", 13},
        {"PAYLOAD PlantPotWide", 14},
        {"PAYLOAD Sack", 15},
        {"PAYLOAD SackStack", 16},
        {"Drink", 17},
    };
}
