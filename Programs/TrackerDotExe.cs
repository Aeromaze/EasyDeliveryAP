using System.Collections.Generic;
using System.Collections.ObjectModel;
using EasyDeliveryAP.Archipelago;
using EasyDeliveryAP.Utils;
using EasyDeliveryAPI;
using UnityEngine;

namespace EasyDeliveryAP;

public class TrackerApp : MonoBehaviour
{
    private static readonly ArchipelagoClient archipelago = EasyDeliveryAP.ArchipelagoClient;
    private ReadOnlyCollection<long> checkedLocations;

    private UIUtil Util = new();
    private Rect P;
    private Vector2 offset = new(5f, 5f);
    private readonly float textSize = 8;
    private readonly float spriteSize = 16;

    private static int total_deliveries = 81;
    private int deliveries;

    // spritesheets
    Texture altSpriteSheet;
    Texture spriteSheet;
    Texture spriteSheetBackup;

    private int page = 0;
    private bool pageIndex = false;

    public static List<int> pages = [0, 1]; // page index list initiallized with always active pages
    
    private readonly static Dictionary<int, string> ButtonText = new()
    {
        {0, "Overview"},
        {1, "Town Deliveries"},
        {2, "City Deliveries"},
        {3, "Payload Deliveries"},
        {4, "Snowcats"},
        {5, "Blindbags"},
        {6, "Radio Towers"},
        {7, "Vending Machines"},
        {8, "Bins"},
        {9, "Signs"}
    };

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
        checkedLocations = archipelago.session.Locations.AllLocationsChecked;

        altSpriteSheet = window.M.altSpriteSheet;
        spriteSheet = window.M.spriteSheet;
        spriteSheetBackup = window.M.spriteSheetBackup;

