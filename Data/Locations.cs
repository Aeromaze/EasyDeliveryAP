using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

    public static readonly Dictionary<string, int> City = new()
    {
        {"Mountain Town", 10},
        {"Snowy Peaks", 20},
        {"Fishing Town", 30},
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
        foreach (KeyValuePair<string, int> startCity in City)
        {
            foreach(KeyValuePair<string, int> endCity in City)
            {
                deliveries.Add($"{startCity.Key} to {endCity.Key} Delivery", int.Parse($"{startCity.Value}{endCity.Value}"));
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

    public class MountainTown
    {
        // Mountain Town nodes
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

        // Mountain Town bins and vending machines
        // Upton
        public static PositionData VMUEats = new("VMUEats", "Upton", 100, "Vending Machine");
        public static PositionData RBUFlowers = new("RBUFlowers", "Upton", 121, "Bin");
        public static PositionData BBUFlowers = new("BBUFlowers", "Upton", 122, "Bin");
        public static PositionData BBUFlowersEats = new("BBUFlowersEats", "Upton", 123, "Bin");
        public static PositionData BBUEats = new("BBUEats", "Upton", 124, "Bin");
        public static PositionData RBUEats = new("RBUEats", "Upton", 125, "Bin");
        public static PositionData RBUTownEdge = new("RBUTownEdge", "Upton", 126, "Bin");
        // Weston
        public static PositionData BBWEntry = new("BBWEntry", "Weston", 127, "Bin");
        public static PositionData RBWEntry = new("RBWEntry", "Weston", 128, "Bin");
        public static PositionData BBWBakeryAcross = new("BBWBakeryAcross", "Weston", 129, "Bin");
        public static PositionData RBWBakeryLeft = new("RBWBakeryLeft", "Weston", 130, "Bin");
        public static PositionData BBWBakeryLeft = new("BBWBakeryLeft", "Weston", 131, "Bin");
        public static PositionData RBWBarTunnel = new("RBWBarTunnel", "Weston", 132, "Bin");
        public static PositionData VMWBarTunnel = new("VMWBarTunnel", "Weston", 101, "Vending Machine");
        public static PositionData BBWDepot = new("BBWDepot", "Weston", 133, "Bin");
        public static PositionData BBWBarLower = new("BBWBarLower", "Weston", 134, "Bin");
        public static PositionData RBWCafe = new("RBWCafe", "Weston", 135, "Bin");
        // Easton
        public static PositionData BBEMart = new("BBEMart", "Easton", 136, "Bin");
        public static PositionData RBEFuel = new("RBEFuel", "Easton", 137, "Bin");
        public static PositionData BBEFuel = new("BBEFuel", "Easton", 138, "Bin");
        public static PositionData VMEFuel = new("VMEFuel", "Easton", 102, "Vending Machine");
        public static PositionData BBEPizzaRight = new("BBEPizzaRight", "Easton", 139, "Bin");
        public static PositionData RBEPizzaLeft = new("RBEPizzaLeft", "Easton", 140, "Bin");
    }

    public class SnowyPeaks
    {
        // Snowy Peaks nodes
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

        // Snowy Peaks bins and vending machines
        // Winton
        public static PositionData BBWFlowers = new("BBWFlowers", "Winton", 141, "Bin");
        public static PositionData RBWEats = new("RBWEats", "Winton", 142, "Bin");
        public static PositionData VMWFuel = new("VMWFuel", "Winton", 103, "Vending Machine");
        public static PositionData BBWFuel = new("BBWFuel", "Winton", 143, "Bin");
        public static PositionData RBWFuel = new("RBWFuel", "Winton", 144, "Bin");
        // Munton
        public static PositionData VMMAuto = new("VMMAuto", "Munton", 104, "Vending Machine");
        public static PositionData RBMPawn = new("RBMPawn", "Munton", 145, "Bin");
        public static PositionData RBMMart = new("RBMMart", "Munton", 146, "Bin");
        public static PositionData BBMMart = new("BBMMart", "Munton", 147, "Bin");
        public static PositionData VMMBar = new("VMMBar", "Munton", 105, "Vending Machine");
        // Lopton
        public static PositionData RBLBar = new("RBLBar", "Lopton", 148, "Bin");
        public static PositionData VMLDepot = new("VMLDepot", "Lopton", 106, "Vending Machine");
        public static PositionData RBLDepot = new("RBLDepot", "Lopton", 149, "Bin");
        public static PositionData BBLDepot = new("BBLDepot", "Lopton", 150, "Bin");
    }

    public class FishingTown
    {
        // Fishing Town nodes
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

        // Fishing Town bins and vending machines
        // Clifton
        public static PositionData RBCBar = new("RBCBar", "Clifton", 151, "Bin");
        public static PositionData BBCBar = new("BBCBar", "Clifton", 152, "Bin");
        public static PositionData VMCMart = new("VMCMart", "Clifton", 107, "Vending Machine");
        public static PositionData BBCMart = new("BBCMart", "Clifton", 153, "Bin");
        public static PositionData RBCMart = new("RBCMart", "Clifton", 154, "Bin");
        // Damton
        public static PositionData RBDBar = new("RBDBar", "Damton", 155, "Bin");
        public static PositionData BBDBar = new("BBDBar", "Damton", 156, "Bin");
        public static PositionData VMDBar = new("VMDBar", "Damton", 108, "Vending Machine");
        public static PositionData RBDDepot = new("RBDDepot", "Damton", 157, "Bin");
        public static PositionData BBDDepot = new("BBDDepot", "Damton", 158, "Bin");
        public static PositionData VMDDepot = new("VMDDepot", "Damton", 109, "Vending Machine");
        // Smalton
        public static PositionData BBSPizza = new("BBSPizza", "Smalton", 159, "Bin");
        public static PositionData RBSPizza = new("RBSPizza", "Smalton", 160, "Bin");
        public static PositionData VMSEatsLeft = new("VMSEatsLeft", "Smalton", 110, "Vending Machine");
        public static PositionData VMSEatsRight = new("VMSEatsRight", "Smalton", 111, "Vending Machine");
        public static PositionData RBSEats = new("RBSEats", "Smalton", 161, "Bin");
        public static PositionData BBSEats = new("BBSEats", "Smalton", 162, "Bin");
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

    public static readonly Dictionary<string, PositionData> MountainTownPositions = new()
    {
        // Upton
        {"(-323.89, 326.97, -278.76)", MountainTown.VMUEats}, // VMUEats
        {"(-379.38, 326.80, -291.69)", MountainTown.RBUFlowers}, // RBUFlowers
        {"(-378.53, 326.80, -290.91)", MountainTown.BBUFlowers}, // BBUFlowers
        {"(-355.97, 326.82, -288.83)", MountainTown.BBUFlowersEats}, // BBUFlowersEats
        {"(-326.58, 326.80, -279.27)", MountainTown.BBUEats}, // BBUEats
        {"(-325.83, 326.79, -279.25)", MountainTown.RBUEats}, // RBUEats
        {"(-267.40, 328.12, -278.91)", MountainTown.RBUTownEdge}, // RBUTownEdge
        // Weston
        {"(287.45, 198.50, -49.99)", MountainTown.BBWEntry}, // BBWEntry
        {"(320.60, 198.86, -65.65)", MountainTown.RBWEntry}, // RBWEntry
        {"(407.72, 196.02, -72.15)", MountainTown.BBWBakeryAcross}, // BBWBakeryAcross
        {"(436.57, 195.03, -84.92)", MountainTown.RBWBakeryLeft}, // RBWBakeryLeft
        {"(435.85, 195.05, -84.81)", MountainTown.BBWBakeryLeft}, // BBWBakeryLeft
        {"(457.49, 195.15, -50.03)", MountainTown.RBWBarTunnel}, // RBWBarTunnel
        {"(446.47, 195.79, -43.11)", MountainTown.VMWBarTunnel}, // VMWBarTunnel
        {"(398.14, 195.00, -37.17)", MountainTown.BBWDepot}, // BBWDepot
        {"(279.93, 156.83, 69.52)", MountainTown.BBWBarLower}, // BBWBarLower
        {"(230.71, 156.11, 68.35)", MountainTown.RBWCafe}, // RBWCafe
        // Easton
        {"(-208.38, 146.34, 42.44)", MountainTown.BBEMart}, // BBEMart
        {"(-143.60, 146.19, 38.97)", MountainTown.RBEFuel}, // RBEFuel
        {"(-144.36, 146.22, 39.11)", MountainTown.BBEFuel}, // BBEFuel
        {"(-140.00, 146.32, 37.85)", MountainTown.VMEFuel}, // VMEFuel
        {"(-202.84, 146.27, 26.13)", MountainTown.BBEPizzaRight}, // BBEPizzaRight
        {"(-139.48, 146.15, 6.84)", MountainTown.RBEPizzaLeft} // RBEPizzaLeft
    };

    public static readonly Dictionary<string, PositionData> SnowyPeaksPositions = new()
    {
        // Winton
        {"(-586.90, 0.26, 616.57)", SnowyPeaks.BBWFlowers},
        {"(-596.36, 0.04, 615.28)", SnowyPeaks.RBWEats},
        {"(-446.21, 33.01, -42.56)", SnowyPeaks.VMWFuel},
        {"(-630.71, -0.10, 595.56)", SnowyPeaks.BBWFuel},
        {"(-629.87, -0.08, 595.92)", SnowyPeaks.RBWFuel},
        // Munton
        {"(0.00, -0.74, 0.00)", SnowyPeaks.VMMAuto},
        {"(-195.85, -33.65, 605.07)", SnowyPeaks.RBMPawn},
        {"(-52.17, 53.53, 645.23)", SnowyPeaks.RBMMart},
        {"(-51.45, 53.56, 644.61)", SnowyPeaks.BBMMart},
        {"(152.72, 86.37, -8.95)", SnowyPeaks.VMMBar},
        // Lopton
        {"(0.00, 0.00, 0.00)", SnowyPeaks.RBLBar},
        {"(113.55, 32.65, -692.20)", SnowyPeaks.VMLDepot},
        {"(-79.79, -0.20, -56.63)", SnowyPeaks.RBLDepot},
        {"(-79.34, -0.20, -55.91)", SnowyPeaks.BBLDepot}
    };

    public static readonly Dictionary<string, PositionData> FishingTownPositions = new()
    {
        // Clifton
        {"(0.34, 0.01, 1.03)", FishingTown.RBCBar},
        {"(0.00, 0.00, 0.00)", FishingTown.BBCBar},
        {"(278.26, 92.31, 313.60)", FishingTown.VMCMart},
        {"(54.99, 0.25, 27.34)", FishingTown.BBCMart},
        {"(54.07, 0.26, 27.37)", FishingTown.RBCMart},
        // Damton
        {"(183.00, -17.49, -653.05)", FishingTown.RBDBar},
        {"(183.82, -17.49, -653.01)", FishingTown.BBDBar},
        {"(414.58, 74.61, -364.05)", FishingTown.VMDBar},
        {"(-22.45, -55.79, -763.66)", FishingTown.RBDDepot},
        {"(-21.64, -55.74, -763.70)", FishingTown.BBDDepot},
        {"(207.45, 36.32, -474.97)", FishingTown.VMDDepot},
        // Smalton
        {"(-431.89, -66.74, -786.37)", FishingTown.BBSPizza},
        {"(-432.70, -66.74, -786.38)", FishingTown.RBSPizza},
        {"(-410.32, 76.99, -430.47)", FishingTown.VMSEatsLeft},
        {"(-412.82, 76.99, -430.47)", FishingTown.VMSEatsRight},
        {"(-641.40, -15.20, -719.75)", FishingTown.RBSEats},
        {"(-642.22, -15.19, -719.72)", FishingTown.BBSEats}
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

public class PositionData
{
    public string Name { get; set; }

    public string Town { get; set; }

    public string Type { get; set; }

    // public Vector3 Position { get; set; }

    public int LocationId { get; set; }

    public PositionData(string name, string town, int locId)
    {
        Name = name;
        Town = town;
        LocationId = locId;
        Type = "None";
    }

    public PositionData(string name, string town, int locId, string type)
    {
        Name = name;
        Town = town;
        LocationId = locId;
        Type = type;
    }
}
