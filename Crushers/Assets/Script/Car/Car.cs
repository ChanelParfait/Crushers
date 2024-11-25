using UnityEngine;

[CreateAssetMenu(fileName = "Car", menuName = "Cars/Make New Car", order = 0)]
public class Car : ScriptableObject
{
    [SerializeField] GameObject carPrefab = null;

    [Header("Car Setup")]


    [Header("Acceleration")]
    [Range(20, 250)]
    [SerializeField] private int maxSpeed = 90;
    [Range(10, 120)]
    [SerializeField] private int maxReverseSpeed = 45;
    [Range(1, 15)] 
    [SerializeField] private int accelerationMultiplier = 2;

    [Header("Steering")]
    [Range(10, 50)] 
    [SerializeField] private int maxSteeringAngle = 27;
    [Range(0.1f, 2f)] 
    [SerializeField] private float steeringSpeed = 0.5f;

    [Header("Deceleration")]
    [Range(1000, 2000)] 
    [SerializeField] private int brakeForce = 350;
    [Range(1, 10)] 
    [SerializeField] private float decelerationMultiplier = 2;
    [Range(1, 10)] 
    [SerializeField] private int handbrakeDriftMultiplier = 5;

    [Header("Mass and Physics")]

    private Vector3 bodyMassCenter;
    [Range(200, 1000)]
    [SerializeField] private int bodyMass;
    [Range(200,1000)]
    [SerializeField] private int gravityMultiplier;
    [Range(1000, 5000)]
    [SerializeField] private float damageMultiplier =  50f;

    


    // Getter Methods
    public GameObject GetCarPrefab()
    {
        return carPrefab;
    }

    public int GetMaxSpeed()
    {
        return maxSpeed;
    }

    public int GetMaxReverseSpeed()
    {
        return maxReverseSpeed;
    }

    public int GetAccelerationMultiplier()
    {
        return accelerationMultiplier;
    }

    public int GetMaxSteeringAngle()
    {
        return maxSteeringAngle;
    }

    public float GetSteeringSpeed()
    {
        return steeringSpeed;
    }

    public int GetBrakeForce()
    {
        return brakeForce;
    }

    public float GetDecelerationMultiplier()
    {
        return decelerationMultiplier;
    }

    public int GetHandbrakeDriftMultiplier()
    {
        return handbrakeDriftMultiplier;
    }
    public Vector3 GetBodyMassCenter()
    {
        return bodyMassCenter;
    }
    public int GetBodyMass()
    {
        return bodyMass;
    }
    public float GetDamageMultiplier() {
        return damageMultiplier;
    }

    public int GetGravityMultiplier() {
        return gravityMultiplier;
    }
}
