using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpringJump", menuName = "Abilities/SpringJump")]
public class TestAbility : AbilityBase
{
    public override void Use()
    {
        Debug.Log("Ability used");
    }

}
