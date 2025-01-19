using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "StunGun", menuName = "Abilities/StunGun")]
public class StunGun : AbilityBase
{
    [Space(10)] 
    [Header("Ability parameters")] 
    [Space(20)]
    [SerializeField] private GameObject stungunPrefab;
    [SerializeField] private Vector3 positionAdjustmentVector;
    public override void Use(GameObject controlledCar)
    {
        
    }
}
