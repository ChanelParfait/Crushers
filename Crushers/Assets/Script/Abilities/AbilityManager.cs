using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    public class MyFloatEvent : UnityEvent<float> { }
    public static MyFloatEvent OnAbilityUse = new MyFloatEvent();

    [SerializeField] private List<AbilityBase> abilities;


    [SerializeField] public bool canUse = true;


    [SerializeField] private Image abilityCanvas;
    [SerializeField] private GameObject controlledCar;

    private void Start()
    {
        if(abilities != null){
            UpdateAbilitySprite(abilities[0]);
        }
        controlledCar = this.gameObject;
    }
    public void UseAbility() {
        if (canUse)
        {
            if (abilities != null)
            {
                OnAbilityUse.Invoke(abilities[0].GetCooldownTime());
                var ability = abilities[0];
                ability.Use(controlledCar);
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
            yield return new WaitForSeconds(abilities[0].GetCooldownTime());
            canUse = true;
        }
    }

    public void UpdateAbilitySprite(AbilityBase abilityIndex) {
        abilityCanvas.sprite = abilityIndex.icon; 
    }

    public float GetAbilityCooldownTime() {
        return abilities[0].GetCooldownTime();
    }
}
