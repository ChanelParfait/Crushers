using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class AbilityBase : MonoBehaviour
{
    public class MyFloatEvent : UnityEvent<float> { }
    public MyFloatEvent OnAbilityUse = new MyFloatEvent();

    [SerializeField] Ability ability;
    [SerializeField] float cooldownTime;
    [SerializeField] bool canUse = true;

    public void UseAbility() {
        if (canUse)
        {
            OnAbilityUse.Invoke(cooldownTime);
            Ability();
            StartCooldown();

        }
    }

    public abstract void Ability();

    public void StartCooldown() {
        StartCoroutine(Cooldown());
        IEnumerator Cooldown() {
            canUse = false;
            yield return new WaitForSeconds(cooldownTime);
            canUse = true;
        }
    }
}
