using UnityEngine;
using UnityEngine.UI;

public class GearButton : MonoBehaviour
{
    [Header("Assigned Gear Scriptable Object")]
    public GearDisplaySO gear; // Reference to the GearDisplaySO

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClicked);
        }
    }

    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(OnButtonClicked);
        }
    }

    private void OnButtonClicked()
    {
        // Notify GearSelectionManager about this gear selection
        GearSelectionManager.Instance.SelectGear(gear);
    }
}