        if (Util.Button("Index", P.x + offset.x + 208, P.y + offset.y + 192, true))
        {
            pageIndex = !pageIndex;
        }
        if (pageIndex)
        {
            int indexPosition = 0;
            foreach (int page in pages)
            {
                if (Util.Button(ButtonText[page], P.x, P.y + indexPosition * 8))
                {
                    this.page = indexPosition;
                    pageIndex = false;
                }
                indexPosition++;
            }
        }
        else
        {
            if (Util.Button("Teleport", P.x + offset.x, P.y + offset.y + 192, true))
            {
                CarPatches.teleport = true;
            }

            DrawText($"Page {page+1}/{pages.Count}", 12, 24);
            if (Util.Button(" < ", P.x + offset.x + 72, P.y + offset.y + 192, true))
            {
                if (page > 0)
                {
                    page -= 1;
                }
                else
                {
                    page = pages.Count - 1;
                }
            }
            if(Util.Button(" > ", P.x + offset.x + 176, P.y + offset.y + 192, true))
            {
                if (page < pages.Count - 1)
                {
                    page += 1;
                }
                else
                {
                    page = 0;
                }
            }
            switch (pages[page])
            {
                case 0: // Overview
                    Overview(0, 0);
                    break;
                case 1: // Town Deliveries
                    DrawText("Town Deliveries", 4, 0);
                    TownDeliveryGrid(0, 3);
                    break;
                case 2: // City Deliveries
                    DrawText("City Deliveries", 4, 0);
                    CityDeliveryGrid(); // add parameters to the ones missing?
                    break;
                case 3: // Payload Deliveries
                    DrawText("Payload Deliveries", 4, 0);
                    PayloadDelivery();
                    break;
                case 4: // Snowcats
                    DrawText("Snowcats", 4, 0);
                    Snowcats();
                    break;
                case 5: // Blindbags
                    DrawText("Blindbags", 4, 0);
                    BlindBags(0, 3);
                    break;
                case 6: // Radio Towers
                    DrawText("Radio Towers", 4, 0);
                    Radio(0, 3);
                    break;
                case 7: // Vending Machines
                    DrawText("Vending Machines", 4, 0);
                    VendingMachines(0, 3);
                    break;
                case 8: // Bins
                    DrawText("Bins", 4, 0);
                    GarbageBins(0, 3);
                    break;
                case 9: // Signs
                    DrawText("Signs", 4, 0);
                    Signs(0, 3);
                    break;
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

    private char? LogicIndicator(int locID, bool inLogic)
    {
        if (checkedLocations.Contains(locID))
        {
            return 'x';
        }
        else if (inLogic)
        {
            return 'o';
        }
        else
        {
            return null;
        }
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

    // Town Deliveries
    private void TownDeliveryGrid(float x, float y)
    {
        Vector2 grid = new (0, 0);
        int del;
        bool sp = APLogic.CanReachSnowyPeaks();
        bool ft = APLogic.CanReachFishingTown() && APLogic.FTGateIsOpen();
        bool city;
        bool inLogic;

        if (APData.intercity_deliveries == "0" || APData.intercity_deliveries == "2")
        {
            DrawText("Up We Ea", x + 3, y);
            DrawText("Wi Mu Lo", x + 12, y + 6);
            DrawText("Cl Da Sm", x + 21, y + 12);
            DrawText("Up\n\nWe\n\nEa", x, y + 2);
            DrawText("Wi\n\nMu\n\nLo", x + 9, y + 8);
            DrawText("Cl\n\nDa\n\nSm", x + 18, y + 14);
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
                if (!(APData.intercity_deliveries == "1" || APData.intercity_deliveries == "3") && ((int)(from.Value/10) != (int)(to.Value/10)))
                {
                    grid.y += 2;
                    continue;
                }

                city = from.Value switch
                {
                    > 20 and < 24 => sp,
                    > 30 and < 34 => ft,
                    _ => true
                };
                if (city)
                {
                    city = to.Value switch
                    {
                        > 20 and < 24 => sp,
                        > 30 and < 34 => ft,
                        _ => true
                    };
                }

                del = int.Parse($"{from.Value}{to.Value}");
                inLogic = city && (APData.lock_towns == "0" || (Items.TownToItem[from.Key].Received > 0 && Items.TownToItem[to.Key].Received > 0));

                if (APData.perfect_deliveries == "0" || APData.perfect_deliveries == "1")
                {
                    DrawText($"{LogicIndicator(del, inLogic)}", x + 3 + grid.x, y + 2 + grid.y);
                }
                if (APData.perfect_deliveries == "1" || APData.perfect_deliveries == "2")
                {
                    DrawText($"{LogicIndicator(del + 10000, inLogic)}", x + 4 + grid.x, y + 2 + grid.y);
                }
                grid.y += 2;
            }
            grid.x += 3;
            grid.y = 0;
        }
    }

    private void CityDeliveryGrid()
    {
        if (APData.intercity_deliveries == "2" || APData.intercity_deliveries == "3")
        {
            Vector2 grid = new (0, 0);
            int del;
            bool sp = APLogic.CanReachSnowyPeaks();
            bool ft = APLogic.CanReachFishingTown() && APLogic.FTGateIsOpen();
            bool inLogic;

            DrawText("Mt Sp Ft", 3, 3);
            DrawText("Mt\n\nSp\n\nFt", 0, 5);
            foreach (var from in Locations.City)
            {
                foreach (var to in Locations.City)
                {
                    if (((from.Key == "Snowy Peaks" || to.Key == "Snowy Peaks") && (!sp || !(Items.Winton.Received > 0 || Items.Munton.Received > 0 || Items.Lopton.Received > 0))) || 
                        ((from.Key == "Fishing Town" || to.Key == "Fishing Town") && (!ft || !(Items.Clifton.Received > 0 || Items.Damton.Received > 0 || Items.Smalton.Received > 0))))
                    {
                        inLogic = false;
                    }
                    else
                    {
                        inLogic = true;
                    }
                    del = int.Parse($"{from.Value}{to.Value}");
                    if (APData.perfect_deliveries == "0" || APData.perfect_deliveries == "1")
                    {
                        DrawText($"{LogicIndicator(del, inLogic)}", 3 + grid.x, 5 + grid.y);
                    }
                    if (APData.perfect_deliveries == "1" || APData.perfect_deliveries == "2")
                    {
                        DrawText($"{LogicIndicator(del + 10000, inLogic)}", 4 + grid.x, 5 + grid.y);
                    }
                    grid.y += 2;
                }
                grid.x += 3;
                grid.y = 0;
            }
        }
    }

    // Overview tracker screen
    private void Overview(float x, float y)
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
            textHeight += 1;
        }

        // Vending Machines
        if (APData.vending_machines == "1")
        {
            int vending = 0;
            for (int id = 100; id < 112; id++)
            {
                if (checkedLocations.Contains(id)) vending += 1;
            }
            DrawText($"Vending Machines: {vending}/12", 0, textHeight);
            textHeight += 1;
        }

        // Bins
        if (APData.trash_bins == "1")
        {
            int bins = 0;
            for (int id = 121; id < 163; id++)
            {
                if (checkedLocations.Contains(id)) bins += 1;
            }
            DrawText($"Garbage Bins: {bins}/62", 0, textHeight);
            textHeight += 1;
        }

        // Signs
        if (APData.signs == "1")
        {
            int signs = 0;
            for (int id = 200; id < 251; id++)
            {
                if (checkedLocations.Contains(id)) signs += 1;
            }
            DrawText($"Signs: {signs}/51", 0, textHeight);
            textHeight += 1;
        }
    }

