using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "KamikazeBomb", menuName = "Abilities/KamikazeBomb")]
public class KamikazeBomb : AbilityBase
{
    [Space(10)] 
    [Header("Ability parameters")] 
    [Space(20)]
    [SerializeField] private GameObject kazikazebombPrefab;
    [SerializeField] private Vector3 positionAdjustmentVector;
    public override void Use(GameObject controlledCar)
    {
        
    }
}
