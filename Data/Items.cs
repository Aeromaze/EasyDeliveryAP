using System.Collections.Generic;

namespace EasyDeliveryAP;

public class ItemData
{
    public string Name { get; set; }

    public int Id { get; set; }

    public int Received { get; set; }

    public bool Enabled { get; set; }

    public bool Toggle { get; set; }

    public int Given { get; set; }

    public ItemData() { }

    public ItemData(string name)
    {
        Name = name;
        Id = -1;
        Received = 0;
        Enabled = false;
        Toggle = false;
        Given = 0;
    }

    public ItemData(string name, int id)
    {
        Name = name;
        Id = id;
        Received = 0;
        Enabled = false;
        Toggle = false;
        Given = 0;
    }
}


public class Items
{
    // Upgrades
    public static ItemData GPS = new("Map");
    public static ItemData Tires = new("Snow Tires");
    public static ItemData Bumper = new("Bumper");
    public static ItemData Chains = new("Ice Chains");

    // Inventory Items
    public static ItemData EnergyDrink = new("Energy Drink", 0);
    public static ItemData EmptyCan = new("Empty Can", 1);
    public static ItemData Lantern = new("Lantern", 2);
    public static ItemData Lighter = new("Lighter", 3);
    public static ItemData Firewood = new("Firewood", 4);
    public static ItemData Shovel = new("Shovel", 5);
    public static ItemData BirdSeed = new("Bird Seed", 6);
    public static ItemData CookingPot = new("Cooking Pot", 7);
    public static ItemData Coffee = new("Coffee", 8);
    public static ItemData CoffeePowder = new("Coffee Powder", 9);
    public static ItemData Tea = new("Tea", 10);
    public static ItemData TeaBags = new("Tea Bags", 11);
    public static ItemData Fish = new("Fish", 12);
    public static ItemData FishingRod = new("Fishing Rod", 13);
    public static ItemData FishSoup = new("Fish Soup", 14);
    public static ItemData DuctTape = new("Duct Tape", 15);
    public static ItemData RestoreDisc = new("Restore Disc", 16);
    public static ItemData HandheldRadio = new("Handheld Radio", 17);

    // Tunnels
    public static ItemData TunnelSP = new("Snowy Peaks Tunnel");
    public static ItemData TunnelFT = new("Fishing Town Tunnel");
    public static ItemData TunnelFactory = new("Factory Tunnel");

    // Towns
    public static ItemData Upton = new("Upton");
    public static ItemData Weston = new("Weston");
    public static ItemData Easton = new("Easton");
    public static ItemData Winton = new("Winton");
    public static ItemData Munton = new("Munton");
    public static ItemData Lopton = new("Lopton");
    public static ItemData Clifton = new("Clifton");
    public static ItemData Damton = new("Damton");
    public static ItemData Smalton = new("Smalton");

    // Misc
    public static ItemData Money = new("Money");
    public static ItemData RadioTower = new("Radio Tower");

    public static Dictionary<int, ItemData> APIdToItem = new()
    {
        {1, GPS},
        {2, Tires},
        {3, Bumper},
        {4, Chains},
        {10, Money},
        {11, TunnelSP},
        {12, TunnelFT},
        {13, TunnelFactory},
        {20, RadioTower},
        {30, Upton},
        {31, Weston},
        {32, Easton},
        {33, Winton},
        {34, Munton},
        {35, Lopton},
        {36, Clifton},
        {37, Damton},
        {38, Smalton},
        {100, EnergyDrink},
        {101, EmptyCan},
        {102, Lantern},
        {103, Lighter},
        {104, Firewood},
        {105, Shovel},
        {106, BirdSeed},
        {107, CookingPot},
        {108, Coffee},
        {109, CoffeePowder},
        {110, Tea},
        {111, TeaBags},
        {112, Fish},
        {113, FishingRod},
        {114, FishSoup},
        {115, DuctTape},
        {116, RestoreDisc},
        {117, HandheldRadio}
    };

    public static Dictionary<string, ItemData> TunnelToItem = new()
    {
        {"Snowy Peaks", TunnelSP},
        {"Fishing Town", TunnelFT},
        // {"Mountain Town", },
    };

    public static Dictionary<string, ItemData> TownToItem = new()
    {
        {"Upton", Upton},
        {"Weston", Weston},
        {"Easton", Easton},
        {"Winton", Winton},
        {"Munton", Munton},
        {"Lopton", Lopton},
        {"Clifton", Clifton},
        {"Damton", Damton},
        {"Smalton", Smalton}
    };
}