    private void PayloadDelivery()
    {
        if (APData.payload_checks == "1")
        {
            bool sp = APLogic.CanReachSnowyPeaks();
            bool ft = APLogic.CanReachFishingTown() && APLogic.FTGateIsOpen();

            bool boxes = Items.Weston.Received > 0 || Items.Easton.Received > 0 || (sp && (Items.Munton.Received > 0 || Items.Lopton.Received > 0)) || (ft && (Items.Clifton.Received > 0 || Items.Damton.Received > 0));
            bool bar = Items.Weston.Received > 0 || (sp && (Items.Munton.Received > 0 || Items.Lopton.Received > 0)) || (ft && (Items.Clifton.Received > 0 || Items.Damton.Received > 0));
            bool pizza = Items.Upton.Received > 0 || Items.Easton.Received > 0 || (sp && (Items.Winton.Received > 0 || Items.Lopton.Received > 0) || (ft && Items.Smalton.Received > 0));
            bool plant = Items.Upton.Received > 0 || Items.Easton.Received > 0 || (sp && (Items.Winton.Received > 0 || Items.Munton.Received > 0)) || (ft && (Items.Clifton.Received > 0 || Items.Smalton.Received > 0));

            DrawText("Box Bunch:", 0, 6);
            DrawText("Box Stack:", 0, 7);
            DrawText("Crate of Drinks:", 0, 9);
            DrawText("Sack:", 0, 18);
            DrawText("Sack Stack:", 0, 19);
            DrawText("Big Box:", 0, 4);
            DrawText("Big Box Stack:", 0, 5);
            DrawText("Crate:", 0, 8);
            DrawText("Crate Stack:", 0, 10);
            DrawText("Lots of Crates of Drinks:", 0, 11);
            DrawText("Drink:", 0, 20);
            DrawText("Pizza Stack:", 0, 12);
            DrawText("Pizza Stack Mega:", 0, 13);
            DrawText("Plant Pot:", 0, 14);
            DrawText("Plant Pot Bunch:", 0, 15);
            DrawText("Plant Pot Stack:", 0, 16);
            DrawText("Plant Pot Wide:", 0, 17);

            DrawText($"{LogicIndicator(3, true)}", 26, 6);
            DrawText($"{LogicIndicator(4, true)}", 26, 7);
            DrawText($"{LogicIndicator(6, true)}", 26, 9);
            DrawText($"{LogicIndicator(15, true)}", 26, 18);
            DrawText($"{LogicIndicator(16, true)}", 26, 19);
            DrawText($"{LogicIndicator(1, boxes)}", 26, 4);
            DrawText($"{LogicIndicator(2, boxes)}", 26, 5);
            DrawText($"{LogicIndicator(5, boxes)}", 26, 8);
            DrawText($"{LogicIndicator(7, boxes)}", 26, 10);
            DrawText($"{LogicIndicator(8, bar)}", 26, 11);
            DrawText($"{LogicIndicator(17, bar)}", 26, 20);
            DrawText($"{LogicIndicator(9, pizza)}", 26, 12);
            DrawText($"{LogicIndicator(10, pizza)}", 26, 13);
            DrawText($"{LogicIndicator(11, plant)}", 26, 14);
            DrawText($"{LogicIndicator(12, plant)}", 26, 15);
            DrawText($"{LogicIndicator(13, plant)}", 26, 16);
            DrawText($"{LogicIndicator(14, plant)}", 26, 17);
        }
    }

