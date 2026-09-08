using EasyDeliveryAP.Archipelago;
using EasyDeliveryAP.Utils;
using EasyDeliveryAPI;
using UnityEngine;

namespace EasyDeliveryAP;

public class TrackerApp : MonoBehaviour
{
    private static readonly ArchipelagoClient archipelago = EasyDeliveryAP.ArchipelagoClient;

    private UIUtil Util = new();
    private Rect P;
    private Vector2 offset = new(5f, 5f);
    private float textSize = 8;
    private float spriteSize = 16;

    // Button variables
    private bool logicButton = false;
    private bool townDeliveries = true;
    private bool cityDeliveries = false;
    private bool payloadDeliveries = false;
    private bool restButton = false;
    
    private static int total_deliveries = 81;
    int deliveries;

    public void FrameUpdate(DesktopDotExe.WindowView window)
    {
        Util.M = window.M;
        Util.R = window.R;
        Util.nav = window.M.nav;

        P = new Rect(window.position * 8f, window.size * 8f);
        P.position += new Vector2(8f, 8f);

        if (ArchipelagoClient.Authenticated)
        {
            DrawTracker(window);
        }
        else
        {
            DrawText("Not connected to Archipelago", 1, 1);
        }
    }

    private void DrawTracker(DesktopDotExe.WindowView window)
    {
        var checkedLocations = archipelago.session.Locations.AllLocationsChecked;

        Texture altSpriteSheet = window.M.altSpriteSheet;
        // Texture spriteSheet = window.M.spriteSheet;
        // Texture spriteSheetBackup = window.M.spriteSheetBackup;

        if (Util.Button((logicButton ? "Overview" : "Extra info"), P.x + 8, P.y + 192))
        {
            logicButton = !logicButton;
        }
        if (logicButton)
        {
            // Select screen/menu
            if (Util.Button((townDeliveries ? ">Town Deliveries<" : " Town Deliveries"), P.x, P.y, true))
            {
                townDeliveries = true;
                cityDeliveries = false;
                payloadDeliveries = false;
                restButton = false;
            }
            if (Util.Button((cityDeliveries ? ">Other Deliveries<" : " Other Deliveries"), P.x + 128, P.y, true))
            {
                townDeliveries = false;
                cityDeliveries = true;
                payloadDeliveries = false;
                restButton = false;
            }
            if (Util.Button((payloadDeliveries ? ">Payload Deliveries<" : " Payload Deliveries"), P.x, P.y + 8, true))
            {
                townDeliveries = false;
                cityDeliveries = false;
                payloadDeliveries = true;
                restButton = false;
            }
            if (Util.Button((restButton ? ">Snowcats<" : " Snowcats"), P.x + 128, P.y + 8, true))
            {
                townDeliveries = false;
                cityDeliveries = false;
                payloadDeliveries = false;
                restButton = true;
            }

            // Draw the different logic trackers
            if (townDeliveries)
            {
                DeliveryGrid(0, 3);
            }
            else if (cityDeliveries)
            {
                if (APData.intercity_deliveries == "2" || APData.intercity_deliveries == "3")
                {
                    Vector2 grid = new (0, 0);
                    int del;
                    bool sp = APLogic.CanReachSnowyPeaks();
                    bool ft = APLogic.CanReachFishingTown() && APLogic.FTGateIsOpen();

                    DrawText("Mt Sp Ft", 3, 3);
                    DrawText("Mt\n\nSp\n\nFt", 0, 5);
                    foreach (var from in Locations.City)
                    {
                        foreach (var to in Locations.City)
                        {
                            if (((from.Key == "Snowy Peaks" || to.Key == "Snowy Peaks") && (!sp || !(Items.Winton.Received > 0 || Items.Munton.Received > 0 || Items.Lopton.Received > 0))) || 
                                ((from.Key == "Fishing Town" || to.Key == "Fishing Town") && (!ft || !(Items.Clifton.Received > 0 || Items.Damton.Received > 0 || Items.Smalton.Received > 0))))
                            {
                                continue;
                            }
                            del = int.Parse($"{from.Value}{to.Value}");
                            if (APData.perfect_deliveries == "0" || APData.perfect_deliveries == "1")
                            {
                                DrawText($"{(checkedLocations.Contains(del) ? "x" : "o")}", 3 + grid.x, 5 + grid.y);
                            }
                            if (APData.perfect_deliveries == "1" || APData.perfect_deliveries == "2")
                            {
                                DrawText($"{(checkedLocations.Contains(del + 10000) ? "x" : "o")}", 4 + grid.x, 5 + grid.y);
                            }
                            grid.y += 2;
                        }
                        grid.x += 3;
                        grid.y = 0;
                    }
                }
            }
            else if (payloadDeliveries)
            {
                if (APData.payload_checks == "1")
                {
                    bool sp = APLogic.CanReachSnowyPeaks();
                    bool ft = APLogic.CanReachFishingTown();

                    DrawText($"Box Bunch:", 0, 6);
                    DrawText($"Box Stack:", 0, 7);
                    DrawText($"Crate of Drinks:", 0, 9);
                    DrawText($"Sack:", 0, 18);
                    DrawText($"Sack Stack:", 0, 19);
                    DrawText($"Big Box:", 0, 4);
                    DrawText($"Big Box Stack:", 0, 5);
                    DrawText($"Crate:", 0, 8);
                    DrawText($"Crate Stack:", 0, 10);
                    DrawText($"Lots of Crates of Drinks:", 0, 11);
                    DrawText($"Drink:", 0, 20);
                    DrawText($"Pizza Stack:", 0, 12);
                    DrawText($"Pizza Stack Mega:", 0, 13);
                    DrawText($"Plant Pot:", 0, 14);
                    DrawText($"Plant Pot Bunch:", 0, 15);
                    DrawText($"Plant Pot Stack:", 0, 16);
                    DrawText($"Plant Pot Wide:", 0, 17);

                    // Any town
                    DrawText($"{(checkedLocations.Contains(3) ? "x" : "o")}", 26, 6);
                    DrawText($"{(checkedLocations.Contains(4) ? "x" : "o")}", 26, 7);
                    DrawText($"{(checkedLocations.Contains(6) ? "x" : "o")}", 26, 9);
                    DrawText($"{(checkedLocations.Contains(15) ? "x" : "o")}", 26, 18);
                    DrawText($"{(checkedLocations.Contains(16) ? "x" : "o")}", 26, 19);
                    if (Items.Weston.Received > 0 || Items.Easton.Received > 0 || (sp && (Items.Munton.Received > 0 || Items.Lopton.Received > 0)) || (ft && (Items.Clifton.Received > 0 || Items.Damton.Received > 0)))
                    {
                        DrawText($"{(checkedLocations.Contains(1) ? "x" : "o")}", 26, 4);
                        DrawText($"{(checkedLocations.Contains(2) ? "x" : "o")}", 26, 5);
                        DrawText($"{(checkedLocations.Contains(5) ? "x" : "o")}", 26, 8);
                        DrawText($"{(checkedLocations.Contains(7) ? "x" : "o")}", 26, 10);
                    }
                    if (Items.Weston.Received > 0 || (sp && (Items.Munton.Received > 0 || Items.Lopton.Received > 0)) || (ft && (Items.Clifton.Received > 0 || Items.Damton.Received > 0)))
                    {
                        DrawText($"{(checkedLocations.Contains(8) ? "x" : "o")}", 26, 11);
                        DrawText($"{(checkedLocations.Contains(17) ? "x" : "o")}", 26, 20);
                    }
                    if (Items.Upton.Received > 0 || Items.Easton.Received > 0 || (sp && (Items.Winton.Received > 0 || Items.Lopton.Received > 0) || (ft && Items.Smalton.Received > 0)))
                    {
                        DrawText($"{(checkedLocations.Contains(9) ? "x" : "o")}", 26, 12);
                        DrawText($"{(checkedLocations.Contains(10) ? "x" : "o")}", 26, 13);
                    }
                    if (Items.Upton.Received > 0 || Items.Easton.Received > 0 || (sp && (Items.Winton.Received > 0 || Items.Munton.Received > 0)) || (ft && (Items.Clifton.Received > 0 || Items.Smalton.Received > 0)))
                    {
                        DrawText($"{(checkedLocations.Contains(11) ? "x" : "o")}", 26, 14);
                        DrawText($"{(checkedLocations.Contains(12) ? "x" : "o")}", 26, 15);
                        DrawText($"{(checkedLocations.Contains(13) ? "x" : "o")}", 26, 16);
                        DrawText($"{(checkedLocations.Contains(14) ? "x" : "o")}", 26, 17);
                    }
                }
            }
            else if (restButton)
            {
                bool sp = APLogic.CanReachSnowyPeaks();
                bool ft = APLogic.CanReachFishingTown();
                bool gate = APLogic.FTGateIsOpen() && ft;
                if (APData.snowcats == "1" || APData.snowcats == "2")
                {
                    DrawText($"Theo:", 0, 4);
                    DrawText($"Cici:", 0, 5);
                    DrawText($"Tooey:", 0, 9);
                    DrawText($"Gus:", 0, 14);
                    DrawText($"Fives:", 0, 11);
                    DrawText($"Sixo:", 0, 12);
                    DrawText($"Foreman:", 0, 8);
                    DrawText($"Seb:", 0, 15);
                    DrawText($"Reed:", 0, 10);
                    DrawText($"Fortino:", 0, 6);
                    DrawText($"Ellie:", 0, 7);
                    DrawText($"Fit:", 0, 13);

                    DrawText($"{(checkedLocations.Contains(40) ? "x" : "o")}", 9, 4);
                    DrawText($"{(checkedLocations.Contains(41) ? "x" : "o")}", 9, 5);
                    if (APLogic.HasSnowTires())
                    {
                        DrawText($"{(checkedLocations.Contains(45) ? "x" : "o")}", 9, 9);
                        DrawText($"{(checkedLocations.Contains(51) ? "x" : "o")}", 9, 14);
                    }
                    if (sp)
                    {
                        DrawText($"{(checkedLocations.Contains(47) ? "x" : "o")}", 9, 11);
                        DrawText($"{(checkedLocations.Contains(48) ? "x" : "o")}", 9, 12);
                        DrawText($"{(checkedLocations.Contains(44) ? "x" : "o")}", 9, 8);
                        DrawText($"{(checkedLocations.Contains(52) ? "x" : "o")}", 9, 15);
                    }
                    if (ft)
                    {
                        DrawText($"{(checkedLocations.Contains(46) ? "x" : "o")}", 9, 10);
                    }
                    if (gate)
                    {
                        DrawText($"{(checkedLocations.Contains(42) ? "x" : "o")}", 9, 6);
                        DrawText($"{(checkedLocations.Contains(43) ? "x" : "o")}", 9, 7);
                        if (Items.Chains.Received > 0)
                        {
                            DrawText($"{(checkedLocations.Contains(50) ? "x" : "o")}", 9, 13);
                        }
                    }
                }
                if (APData.snowcats == "1")
                {
                    DrawText($"Ada:", 0, 16);

                    DrawText($"{(checkedLocations.Contains(49) ? "x" : "o")}", 9, 16);
                }
            }
        }
        else
        {
            DrawText("Items Received:", 9, 0);

            // Car upgrades
            DrawText($"U\np\ng\nr\na\nd\ne\ns\n{(APData.car_upgrades == "0" ? "(Buy)" : "")}", 0, 1);
            if(Items.Tires.Received > 0)
            DrawSprite(altSpriteSheet, APGUI.snowTires, 0.75f, 3/4f);
            if(Items.Bumper.Received > 0)
            DrawSprite(altSpriteSheet, APGUI.truckBumper, 0.75f, 2f);
            if(Items.Chains.Received > 0)
            DrawSprite(altSpriteSheet, APGUI.iceChains, 0.75f, 13/4f);

            // Radio Towers
            if(APData.radio_towers == "1" || APData.radio_towers == "3")
            {
                DrawSprite(altSpriteSheet, APGUI.radio, 2.5f, 3/4f);
                DrawText($"x", 5.5f, 3.5f);
                DrawText($"{Items.RadioTower.Received}", 5.5f, 5f);
            }

            // Towns and Tunnels
            if (APData.lock_towns == "1" || APData.blocked_tunnels == "1" || APData.blocked_tunnels == "2")
            {
                DrawText("Mountain\nTown:", 8f, 1.5f);
                DrawText("Snowy\nPeaks:", 17f, 1.5f);
                DrawText("Fishing\nTown:", 24f, 1.5f);
                if (APData.lock_towns == "1")
                {
                    DrawText($"{(Items.Upton.Received > 0 ? "Upton" : "")}\n{(Items.Weston.Received > 0 ? "Weston" : "")}\n{(Items.Easton.Received > 0 ? "Easton" : "")}", 8f, 3.7f);
                    DrawText($"{(Items.Winton.Received > 0 ? "Winton" : "")}\n{(Items.Munton.Received > 0 ? "Munton" : "")}\n{(Items.Lopton.Received > 0 ? "Lopton" : "")}", 17f, 3.7f);
                    DrawText($"{(Items.Clifton.Received > 0 ? "Clifton" : "")}\n{(Items.Damton.Received > 0 ? "Damton" : "")}\n{(Items.Smalton.Received > 0 ? "Smalton" : "")}", 24f, 3.7f);
                }
                if (APData.blocked_tunnels == "1" || APData.blocked_tunnels == "2")
                {
                    DrawText($"{(Items.TunnelFactory.Received > 0 ? "Factory\nTunnel" : "")}", 8f, 7f);
                    DrawText($"{(Items.TunnelSP.Received > 0 ? "Tunnel" : "")}", 17f, 7f);
                    DrawText($"{(Items.TunnelFT.Received > 0 ? "Tunnel" : "")}", 24f, 7f);
                }
            }


            DrawText("Locations checked:", 9, 12);
            // DrawText(TrackerText.trackerChecksTextNew, 0, 13);

            int textHeight = 14;

            // Deliveries
            deliveries = 0;
            foreach (int id in Locations.Deliveries.Values)
            {
                if (checkedLocations.Contains(id)) deliveries += 1;
                if (checkedLocations.Contains(id + 10000)) deliveries += 1;
            }
            DrawText($"{(APData.perfect_deliveries == "2" ? "Perfect " : "")}Deliveries: {deliveries}/{(APData.perfect_deliveries == "1" ? total_deliveries*2 : total_deliveries)}", 0, 13);

            // Payload checks
            if (APData.payload_checks == "1")
            {
                int payloads = 0;
                foreach (int id in Locations.PayloadDeliveries.Values)
                {
                    if (checkedLocations.Contains(id)) payloads += 1;
                }
                DrawText($"Payload Deliveries: {payloads}/17", 0, textHeight);
                textHeight += 1;
            }

            // Radio Checks
            if (APData.radio_towers == "1" || APData.radio_towers == "3")
            {
                int radios = 0;
                for (int id = 60; id < 64; id++)
                {
                    if (checkedLocations.Contains(id)) radios += 1;
                }
                DrawText($"Radio Towers: {radios}/4", 0, textHeight);
                textHeight += 1;
            }

            // Blind Bags
            if (APData.blind_bags == "1")
            {
                int blindbags = 0;
                foreach (int id in Locations.BlindBags.Values)
                {
                    if (checkedLocations.Contains(id)) blindbags += 1;
                }
                DrawText($"Blind Bags: {blindbags}/12", 0, textHeight);
                textHeight += 1;
            }

            // Snowcats
            if (APData.snowcats == "1" || APData.snowcats == "2")
            {
                int snowcats = 0;
                for (int id = 40; id < 53; id++)
                {
                    if (checkedLocations.Contains(id)) snowcats += 1;
                }
                DrawText($"Snowcats: {snowcats}/{(APData.snowcats == "1" ? "13" : "12")}", 0, textHeight);
            }
        }
    }

