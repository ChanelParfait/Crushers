using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AbilityManager : MonoBehaviour
{
    public class MyFloatEvent : UnityEvent<float> { }
    public MyFloatEvent OnAbilityUse = new MyFloatEvent();

    [SerializeField] private List<AbilityBase> abilities;

    [SerializeField] float cooldownTime = 1;
    [SerializeField] public bool canUse = true;

    public void UseAbility() {
        if (canUse)
        {
            if (abilities != null)
            {
                OnAbilityUse.Invoke(cooldownTime);
                var ability = abilities[0];
                ability.Use();
                StartCooldown();
            }
        }
    }

    public void StartCooldown()
    {
        StartCoroutine(Cooldown());
        IEnumerator Cooldown()
        {
            canUse = false;
            yield return new WaitForSeconds(cooldownTime);
            canUse = true;
        }
    }
}