    private void Snowcats()
    {
        // Snowcat logic tracker
        bool snowtires = APLogic.HasSnowTires();
        bool sp = APLogic.CanReachSnowyPeaks();
        bool ft = APLogic.CanReachFishingTown();
        bool gate = APLogic.FTGateIsOpen() && ft;
        if (APData.snowcats == "1" || APData.snowcats == "2")
        {
            DrawText("Theo:", 0, 4);
            DrawText("Cici:", 0, 5);
            DrawText("Tooey:", 0, 9);
            DrawText("Gus:", 0, 14);
            DrawText("Fives:", 0, 11);
            DrawText("Sixo:", 0, 12);
            DrawText("Foreman:", 0, 8);
            DrawText("Seb:", 0, 15);
            DrawText("Reed:", 0, 10);
            DrawText("Fortino:", 0, 6);
            DrawText("Ellie:", 0, 7);
            DrawText("Fit:", 0, 13);

            DrawText($"{LogicIndicator(40, true)}", 9, 4);
            DrawText($"{LogicIndicator(41, true)}", 9, 5);
            DrawText($"{LogicIndicator(45, snowtires)}", 9, 9);
            DrawText($"{LogicIndicator(51, snowtires)}", 9, 14);
            DrawText($"{LogicIndicator(47, sp)}", 9, 11);
            DrawText($"{LogicIndicator(48, sp)}", 9, 12);
            DrawText($"{LogicIndicator(44, sp)}", 9, 8);
            DrawText($"{LogicIndicator(52, sp)}", 9, 15);
            DrawText($"{LogicIndicator(46, ft)}", 9, 10);
            DrawText($"{LogicIndicator(42, gate)}", 9, 6);
            DrawText($"{LogicIndicator(43, gate)}", 9, 7);
            DrawText($"{LogicIndicator(50, gate && Items.Chains.Received > 0)}", 9, 13);
        }
        if (APData.snowcats == "1")
        {
            DrawText("Ada:", 0, 16);

            DrawText($"{(checkedLocations.Contains(49) ? "x" : "o")}", 9, 16);
        }
    }
    
    private void Radio(float x, float y)
    {
        bool sp = APLogic.CanReachSnowyPeaks();
        bool ft = APLogic.CanReachFishingTown() && APLogic.FTGateIsOpen();
        
        DrawText("Upton:", 0 + x, 0 + y);
        DrawText("Easton:", 0 + x, 1 + y);
        DrawText("Snowy Peaks:", 0 + x, 2 + y);
        DrawText("Fishing Town:", 0 + x, 3 + y);

        DrawText($"{LogicIndicator(60, true)}", 15 + x, 0 + y);
        DrawText($"{LogicIndicator(61, APLogic.HasSnowTires() || Items.Lighter.Received > 0)}", 15 + x, 1 + y);
        DrawText($"{LogicIndicator(62, sp)}", 15 + x, 2 + y);
        DrawText($"{LogicIndicator(63, ft && Items.Chains.Received > 0)}", 15 + x, 3 + y);
    }

