
using UnityEngine;
using UnityEngine.UI;

public class GearSelectionManager : MonoBehaviour
{
    public Button[] gearButtons; // Array of gear slot buttons
    public Button[] itemButtons; // Array of item buttons to select images from
    private Button selectedGearButton = null; // Currently selected gear slot button
    
    public SetupMenuController setupMenuController;
    public string attachmentPoint;
    
    public static GearSelectionManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SelectGear(GearDisplaySO gear)
    {
        if (setupMenuController != null)
        {
            string attachmentPoint = gear.name; // Or another way to determine the attachment point
            setupMenuController.SelectGear(attachmentPoint, gear);
        }
        else
        {
            Debug.LogError("SetupMenuController is not assigned!");
        }
    }

    public void SelectGearSlot(Button gearButton)
    {
        selectedGearButton = gearButton; // Set the current gear slot button
    }

    public void OnItemButtonClicked(Button itemButton)
    {
        if (selectedGearButton != null)
        {
            // Get the GearDisplaySO from the item button
            GearDisplaySO selectedGear = itemButton.GetComponent<GearDisplaySO>();

            if (selectedGear != null)
            {
                // Notify SetupMenuController to attach the gear
                OnGearSelected(selectedGear);
            }

            // Clear the selection
            selectedGearButton = null;
        }
        else
        {
            Debug.Log("No gear slot selected!");
        }
    }

    private void OnGearSelected(GearDisplaySO gear)
    {
        if (setupMenuController != null && !string.IsNullOrEmpty(attachmentPoint))
        {
            // Pass the GearDisplaySO and attachment point to SetupMenuController
            setupMenuController.SelectGear(attachmentPoint, gear);
        }
        else
        {
            Debug.LogError("SetupMenuController or attachmentPoint is not assigned!");
        }
    }
}
