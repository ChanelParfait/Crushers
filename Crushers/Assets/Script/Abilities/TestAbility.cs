using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestAbility", menuName ="Abilities/TestAbility")]
public class TestAbility : AbilityBase
{
    public override void Use()
    {
        Debug.Log("Ability used");
    }

}
