using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    public class MyFloatEvent : UnityEvent<float> { }
    public MyFloatEvent OnAbilityUse = new MyFloatEvent();

    [SerializeField] private List<AbilityBase> abilities;

    [SerializeField] float cooldownTime = 5;
    [SerializeField] public bool canUse = true;


    [SerializeField] private Image abilityCanvas;


    private void Start()
    {
        UpdateAbilitySprite(abilities[0]);
    }
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

    public void UpdateAbilitySprite(AbilityBase abilityIndex) {
        abilityCanvas.sprite = abilityIndex.icon; 
    }
}
