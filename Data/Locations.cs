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

        // Signs
        public static PositionData SignToSP = new("Welcome to Mountain Town", "Upton", 200, "Sign");
        public static PositionData SignUptonExit = new("To Weston, To Easton", "Upton", 201, "Sign");
        public static PositionData SignUptonRadio = new("Upton Radio Tower", "Upton", 202, "Sign");
        public static PositionData SignWestonEntryUpton = new("Weston Upper To Upton", "Weston", 203, "Sign");
        public static PositionData SignWestonEntryEaston = new("Weston Upper To Easton", "Weston", 204, "Sign");
        public static PositionData SignFactory = new("Factory", "Weston", 205, "Sign");
        public static PositionData SignWestonExitUpton = new("Weston To Upton", "Weston", 206, "Sign");
        public static PositionData SignWestonExitEaston = new("Weston To Easton", "Weston", 207, "Sign");
        public static PositionData SignToEaston = new("Weston Bar", "Weston", 208, "Sign");
        public static PositionData SignEastonEntry = new("Weston Bridge", "Weston", 209, "Sign");
        public static PositionData SignEastonOtherEntry = new("Easton To Weston, To Upton", "Easton", 210, "Sign");
        public static PositionData SignToFTLower = new("Easton Tunnel to Fishing Town", "Easton", 211, "Sign");
        public static PositionData SignEastonExit = new("Easton", "Easton", 212, "Sign");
        public static PositionData SignToFTUpper = new("Easton Edge Tunnel to Fishing Town", "Easton", 213, "Sign");
        public static PositionData SignEastonRadio = new("Easton Radio Tower", "Easton", 214, "Sign");
        public static PositionData SignToFT = new("Easton Tunnel", "Easton", 215, "Sign");
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
        public static NodeData Bar1 = new("Bar 1", "Munton", 15);
        public static NodeData EasyDepot = new("Easy Depot", "Lopton", 16);
        public static NodeData EZAuto = new("EZ Auto", "Munton", 17);
        public static NodeData EZCafe = new("EZ Cafe", "Lopton", 18);
        public static NodeData Bar2 = new("Bar 2", "Lopton", 19);
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

        // Signs
        public static PositionData SignToMT = new("Welcome to Snowy Peaks", "Winton", 216, "Sign");
        public static PositionData SignWintonExit = new("To Munton", "Winton", 217, "Sign");
        public static PositionData SignWintonEntry = new("Winton", "Winton", 218, "Sign");
        public static PositionData SignMuntonEntry = new("Munton", "Munton", 219, "Sign");
        public static PositionData SignMuntonBridgeWinton = new("To Winton", "Munton", 220, "Sign");
        public static PositionData SignMuntonBridgeLopton = new("To Lopton", "Munton", 221, "Sign");
        public static PositionData SignUpperMunton = new("Upper Munton", "Munton", 222, "Sign");
        public static PositionData SignLoptonEntry = new("Lopton", "Lopton", 223, "Sign");
        public static PositionData SignLoptonBridge = new("To Munton, To Winton", "Lopton", 224, "Sign");
        public static PositionData SignLoptonRadio = new("Snowy Peaks Radio Tower", "Lopton", 225, "Sign");
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
        public static NodeData Node14 = new("Node (14)", "Damton", 21);
        public static NodeData Nodedock21 = new("Node (dock2) (1)", "Smalton", 22);
        public static NodeData Node102 = new("Node (102)", "Clifton", 23);
        public static NodeData Node98 = new("Node (98)", "Clifton", 24);
        public static NodeData EasyFlowers = new("Easy Flowers", "Smalton", 25);
        public static NodeData EasyEats = new("Easy Eats", "Smalton", 26);
        public static NodeData PawnShop = new("Pawn Shop", "Clifton", 27);
        public static NodeData EZMart = new("EZ Mart", "Clifton", 28);
        public static NodeData Bar2 = new("Bar", "Damton", 29); // might be swapped
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

        // Signs
        public static PositionData SignWelcome = new("Welcome to Fishing Town", "Clifton", 226, "Sign");
        public static PositionData SignToMT = new("Tunnel to Mountain Town", "Clifton", 227, "Sign");
        public static PositionData SignCliftonEdgeTSTD = new("To Smalton, To Damton", "Clifton", 228, "Sign");
        public static PositionData SignCliftonC = new("Clifton", "Clifton", 229, "Sign");
        public static PositionData SignCliftonToMT = new("Tunnel to Mountain Town", "Clifton", 232, "Sign"); // Clifton proper
        public static PositionData SignCliftonTSTD = new("To Smalton, To Damton", "Clifton", 233, "Sign"); // Clifton proper
        public static PositionData SignCliftonTD = new("Fork To Damton", "Clifton", 230, "Sign"); // Fork
        public static PositionData SignCliftonTS = new("Fork To Smalton", "Clifton", 231, "Sign"); // Fork
        public static PositionData SignLCliftonRLC = new("SignLCliftonRLC", "Clifton", 234, "Sign");
        public static PositionData SignLCliftonRTS = new("SignLCliftonRTS", "Clifton", 235, "Sign");
        public static PositionData SignLCliftonRTD = new("SignLCliftonRTD", "Clifton", 236, "Sign");
        public static PositionData SignLCliftonLTD = new("SignLCliftonLTD", "Clifton", 237, "Sign");
        public static PositionData SignLCliftonLTS = new("SignLCliftonLTS", "Clifton", 238, "Sign");
        public static PositionData SignLCliftonLLC = new("SignLCliftonLLC", "Clifton", 239, "Sign");
        public static PositionData SignSmaltonBS = new("Smalton Bridge", "Smalton", 240, "Sign");
        public static PositionData SignSmaltonTD = new("Smalton Dam", "Smalton", 241, "Sign");
        public static PositionData SignDamtonDTS = new("SignDamtonDTS", "Damton", 242, "Sign");
        public static PositionData SignDamtonDTCTS = new("SignDamtonDTCTS", "Damton", 243, "Sign");
        public static PositionData SignDamtonDLD = new("SignDamtonDLD", "Damton", 244, "Sign");
        public static PositionData SignDamtonUD = new("Upper Damton", "Damton", 245, "Sign");
        public static PositionData SignDamtonLD = new("Lower Damton", "Damton", 246, "Sign");
        public static PositionData SignDamtonD = new("SignDamtonD", "Damton", 247, "Sign");
        public static PositionData SignDamtonTCTS = new("SignDamtonTCTS", "Damton", 248, "Sign");
        public static PositionData SignDamtonTS = new("SignDamtonTS", "Damton", 249, "Sign");
        public static PositionData SignDamtonTD = new("SignDamtonTD", "Damton", 250, "Sign");
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
        {"(-139.48, 146.15, 6.84)", MountainTown.RBEPizzaLeft}, // RBEPizzaLeft

        // Signs
        {"(-445.35, 329.58, -305.98)", MountainTown.SignToSP}, // Enter: Tunnel to Snowy Peaks Exit: Welcome to Mountain Town
        {"(-192.70, 331.69, -251.75)", MountainTown.SignUptonExit}, // Enter: Upton Exit: To Weston To Easton
        {"(385.50, 352.25, -244.08)", MountainTown.SignUptonRadio}, // Enter:  Exit: Upton Radio Tower
        {"(262.00, 199.57, -67.52)", MountainTown.SignWestonEntryUpton}, // Enter: Weston Exit: To Upton
        {"(269.42, 200.02, -45.38)", MountainTown.SignWestonEntryEaston}, // Enter: To Easton Exit: 
        {"(486.47, 195.85, -61.77)", MountainTown.SignFactory}, // Enter: Factory Exit: 
        {"(374.95, 164.78, 35.62)", MountainTown.SignWestonExitUpton}, // Enter: To Upton Exit: 
        {"(373.63, 163.87, 47.30)", MountainTown.SignWestonExitEaston}, // Enter: To Easton Exit: 
        {"(274.21, 159.25, 84.87)", MountainTown.SignToEaston}, // Enter: To Easton Exit: 
        {"(126.83, 152.05, 41.89)", MountainTown.SignEastonEntry}, // Enter: Weston Exit: To Easton
        {"(-33.76, 142.62, 62.99)", MountainTown.SignEastonOtherEntry}, // Enter: To Weston To Upton Exit: 
        {"(-45.99, 142.62, 70.29)", MountainTown.SignToFTLower}, // Enter: Tunnel to Fishing Town Exit: 
        {"(-58.33, 142.32, 54.48)", MountainTown.SignEastonExit}, // Enter: Easton Exit: 
        {"(-382.42, 146.43, 39.41)", MountainTown.SignToFTUpper}, // Enter: Tunnel to Fishing Town Exit: Easton
        {"(-436.95, 140.38, 35.60)", MountainTown.SignEastonRadio}, // Enter: Easton Radio Tower Exit: Easton Radio Tower
        {"(-495.71, 126.33, 140.12)", MountainTown.SignToFT} // Enter: Tunnel to Fishing Town Exit: Welcome to Mountain Town
    };

    public static readonly Dictionary<string, PositionData> SnowyPeaksPositions = new()
    {
        // Winton
        {"(-279.34, 277.75, 233.77)", SnowyPeaks.BBWFlowers}, // (-279.34, 277.75, 233.77)
        {"(-288.79, 277.53, 232.49)", SnowyPeaks.RBWEats}, // (-288.79, 277.53, 232.49)
        {"(-325.92, 277.63, 209.57)", SnowyPeaks.VMWFuel}, // (-325.92, 277.63, 209.57)
        {"(-323.14, 277.39, 212.76)", SnowyPeaks.BBWFuel}, // (-323.14, 277.39, 212.76)
        {"(-322.31, 277.42, 213.12)", SnowyPeaks.RBWFuel}, // (-322.31, 277.42, 213.12)
        // Munton
        {"(120.29, 243.89, 252.13)", SnowyPeaks.VMMAuto}, // (120.29, 243.89, 252.13)
        {"(111.71, 243.84, 222.27)", SnowyPeaks.RBMPawn}, // (111.71, 243.84, 222.27)
        {"(256.12, 331.05, 261.82)", SnowyPeaks.BBMMart}, // (256.12, 331.05, 261.82)
        {"(255.40, 331.02, 262.43)", SnowyPeaks.RBMMart}, // (255.40, 331.02, 262.43)
        {"(273.01, 330.99, 243.18)", SnowyPeaks.VMMBar}, // (273.01, 330.99, 243.18)
        // Lopton
        {"(307.56, 277.49, -382.80)", SnowyPeaks.RBLBar}, // (307.56, 277.49, -382.80)
        {"(233.84, 277.28, -440.07)", SnowyPeaks.VMLDepot}, // (233.84, 277.28, -440.07)
        {"(227.77, 277.29, -439.43)", SnowyPeaks.RBLDepot}, // (227.77, 277.29, -439.43)
        {"(228.23, 277.29, -438.71)", SnowyPeaks.BBLDepot}, // (228.23, 277.29, -438.71)


        // Signs
        {"(-156.92, 245.51, 272.73)", SnowyPeaks.SignToMT}, // Enter: Tunnel to Mountain Town Exit: Welcome to Snowy Peaks
        {"(-139.71, 245.77, 249.59)", SnowyPeaks.SignWintonExit}, // Enter: To Munton Exit: 
        {"(-153.33, 246.28, 247.21)", SnowyPeaks.SignWintonEntry}, // Enter: Winton Exit: 
        {"(81.62, 246.57, 221.57)", SnowyPeaks.SignMuntonEntry}, // Enter: Munton Exit: To Winton
        {"(335.00, 245.57, 124.35)", SnowyPeaks.SignMuntonBridgeWinton}, // Enter: To Winton Exit: 
        {"(335.29, 246.03, 105.27)", SnowyPeaks.SignMuntonBridgeLopton}, // Enter: Munton Exit: To Lopton
        {"(360.68, 246.47, 125.37)", SnowyPeaks.SignUpperMunton}, // Enter: Upper Munton Exit: 
        {"(353.74, 248.13, -214.54)", SnowyPeaks.SignLoptonEntry}, // Enter: Lopton Exit: To Munton To Winton
        {"(-331.79, 246.64, -266.31)", SnowyPeaks.SignLoptonBridge}, // Enter: To Munton To Winton Exit: 
        {"(-320.11, 247.85, -247.79)", SnowyPeaks.SignLoptonRadio} // Enter: Snowy Peaks Radio Tower Exit: 

    };

    public static readonly Dictionary<string, PositionData> FishingTownPositions = new()
    {
        // Clifton
        {"(234.21, 92.07, 289.82)", FishingTown.RBCBar}, // (234.21, 92.07, 289.82)
        {"(233.87, 92.06, 288.78)", FishingTown.BBCBar}, // (233.87, 92.06, 288.78)
        {"(278.26, 92.31, 313.60)", FishingTown.VMCMart}, // No problem
        {"(288.85, 92.32, 316.12)", FishingTown.BBCMart}, // (288.85, 92.32, 316.12)
        {"(287.93, 92.33, 316.15)", FishingTown.RBCMart}, // (287.93, 92.33, 316.15)
        // Damton
        {"(416.86, 74.57, -364.26)", FishingTown.RBDBar}, // (416.86, 74.57, -364.26)
        {"(417.68, 74.57, -364.23)", FishingTown.BBDBar}, // (417.68, 74.57, -364.23)
        {"(414.58, 74.61, -364.05)", FishingTown.VMDBar}, // No problem
        {"(211.42, 36.28, -474.88)", FishingTown.RBDDepot}, // (211.42, 36.28, -474.88)
        {"(212.23, 36.32, -474.92)", FishingTown.BBDDepot}, // (212.23, 36.32, -474.92)
        {"(207.45, 36.32, -474.97)", FishingTown.VMDDepot}, // No problem
        // Smalton
        {"(-198.02, 25.32, -497.58)", FishingTown.BBSPizza}, // (-198.02, 25.32, -497.58)
        {"(-198.83, 25.32, -497.60)", FishingTown.RBSPizza}, // (-198.83, 25.32, -497.60)
        {"(-410.32, 76.99, -430.47)", FishingTown.VMSEatsLeft}, // No problem
        {"(-412.82, 76.99, -430.47)", FishingTown.VMSEatsRight}, // No problem
        {"(-407.54, 76.87, -430.97)", FishingTown.RBSEats}, // (-407.54, 76.87, -430.97)
        {"(-408.35, 76.87, -430.94)", FishingTown.BBSEats}, // (-408.35, 76.87, -430.94)

        // Signs
        {"(-426.09, 125.57, 201.99)", FishingTown.SignWelcome}, // Enter: Tunnel to Mountain Town Exit: Welcome to Fishing Town
        {"(-6.70, 82.43, 351.96)", FishingTown.SignToMT}, // Enter: Tunnel to Mountain Town Exit: 
        {"(-2.87, 80.72, 337.14)", FishingTown.SignCliftonEdgeTSTD}, // Enter: To Smalton To Damton Exit: 
        {"(19.71, 81.17, 342.55)", FishingTown.SignCliftonC}, // Enter: Clifton Exit: 
        {"(89.07, 92.61, 335.14)", FishingTown.SignCliftonToMT}, // Enter: Tunnel to Mountain Town Exit: 
        {"(87.40, 93.06, 313.40)", FishingTown.SignCliftonTSTD}, // Enter: To Smalton To Damton Exit: 
        {"(-10.98, 68.86, 282.54)", FishingTown.SignCliftonTD}, // Enter: To Damton Exit: 
        {"(-25.19, 67.54, 285.79)", FishingTown.SignCliftonTS}, // Enter: To Smalton Exit: 
        {"(-14.93, 33.02, 195.51)", FishingTown.SignLCliftonRLC}, // Enter: Clifton Exit: Leaving Clifton
        {"(-14.25, 32.98, 176.99)", FishingTown.SignLCliftonRTS}, // Enter: To Smalton Exit: 
        {"(2.11, 33.39, 178.20)", FishingTown.SignLCliftonRTD}, // Enter: To Damton Exit: 
        {"(238.04, 35.06, 102.83)", FishingTown.SignLCliftonLTD}, // Enter: To Damton Exit: 
        {"(231.32, 34.65, 117.80)", FishingTown.SignLCliftonLTS}, // Enter: To Smalton Exit: 
        {"(249.88, 34.85, 123.69)", FishingTown.SignLCliftonLLC}, // Enter: Clifton Exit: Leaving Clifton
        {"(-398.06, 45.30, -128.26)", FishingTown.SignSmaltonBS}, // Enter: Smalton Exit: To Clifton To Damton
        {"(-136.70, 26.10, -484.49)", FishingTown.SignSmaltonTD}, // Enter: Smalton Exit: To Damton
        {"(93.58, 36.67, -444.48)", FishingTown.SignDamtonDTS}, // Enter: To Smalton Exit: 
        {"(100.39, 36.67, -432.55)", FishingTown.SignDamtonDTCTS}, // Enter: To Clifton To Smalton Exit: 
        {"(115.01, 37.37, -439.39)", FishingTown.SignDamtonDLD}, // Enter: Damton Exit: Leaving Damton
        {"(254.17, 39.26, -376.64)", FishingTown.SignDamtonUD}, // Enter: Upper Damton Exit: 
        {"(419.08, 76.57, -193.78)", FishingTown.SignDamtonLD}, // Enter: Lower Damton Exit: 
        {"(162.18, 31.89, -353.16)", FishingTown.SignDamtonD}, // Enter: Damton Exit: Leaving Damton
        {"(155.50, 31.61, -335.05)", FishingTown.SignDamtonTCTS}, // Enter: To Clifton To Smalton Exit: 
        {"(141.19, 32.72, -339.13)", FishingTown.SignDamtonTS}, // Enter: To Smalton Exit: 
        {"(351.96, 43.82, -138.48)", FishingTown.SignDamtonTD} // Enter: To Damton Exit: To Clifton To Smalton
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

    public int LocationId { get; set; } // Could be made an array if needed for multiple locations

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
