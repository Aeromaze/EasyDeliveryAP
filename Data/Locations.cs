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

    private static Dictionary<string, int> GetDeliveries()
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

    public static Dictionary<string, int> Deliveries = GetDeliveries();

    public static readonly Dictionary<string, int> PayloadDeliveries = new()
    {
        {"PAYLOAD Big Box", 1},
        {"PAYLOAD BixBoxStack", 2},
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

    public static readonly Dictionary<string, int> BlindBags = new()
    {
        {"1Blind Bag", 20},
        {"1Blind Bag (1)", 21},
        {"1Blind Bag (2)", 22},
        {"1Blind Bag (3)", 23},
        {"5Blind Bag", 24},
        {"5Blind Bag (1)", 25},
        {"5Blind Bag (2)", 26},
        {"5Blind Bag (3)", 27},
        {"4Blind Bag", 28},
        {"4Blind Bag (1)", 29},
        {"4Blind Bag (2)", 30},
        {"4Blind Bag (3)", 31},
    };

    // Mountain Town nodes
    public class MountainTown
    {
        public static NodeData EasyFlowers = new("Easy Flowers", "Upton", 0);
        public static NodeData Node57 = new("Node (57)", "Upton", 1);
        public static NodeData Node48 = new("Node (48)", "Upton", 2);
        public static NodeData Node56 = new("Node (56)", "Upton", 3);
        public static NodeData Node55 = new("Node (55)", "Upton", 4);
        public static NodeData EasyEats = new("Easy Eats", "Upton", 5);
        public static NodeData Node47 = new("Node (47)", "Upton", 6);
        public static NodeData Node54 = new("Node (54)", "Upton", 7);
        public static NodeData Node5 = new("Node (5)", "Weston", 8);
        public static NodeData EZBakery = new("EZ Bakery", "Weston", 9);
        public static NodeData Node59 = new("Node (59)", "Weston", 10);
        public static NodeData Node60 = new("Node (60)", "Weston", 11);
        public static NodeData Bar1 = new("Bar", "Weston", 12);
        public static NodeData EasyDepot = new("Node (57)", "Weston", 13);
        public static NodeData Bar2 = new("Bar 2", "Weston", 14);
        public static NodeData EZCafe = new("EZ Cafe", "Weston", 15);
        public static NodeData Node65 = new("Node (65)", "Weston", 16);
        public static NodeData Node71 = new("Node (71)", "Easton", 17);
        public static NodeData Node72 = new("Node (72)", "Easton", 18);
        public static NodeData EasyPizza = new("Easy Pizza", "Easton", 19);
        public static NodeData Node74 = new("Node (74)", "Easton", 20);
        public static NodeData PawnShop = new("Pawn Shop", "Easton", 21);
        public static NodeData EZAuto = new("EZ Auto", "Easton", 22);
        public static NodeData EZMart = new("EZ Mart", "Easton", 23);
        public static NodeData Node92 = new("Node (92)", "Upton", 24);
        public static NodeData Node93 = new("Node (93)", "Upton", 25);
    }

    // Snowy Peaks nodes
    public class SnowyPeaks
    {
        public static NodeData Destnode5 = new("Destnode5", "Lopton", 0);
        public static NodeData Destnode1 = new("Destnode1", "Lopton", 1);
        public static NodeData Destnode3 = new("Destnode3", "Lopton", 2);
        public static NodeData Destnode2 = new("Destnode2", "Lopton", 3);
        public static NodeData Destnode4 = new("Destnode4", "Lopton", 4);
        public static NodeData Destnode6 = new("Destnode6", "Lopton", 5);
        public static NodeData Destnode = new("Destnode", "Lopton", 6);
        public static NodeData Destnode10 = new("Destnode10", "Munton", 7);
        public static NodeData Destnode8 = new("Destnode8", "Lopton", 8);
        public static NodeData Destnode7 = new("Destnode7", "Lopton", 9);
        public static NodeData Destnode9 = new("Destnode9", "Winton", 10);
        public static NodeData EasyEats = new("Easy Eats", "Winton", 11);
        public static NodeData EZBakery = new("EZ Bakery", "Lopton", 12);
        public static NodeData EasyPizza = new("Easy Pizza", "Lopton", 13);
        public static NodeData EasyFlowers = new("Easy Flowers", "Winton", 14);
        public static NodeData Bar1 = new("Bar 1", "Lopton", 15);
        public static NodeData EasyDepot = new("Easy Depot", "Lopton", 16);
        public static NodeData EZAuto = new("EZ Auto", "Munton", 17);
        public static NodeData EZCafe = new("EZ Cafe", "Lopton", 18);
        public static NodeData Bar2 = new("Bar 2", "Munton", 19);
        public static NodeData EZMart = new("EZ Mart", "Munton", 20);
        public static NodeData PawnShop = new("Pawn Shop", "Munton", 21);
    }

    // Fishing Town
    public class FishingTown
    {
        public static NodeData Node93 = new("Node (93)", "Clifton", 0);
        public static NodeData Nodedock = new("Node(dock)", "Smalton", 1);
        public static NodeData Node59 = new("Node (59)", "Damton", 2);
        public static NodeData Node101 = new("Node (101)", "Clifton", 3);
        public static NodeData Nodedock2 = new("Node (dock2)", "Smalton", 4);
        public static NodeData Node11 = new("Node (11)", "Damton", 5);
        public static NodeData Nodedock31 = new("Node (dock3) (1)", "Damton", 6);
        public static NodeData Node58 = new("Node (58)", "Damton", 7);
        public static NodeData Node91 = new("Node (91)", "Clifton", 8);
        public static NodeData Node54 = new("Node (54)", "Damton", 9);
        public static NodeData Node18 = new("Node (18)", "Damton", 10);
        public static NodeData Node64 = new("Node (64)", "Damton", 11);
        public static NodeData Node97 = new("Node (97)", "Clifton", 12);
        public static NodeData Node20 = new("Node (20)", "Damton", 13);
        public static NodeData Node22 = new("Node (22)", "Damton", 14);
        public static NodeData Node89 = new("Node (89)", "Clifton", 15);
        public static NodeData Node55 = new("Node (55)", "Damton", 16);
        public static NodeData Node112 = new("Node (112)", "Damton", 17);
        public static NodeData Node12 = new("Node (12)", "Damton", 18);
        public static NodeData Nodedock3 = new("Node (dock3)", "Damton", 19);
        public static NodeData Nodedock1 = new("Node(dock)(1)", "Smalton", 20);
        public static NodeData Node14 = new("Node (14)", "Smalton", 21);
        public static NodeData Nodedock21 = new("Node (dock2) (1)", "Smalton", 22);
        public static NodeData Node102 = new("Node (102)", "Clifton", 23);
        public static NodeData Node98 = new("Node (98)", "Clifton", 24);
        public static NodeData EasyFlowers = new("Easy Flowers", "Smalton", 25);
        public static NodeData EasyEats = new("Easy Eats", "Smalton", 26);
        public static NodeData PawnShop = new("Pawn Shop", "Clifton", 27);
        public static NodeData EZMart = new("EZ Mart", "Clifton", 28);
        public static NodeData Bar2 = new("Bar", "Damton", 29);
        public static NodeData EasyDepot = new("Easy Depot", "Damton", 30);
        public static NodeData Bar1 = new("Bar", "Clifton", 31);
        public static NodeData EZCafe = new("EZ Cafe", "Damton", 32);
        public static NodeData EasyPizza = new("Easy Pizza", "Smalton", 33);
        public static NodeData EZAuto = new("EZ Auto", "Clifton", 34);
        public static NodeData EZBakery = new("EZ Bakery", "Damton", 35);
    }

    public static readonly Dictionary<string, NodeData> MountainTownNode = new()
    {
        {"Easy Flowers", MountainTown.EasyFlowers},
        {"Node (57)", MountainTown.Node57},
        {"Node (48)", MountainTown.Node48},
        {"Node (56)", MountainTown.Node56},
        {"Node (55)", MountainTown.Node55},
        {"Easy Eats", MountainTown.EasyEats},
        {"Node (47)", MountainTown.Node47},
        {"Node (54)", MountainTown.Node54},
        {"Node (5)", MountainTown.Node5},
        {"EZ Bakery", MountainTown.EZBakery},
        {"Node (59)", MountainTown.Node59},
        {"Node (60)", MountainTown.Node60},
        {"Bar 1", MountainTown.Bar1},
        {"Easy Depot", MountainTown.EasyDepot},
        {"Bar 2", MountainTown.Bar2},
        {"EZ Cafe", MountainTown.EZCafe},
        {"Node (65)", MountainTown.Node65},
        {"Node (71)", MountainTown.Node71},
        {"Node (72)", MountainTown.Node72},
        {"Easy Pizza", MountainTown.EasyPizza},
        {"Node (74)", MountainTown.Node74},
        {"Pawn Shop", MountainTown.PawnShop},
        {"EZ Auto", MountainTown.EZAuto},
        {"EZ Mart", MountainTown.EZMart},
        {"Node (92)", MountainTown.Node92},
        {"Node (93)", MountainTown.Node93},
    };

    public static readonly Dictionary<int, NodeData> MountainTownIndex = new()
    {
        {0, MountainTown.EasyFlowers},
        {1, MountainTown.Node57},
        {2, MountainTown.Node48},
        {3, MountainTown.Node56},
        {4, MountainTown.Node55},
        {5, MountainTown.EasyEats},
        {6, MountainTown.Node47},
        {7, MountainTown.Node54},
        {8, MountainTown.Node5},
        {9, MountainTown.EZBakery},
        {10, MountainTown.Node59},
        {11, MountainTown.Node60},
        {12, MountainTown.Bar1},
        {13, MountainTown.EasyDepot},
        {14, MountainTown.Bar2},
        {15, MountainTown.EZCafe},
        {16, MountainTown.Node65},
        {17, MountainTown.Node71},
        {18, MountainTown.Node72},
        {19, MountainTown.EasyPizza},
        {20, MountainTown.Node74},
        {21, MountainTown.PawnShop},
        {22, MountainTown.EZAuto},
        {23, MountainTown.EZMart},
        {24, MountainTown.Node92},
        {25, MountainTown.Node93},
    };

    public static readonly Dictionary<string, NodeData> SnowyPeaksNode = new()
    {
        {"Destnode5", SnowyPeaks.Destnode5},
        {"Destnode1", SnowyPeaks.Destnode1},
        {"Destnode3", SnowyPeaks.Destnode3},
        {"Destnode2", SnowyPeaks.Destnode2},
        {"Destnode4", SnowyPeaks.Destnode4},
        {"Destnode6", SnowyPeaks.Destnode6},
        {"Destnode", SnowyPeaks.Destnode},
        {"Destnode10", SnowyPeaks.Destnode10},
        {"Destnode8", SnowyPeaks.Destnode8},
        {"Destnode7", SnowyPeaks.Destnode7},
        {"Destnode9", SnowyPeaks.Destnode9},
        {"Easy Eats", SnowyPeaks.EasyEats},
        {"EZ Bakery", SnowyPeaks.EZBakery},
        {"Easy Pizza", SnowyPeaks.EasyPizza},
        {"Easy Flowers", SnowyPeaks.EasyFlowers},
        {"Bar 1", SnowyPeaks.Bar1},
        {"Easy Depot", SnowyPeaks.EasyDepot},
        {"EZ Auto", SnowyPeaks.EZAuto},
        {"EZ Cafe", SnowyPeaks.EZCafe},
        {"Bar 2", SnowyPeaks.Bar2},
        {"EZ Mart", SnowyPeaks.EZMart},
        {"Pawn Shop", SnowyPeaks.PawnShop},
    };

    public static readonly Dictionary<int, NodeData> SnowyPeaksIndex = new()
    {
        {0, SnowyPeaks.Destnode5},
        {1, SnowyPeaks.Destnode1},
        {2, SnowyPeaks.Destnode3},
        {3, SnowyPeaks.Destnode2},
        {4, SnowyPeaks.Destnode4},
        {5, SnowyPeaks.Destnode6},
        {6, SnowyPeaks.Destnode},
        {7, SnowyPeaks.Destnode10},
        {8, SnowyPeaks.Destnode8},
        {9, SnowyPeaks.Destnode7},
        {10, SnowyPeaks.Destnode9},
        {11, SnowyPeaks.EasyEats},
        {12, SnowyPeaks.EZBakery},
        {13, SnowyPeaks.EasyPizza},
        {14, SnowyPeaks.EasyFlowers},
        {15, SnowyPeaks.Bar1},
        {16, SnowyPeaks.EasyDepot},
        {17, SnowyPeaks.EZAuto},
        {18, SnowyPeaks.EZCafe},
        {19, SnowyPeaks.Bar2},
        {20, SnowyPeaks.EZMart},
        {21, SnowyPeaks.PawnShop},
    };

    public static readonly Dictionary<string, NodeData> FishingTownNode = new()
    {
        {"Node93", FishingTown.Node93},
        {"Nodedock", FishingTown.Nodedock},
        {"Node59", FishingTown.Node59},
        {"Node101", FishingTown.Node101},
        {"Nodedock2", FishingTown.Nodedock2},
        {"Node11", FishingTown.Node11},
        {"Nodedock31", FishingTown.Nodedock31},
        {"Node58", FishingTown.Node58},
        {"Node91", FishingTown.Node91},
        {"Node54", FishingTown.Node54},
        {"Node18", FishingTown.Node18},
        {"Node64", FishingTown.Node64},
        {"Node97", FishingTown.Node97},
        {"Node20", FishingTown.Node20},
        {"Node22", FishingTown.Node22},
        {"Node89", FishingTown.Node89},
        {"Node55", FishingTown.Node55},
        {"Node112", FishingTown.Node112},
        {"Node12", FishingTown.Node12},
        {"Nodedock3", FishingTown.Nodedock3},
        {"Nodedock1", FishingTown.Nodedock1},
        {"Node14", FishingTown.Node14},
        {"Nodedock21", FishingTown.Nodedock21},
        {"Node102", FishingTown.Node102},
        {"Node98", FishingTown.Node98},
        {"EasyFlowers", FishingTown.EasyFlowers},
        {"EasyEats", FishingTown.EasyEats},
        {"PawnShop", FishingTown.PawnShop},
        {"EZMart", FishingTown.EZMart},
        {"Bar2", FishingTown.Bar2},
        {"EasyDepot", FishingTown.EasyDepot},
        {"Bar1", FishingTown.Bar1},
        {"EZCafe", FishingTown.EZCafe},
        {"EasyPizza", FishingTown.EasyPizza},
        {"EZAuto", FishingTown.EZAuto},
        {"EZBakery", FishingTown.EZBakery},
    };

    public static readonly Dictionary<int, NodeData> FishingTownIndex = new()
    {
        {0, FishingTown.Node93},
        {1, FishingTown.Nodedock},
        {2, FishingTown.Node59},
        {3, FishingTown.Node101},
        {4, FishingTown.Nodedock2},
        {5, FishingTown.Node11},
        {6, FishingTown.Nodedock31},
        {7, FishingTown.Node58},
        {8, FishingTown.Node91},
        {9, FishingTown.Node54},
        {10, FishingTown.Node18},
        {11, FishingTown.Node64},
        {12, FishingTown.Node97},
        {13, FishingTown.Node20},
        {14, FishingTown.Node22},
        {15, FishingTown.Node89},
        {16, FishingTown.Node55},
        {17, FishingTown.Node112},
        {18, FishingTown.Node12},
        {19, FishingTown.Nodedock3},
        {20, FishingTown.Nodedock1},
        {21, FishingTown.Node14},
        {22, FishingTown.Nodedock21},
        {23, FishingTown.Node102},
        {24, FishingTown.Node98},
        {25, FishingTown.EasyFlowers},
        {26, FishingTown.EasyEats},
        {27, FishingTown.PawnShop},
        {28, FishingTown.EZMart},
        {29, FishingTown.Bar2},
        {30, FishingTown.EasyDepot},
        {31, FishingTown.Bar1},
        {32, FishingTown.EZCafe},
        {33, FishingTown.EasyPizza},
        {34, FishingTown.EZAuto},
        {35, FishingTown.EZBakery},
    };

}

public class NodeData
{
    public string Name { get; set; }

    public string Town { get; set; }

    public int Index { get; set; }

    public NodeData()
    {
    }

    public NodeData(string name, string town, int index)
    {
        Name = name;
        Town = town;
        Index = index;
    }
}
