using UnityEngine;

public enum PUtype
{
    Traps,
    Stats,
    Projectile
}
[CreateAssetMenu(menuName = "Pickup/PickupConfig")]
public class PickupConfig : ScriptableObject
{
    
    public PUtype PUtype;

    // Fields for Pickup (Make changes in PickUpConfigEditor for your needed PUtype)
    // Float variables
    [HideInInspector] public float DmgMultiplier;
    [HideInInspector] public float duration;
    [HideInInspector] public float StatsMultiplier;
    [HideInInspector] public float speedMultiplier;
    [HideInInspector] public float radius;
    
    // Bool Variables
    [HideInInspector] public bool homing;
    
    // Getters
    public float GetDmgMultiplier()
    {
        return DmgMultiplier;
    }
    
    public float GetDuration()
    {
        return duration;
    }
    
    public float GetStatsMultiplier()
    {
        return StatsMultiplier;
    }
    
    public float GetSpeedMultiplier()
    {
        return speedMultiplier;
    }
    
    public float GetRadius()
    {
        return radius;
    }
    
    public bool GetHoming()
    {
        return homing;
    }
}

