namespace EasyDeliveryAP.Utils;

public class APLogic
{
    public static bool CanReachSnowyPeaks()
    {
        if (Items.Lighter.Received > 0 && Items.Tires.Received > 0)
        {
            if (APData.blocked_tunnels != "1") return true;
            else if (Items.TunnelSP.Received > 0) return true;
        }
        return false;
    }

    public static bool CanReachFishingTown()
    {
        if (!HasSnowTires() || (APData.blocked_tunnels == "1" && Items.TunnelFT.Received < 1)) return false;
        if (FTGateIsOpen()) return true;
        return false;
    }

    public static bool CanAccessIntercity()
    {
        if (CanReachSnowyPeaks() || CanReachFishingTown()) return true;
        return false;
    }

    // To determine if there's free passage to Fishing Town
    public static bool FTGateIsOpen()
    {
        if (APData.radio_towers == "1" || APData.radio_towers == "3")
        {
            if (Items.RadioTower.Received >= 3) return true;
        }
        // TODO: change to actally check if the gate can be opened
        else if (CanReachSnowyPeaks() && Items.Bumper.Received > 0) return true;
        return false;
    }

    public static bool HasSnowTires()
    {
        if (Items.Tires.Received > 0) 
        {
            if (APData.car_upgrades == "1") return true;
            if (CanReachSnowyPeaks()) return true;
        }
        return false;
    }

    /// <summary>NOT YET IMPLEMENTED! Checks if the Snowy Peaks tunnel has already been entered.</summary>
    public static bool HasEnteredSPTunnel()
    {
        return false;
    }
}