using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GrapplingHook", menuName = "Abilities/GrapplingHook")]
public class GrapplingHook : AbilityBase
{
    [Space(10)]
    [Header("Ability parameters")]
    [Space(20)]
    [SerializeField] private GameObject grapplingHookPrefab;

    public override void Use(GameObject controlledCar)
    {
        throw new System.NotImplementedException();
    }

}