    private void DrawSprite(Texture spriteSheet, Vector2 sprite, float x, float y)
    {
        Util.R.spr(spriteSheet, sprite.x, sprite.y, offset.x + P.x + spriteSize * x, offset.y + P.y + spriteSize * y, spriteSize, spriteSize, false, spriteSize, spriteSize);
    }

    private void DrawText(string text, float x, float y)
    {
        Util.R.put(text, offset.x + P.x + x * textSize, offset.y + P.y + y * textSize);
    }

    public static void TotalDeliveries()
    {
        total_deliveries = APData.intercity_deliveries switch
        {
            "0" => 27,
            "1" => 81,
            "2" => 36,
            "3" => 90,
            _ => 81,
        };
    }

    private void DeliveryGrid(float x, float y)
    {
        var checkedLocations = archipelago.session.Locations.AllLocationsChecked;

        Vector2 grid = new (0, 0);
        int del;
        bool sp = APLogic.CanReachSnowyPeaks();
        bool ft = APLogic.CanReachFishingTown() && APLogic.FTGateIsOpen();
        bool city;

        if (APData.intercity_deliveries == "0" || APData.intercity_deliveries == "2")
        {
            DrawText("Up We Ea", x + 3, y);
            DrawText("Wi Mu Lo", x + 12, y);
            DrawText("Cl Da Sm", x + 21, y);
            DrawText("Up\n\nWe\n\nEa", x, y + 2);
            DrawText("Wi\n\nMu\n\nLo", x + 9, y + 12);
            DrawText("Cl\n\nDa\n\nSm", x + 18, y + 21);
        }
        else
        {
            DrawText("Up We Ea Wi Mu Lo Cl Da Sm", x + 3, y);
            DrawText("Up\n\nWe\n\nEa\n\nWi\n\nMu\n\nLo\n\nCl\n\nDa\n\nSm", x, y + 2);
        }
        foreach (var from in Locations.Town)
        {
            foreach (var to in Locations.Town)
            {
                city = from.Value switch
                {
                    > 20 and < 24 => sp,
                    > 30 and < 34 => ft,
                    _ => true
                };
                if (!city)
                {
                    continue;
                }
                city = to.Value switch
                {
                    > 20 and < 24 => sp,
                    > 30 and < 34 => ft,
                    _ => true
                };
                if (!city)
                {
                    continue;
                }

                del = int.Parse($"{from.Value}{to.Value}");
                if (checkedLocations.Contains(del))
                {
                    DrawText($"x", x + 3 + grid.x, y + 2 + grid.y);
                }
                else if (APData.lock_towns == "0" || (Items.TownToItem[from.Key].Received > 0 && Items.TownToItem[to.Key].Received > 0))
                {
                    DrawText($"o", x + 3 + grid.x, y + 2 + grid.y);
                }
                if (checkedLocations.Contains(del + 10000))
                {
                    DrawText($"x", x + 4 + grid.x, y + 2 + grid.y);
                }
                else if (APData.lock_towns == "0" || (Items.TownToItem[from.Key].Received > 0 && Items.TownToItem[to.Key].Received > 0))
                {
                    DrawText($"o", x + 4 + grid.x, y + 2 + grid.y);
                }
                grid.y += 2;
            }
            grid.x += 3;
            grid.y = 0;
        }
    }
}

