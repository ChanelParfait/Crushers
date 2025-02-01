using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GearUIManager : MonoBehaviour
{
    public static GearUIManager instance;
    private Button selectedGearSlot;
    private Dictionary<Button, Gear> selectedGears = new Dictionary<Button, Gear>();

    void Awake()
    {
        instance = this;
    }

    public void SelectGearSlot(Button gearSlotButton)
    {
        selectedGearSlot = gearSlotButton;
    }

    public void SelectGear(Gear gear)
    {
        if (selectedGearSlot != null)
        {
            selectedGears[selectedGearSlot] = gear;
            Debug.Log("Selected Gear for " + selectedGearSlot.name + ": " + gear.Name);
            UIManager.instance.UpdateSelectedGearUI(gear, selectedGearSlot);
            SetupMenuController.instance.UpdateGearPreview(gear);
        }
    }

    public Gear GetSelectedGear(Button button)
    {
        return selectedGears.ContainsKey(button) ? selectedGears[button] : null;
    }
}