    private void BlindBags(float x, float y)
    {
        bool sp = APLogic.CanReachSnowyPeaks();
        bool ft = APLogic.CanReachFishingTown() && APLogic.FTGateIsOpen();
        bool logic = true;

        int bag;

        DrawText("1 2 3 4", 17 + x, y);
        DrawText("Mountain Town:", x, 1 + y);
        DrawText("Snowy Peaks:", x, 2 + y);
        DrawText("Fishing Town:", x, 3 + y);

        for (int i = 20; i <= 31; i++)
        {
            bag = (i - 20) % 4;
            if (i > 23)
            {
                logic = i > 27 ? ft : sp;
            }
            DrawText($"{LogicIndicator(i, logic)}", 17 + x + bag * 2, 1 + y + (int)((i - 20)/4));
        }
    }

    private void VendingMachines(float x, float y)
    {
        bool sp = APLogic.CanReachSnowyPeaks();
        bool ft = APLogic.CanReachFishingTown() && APLogic.FTGateIsOpen();

        DrawText("Upton:", x, y);
        DrawText("Weston:", x, y + 1);
        DrawText("Easton:", x, y + 2);
        DrawText("Winton:", x, y + 3);
        DrawText("Lower Munton:", x, y + 4);
        DrawText("Upper Munton:", x, y + 5);
        DrawText("Lopton:", x, y + 6);
        DrawText("Clifton:", x, y + 7);
        DrawText("Upper Damton:", x, y + 8);
        DrawText("Lower Damton:", x, y + 9);
        DrawText("Smalton (Left):", x, y + 10);
        DrawText("Smalton (Right):", x, y + 11);

        DrawText($"{LogicIndicator(100, (APData.lock_towns != "1" || Items.Upton.Received > 0))}", x + 18, y);
        DrawText($"{LogicIndicator(101, (APData.lock_towns != "1" || Items.Weston.Received > 0))}", x + 18, y + 1);
        DrawText($"{LogicIndicator(102, (APData.lock_towns != "1" || Items.Easton.Received > 0))}", x + 18, y + 2);
        DrawText($"{LogicIndicator(103, sp && (APData.lock_towns != "1" || Items.Winton.Received > 0))}", x + 18, y + 3);
        DrawText($"{LogicIndicator(104, sp && (APData.lock_towns != "1" || Items.Munton.Received > 0))}", x + 18, y + 4);
        DrawText($"{LogicIndicator(105, sp && (APData.lock_towns != "1" || Items.Munton.Received > 0))}", x + 18, y + 5);
        DrawText($"{LogicIndicator(106, sp && (APData.lock_towns != "1" || Items.Lopton.Received > 0))}", x + 18, y + 6);
        DrawText($"{LogicIndicator(107, ft && (APData.lock_towns != "1" || Items.Clifton.Received > 0))}", x + 18, y + 7);
        DrawText($"{LogicIndicator(108, ft && (APData.lock_towns != "1" || Items.Damton.Received > 0))}", x + 18, y + 8);
        DrawText($"{LogicIndicator(109, ft && (APData.lock_towns != "1" || Items.Damton.Received > 0))}", x + 18, y + 9);
        DrawText($"{LogicIndicator(110, ft && (APData.lock_towns != "1" || Items.Smalton.Received > 0))}", x + 18, y + 10);
        DrawText($"{LogicIndicator(111, ft && (APData.lock_towns != "1" || Items.Smalton.Received > 0))}", x + 18, y + 11);
    }

