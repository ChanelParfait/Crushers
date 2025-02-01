using System.Collections.Generic;
using UnityEngine;

public class GearManager : MonoBehaviour
{
    public List<Gear> AvailableGears; // Predefined gears
    
    public Gear GetGearByID(string gearID)
    {
        return AvailableGears.Find(gear => gear.GearID == gearID);
    }
}
