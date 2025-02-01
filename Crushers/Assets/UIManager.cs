using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Dictionary<Button, Image> gearSlotImages = new Dictionary<Button, Image>();

    public Button slot1Button, slot2Button, slot3Button;
    public Image slot1Image, slot2Image, slot3Image;
    
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        RegisterGearSlot(slot1Button, slot1Image);
        RegisterGearSlot(slot2Button, slot2Image);
        RegisterGearSlot(slot3Button, slot3Image);
    }

    public void RegisterGearSlot(Button gearSlotButton, Image gearSlotImage)
    {
        if (!gearSlotImages.ContainsKey(gearSlotButton))
        {
            gearSlotImages.Add(gearSlotButton, gearSlotImage);
        }
    }


    public void UpdateSelectedGearUI(Gear gear, Button button)
    {
        if (gearSlotImages.ContainsKey(button) && gear.GearIcon != null)
        {
            gearSlotImages[button].sprite = gear.GearIcon;
        }
    }

} 