// Mostly obsolete
public class TrackerText
{
    private static readonly ArchipelagoClient archipelago = EasyDeliveryAP.ArchipelagoClient;
    static string trackerItemsText;
    public static string trackerChecksText;
    public static string trackerChecksTextNew;
    static int donePayloads;
    static int deliveries;
    static int blindBags;
    static int snowcats;
    static int radioTowers;
    public static string trackerText;

    public static void UpdateTracker()
    {
        DesktopDotExe.File file = EasyDeliveryAP.TrackerFile;
        trackerItemsText = "Items received:\n";
        trackerChecksText = "Locations checked:\n";
        trackerChecksTextNew = "";
        donePayloads = 0;
        deliveries = 0;
        blindBags = 0;
        snowcats = 0;
        radioTowers = 0;

        if (!ArchipelagoClient.Authenticated)
        {
            // file.data = "Not connected";
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

        if (APData.lock_towns == "1")
        {
            trackerItemsText += "  Towns: ";
            foreach (ItemData town in Items.TownToItem.Values)
            {
                if (town.Received >= 1)
                {
                    trackerItemsText += $"{town.Name}, ";
                }
            }
            trackerItemsText += "\n";
        }

        foreach (int id in Locations.Deliveries.Values)
        {
            if (checkedLocations.Contains(id)) deliveries += 1;
            if (checkedLocations.Contains(id + 10000)) deliveries += 1;
        }

        int total_deliveries = 81;

        switch(APData.intercity_deliveries)
        {
            case "0":
                total_deliveries = 27;
                break;
            case "1":
                total_deliveries = 81;
                break;
            case "2":
                total_deliveries = 36;
                break;
            case "3":
                total_deliveries = 90;
                break;
        }

        trackerChecksText += $"  {(APData.perfect_deliveries == "2" ? "Perfect " : "")}Deliveries: {deliveries}/{(APData.perfect_deliveries == "1" ? total_deliveries*2 : total_deliveries)}\n";
        trackerChecksTextNew += $"{(APData.perfect_deliveries == "2" ? "Perfect " : "")}Deliveries: {deliveries}/{(APData.perfect_deliveries == "1" ? total_deliveries*2 : total_deliveries)}\n";

        if (APData.payload_checks == "1")
        {
            foreach (int id in Locations.PayloadDeliveries.Values)
            {
                if (checkedLocations.Contains(id)) donePayloads += 1;
            }
            trackerChecksText += $"  Payload Deliveries: {donePayloads}/17\n";
            trackerChecksTextNew += $"Payload Deliveries: {donePayloads}/17\n";
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
                trackerChecksTextNew += $"Radio Towers: {radioTowers}/4\n";
                break;
            case "2":
                for (int id = 60; id < 64; id++)
                {
                    if (checkedLocations.Contains(id)) radioTowers += 1;
                }
                trackerChecksText += $"  Radio Towers: {radioTowers}/4\n";
                trackerChecksTextNew += $"Radio Towers: {radioTowers}/4\n";
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
            trackerChecksTextNew += $"Blind Bags: {blindBags}/12\n";
        }

        if (APData.snowcats == "1" || APData.snowcats == "2")
        {
            for (int id = 40; id < 53; id++)
            {
                if (checkedLocations.Contains(id)) snowcats += 1;
            }
            trackerChecksText += $"  Snowcats: {snowcats}/{(APData.snowcats == "1" ? "13" : "12")}\n";
            trackerChecksTextNew += $"Snowcats: {snowcats}/{(APData.snowcats == "1" ? "13" : "12")}\n";
        }

        trackerText = trackerItemsText + trackerChecksText;
    }
}