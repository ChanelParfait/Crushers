using UnityEngine;

[CreateAssetMenu(fileName = "Car", menuName = "Cars/Make New Car", order = 0)]
public class Car : ScriptableObject
{
    [Header("Acceleration")]
    [Range(20, 250)]
    [SerializeField] private int baseMaxSpeed = 90;
    [Range(10, 120)]
    [SerializeField] private int baseMaxReverseSpeed = 45;
    [Range(1, 15)] 
    [SerializeField] private int baseAccelerationMultiplier = 2;

    [Header("Steering")]
    [Range(10, 50)] 
    [SerializeField] private int baseMaxSteeringAngle = 27;
    [Range(0.1f, 2f)] 
    [SerializeField] private float baseSteeringSpeed = 0.5f;

    [Header("Deceleration")]
    [Range(2000, 5000)] 
    [SerializeField] private int baseBrakeForce = 350;
    [Range(1, 10)] 
    [SerializeField] private float baseDecelerationMultiplier = 2;
    [Range(1, 10)] 
    [SerializeField] private int baseHandbrakeDriftMultiplier = 5;

    [Header("Mass and Physics")]

    [Range(600, 1000)]
    [SerializeField] private int baseBodyMass;
    [Range(200,1000)]
    [SerializeField] private int baseGravityMultiplier;
    [Range(1000, 5000)]
    [SerializeField] private float baseDamageMultiplier =  50f;

    


    // Getter Methods
    public int GetBaseMaxSpeed()
    {
        return baseMaxSpeed;
    }

    public int GetBaseMaxReverseSpeed()
    {
        return baseMaxReverseSpeed;
    }

    public int GetBaseAccelerationMultiplier()
    {
        return baseAccelerationMultiplier;
    }

    public int GetBaseMaxSteeringAngle()
    {
        return baseMaxSteeringAngle;
    }

    public float GetBaseSteeringSpeed()
    {
        return baseSteeringSpeed;
    }

    public int GetBaseBrakeForce()
    {
        return baseBrakeForce;
    }

    public float GetBaseDecelerationMultiplier()
    {
        return baseDecelerationMultiplier;
    }

    public int GetBaseHandbrakeDriftMultiplier()
    {
        return baseHandbrakeDriftMultiplier;
    }

    public int GetBaseBodyMass()
    {
        return baseBodyMass;
    }
    public float GetBaseDamageMultiplier() {
        return baseDamageMultiplier;
    }

    public int GetBaseGravityMultiplier() {
        return baseGravityMultiplier;
    }
}
