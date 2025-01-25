
using UnityEngine;

public abstract class GearDisplaySO : ScriptableObject
{
    [Header("Gear Parameters")]
    [SerializeField] protected GameObject gearPrefab; // Prefab for the gear part
    [SerializeField] protected Vector3 positionAdjustmentVector; // Offset for placement
    [SerializeField] protected Vector3 rotationAdjustmentVector; // Rotation for placement

    public abstract void Use(GameObject controlledCar, string attachmentPoint);
}
