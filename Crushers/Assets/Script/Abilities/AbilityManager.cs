using System;
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


    [SerializeField] private Image abilityCanvas; // HUD canvas
    [SerializeField] private Color selectedColor = Color.yellow; // Color for selected ability
    [SerializeField] private Color defaultColor = Color.white; 
    
    private int selectedIndex = 0; // Tracks the currently selected ability
    private GameObject controlledCar; // The player's car
    private List<bool> cooldowns; // Cooldowns for each ability
    private List<Image> abilityImages;

    private void Start()
    {
        if (abilities.Count == 0) return;
        
        cooldowns = new List<bool>();
        foreach (var ability in abilities)
        {
            cooldowns.Add(true);
        }

        abilityImages = new List<Image>(abilityCanvas.GetComponentsInChildren<Image>());
        UpdateAllAbilitySprites();
        controlledCar = gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchAbility(-1);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchAbility(1);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            UseAbility();
        }
    }

    public void UseAbility()
    {
        if (canUse && cooldowns[selectedIndex])
        {
            if (abilities != null && abilities.Count > 0)
            {
                AbilityBase ability = abilities[selectedIndex];
                OnAbilityUse.Invoke(ability.GetCooldownTime());
                ability.Use(controlledCar);
                StartCooldown(selectedIndex, ability.GetCooldownTime());
            }
        }
    }

    private void StartCooldown(int index, float cooldownTime)
    {
        StartCoroutine(CooldownRoutine(index, cooldownTime));
    }

    private IEnumerator CooldownRoutine(int index, float cooldownTime)
    {
        cooldowns[index] = false;
        yield return new WaitForSeconds(cooldownTime);
        cooldowns[index] = true;
    }

    private void SwitchAbility(int direction)
    {
        if (abilities.Count == 0) return;
        selectedIndex = (selectedIndex + direction + abilities.Count) % abilities.Count;
        UpdateAllAbilitySprites();

        Debug.Log($"Switched to ability: {abilities[selectedIndex].title}");
    }

    private void UpdateAllAbilitySprites()
    {
        for (int i = 0; i < abilityImages.Count; i++)
        {
            if (i < abilities.Count)
            {
                abilityImages[i].sprite = abilities[i].icon;
                abilityImages[i].color = (i == selectedIndex) ? selectedColor : defaultColor; // Highlight selected
            }
            else
            {
                abilityImages[i].gameObject.SetActive(false);
            }
        }
    }
}