    private void GarbageBins(float x, float y)
    {
        bool sp = APLogic.CanReachSnowyPeaks();
        bool ft = APLogic.CanReachFishingTown() && APLogic.FTGateIsOpen();

        int upton = 0;
        int weston = 0;
        int easton = 0;
        int winton = 0;
        int munton = 0;
        int lopton = 0;
        int clifton = 0;
        int damton = 0;
        int smalton = 0;

        DrawText("Upton:\nWeston:\nEaston:\nWinton:\nMunton:\nLopton:\nClifton:\nDamton:\nSmalton:", x, y);

        for (int i = 121; i <= 162; i++)
        {
            if (checkedLocations.Contains(i))
            {
                switch (i)
                {
                    case <= 126:
                        upton++;
                        break;
                    case <= 135:
                        weston++;
                        break;
                    case <= 140:
                        easton++;
                        break;
                    case <= 144:
                        winton++;
                        break;
                    case <= 147:
                        munton++;
                        break;
                    case <= 150:
                        lopton++;
                        break;
                    case <= 154:
                        clifton++;
                        break;
                    case <= 158:
                        damton++;
                        break;
                    case <= 162:
                        smalton++;
                        break;
                }
            }
        }
        DrawText($"{(upton == 6 ? "x" : (APData.lock_towns != "1" || Items.Upton.Received > 0) ? "o" : " ")} ({upton}/6)", x + 10, y);
        DrawText($"{(weston == 9 ? "x" : (APData.lock_towns != "1" || Items.Weston.Received > 0) ? "o" : " ")} ({weston}/9)", x + 10, y + 1);
        DrawText($"{(easton == 5 ? "x" : (APData.lock_towns != "1" || Items.Easton.Received > 0) ? "o" : " ")} ({easton}/5)", x + 10, y + 2);
        DrawText($"{(winton == 4 ? "x" : sp && (APData.lock_towns != "1" || Items.Winton.Received > 0) ? "o" : " ")} ({winton}/4)", x + 10, y + 3);
        DrawText($"{(munton == 3 ? "x" : sp && (APData.lock_towns != "1" || Items.Munton.Received > 0) ? "o" : " ")} ({munton}/3)", x + 10, y + 4);
        DrawText($"{(lopton == 3 ? "x" : sp && (APData.lock_towns != "1" || Items.Lopton.Received > 0) ? "o" : " ")} ({lopton}/3)", x + 10, y + 5);
        DrawText($"{(clifton == 4 ? "x" : ft && (APData.lock_towns != "1" || Items.Clifton.Received > 0) ? "o" : " ")} ({clifton}/4)", x + 10, y + 6);
        DrawText($"{(damton == 4 ? "x" : ft && (APData.lock_towns != "1" || Items.Damton.Received > 0) ? "o" : " ")} ({damton}/4)", x + 10, y + 7);
        DrawText($"{(smalton == 4 ? "x" : ft && (APData.lock_towns != "1" || Items.Smalton.Received > 0) ? "o" : " ")} ({smalton}/4)", x + 10, y + 8);
    }

    private void Signs(float x, float y)
    {
        bool sp = APLogic.CanReachSnowyPeaks();
        bool ft = APLogic.CanReachFishingTown() && APLogic.FTGateIsOpen();

        int mountain = 0;
        int snowy = 0;
        int fishing = 0;

        DrawText("Mountain Town:\nSnowy Peaks:\nFishing Town:", x, y);

        for (int i = 200; i <= 250; i++)
        {
            if (checkedLocations.Contains(i))
            {
                switch (i)
                {
                    case <= 215:
                        mountain++;
                        break;
                    case <= 225:
                        snowy++;
                        break;
                    case <= 250:
                        fishing++;
                        break;
                }
            }
        }
        DrawText($"{(mountain == 16 ? "x" : "o")} ({mountain}/16)", x + 17, y);
        DrawText($"{(snowy == 10 ? "x" : sp ? "o" : " ")} ({snowy}/10)", x + 17, y + 1);
        DrawText($"{(fishing == 25 ? "x" : ft ? "o" : " ")} ({fishing}/25)", x + 17, y + 2);
    }
}
