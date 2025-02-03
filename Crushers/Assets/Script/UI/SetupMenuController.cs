using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class SetupMenuController : MonoBehaviour
{   
    // player index and event system
    private int playerIndex;
    private MultiplayerEventSystem eventSystem;

    // Events //
    public static UnityAction<int, GameObject> vehicleSelected; 
    public static UnityAction<int> playerReady;
    public static SetupMenuController instance;
    
     //List of vehicles to choose from
     [SerializeField] private GameObject[] vehicleList;
     private int currentVehicleIndex = 0;
     
     //Gears
     private GameObject currentPreview;
     private List<Gear> equippedGears = new List<Gear>();
     private List<GameObject> currentPreviews = new List<GameObject>(); // Stores instantiated gear models

     
     /// UI Gameobjects //
    [SerializeField] private TextMeshProUGUI titleTxt; 
    [SerializeField] private GameObject readyPnl; 
    [SerializeField] private GameObject menuPnl; 
    [SerializeField] private Button readyBtn;
    
     private float ignoreInputTime = 0.1f;
     private bool inputEnabled;
     private bool selectionEnabled;

     private void Awake()
     {
         instance = this;
     }

     void Start()
    {
        /// initialise controls and enable them 
        InputActionAsset controls = GetComponentInParent<PlayerInput>().actions;
        InputActionMap UI = controls.FindActionMap("UI");
        UI.Enable();
   
        // find Player Index
        playerIndex = GetComponentInParent<PlayerInput>().playerIndex; 
        titleTxt.SetText("Player" + (playerIndex + 1).ToString());
        ignoreInputTime = Time.time + ignoreInputTime;
        
        eventSystem = GetComponentInChildren<MultiplayerEventSystem>();

        selectionEnabled = true;
        UpdateVehicleDisplay();
        FindAttachmentPoints();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > ignoreInputTime){
            inputEnabled = true;
        }
    }

    // UI Navigation for setup menu
    public void OnLeft(CallbackContext context)
    {
        if(!selectionEnabled){ return; }
        if(context.performed){
            // select previous vehicle
            vehicleList[currentVehicleIndex].SetActive(false);
            currentVehicleIndex = (currentVehicleIndex - 1 + vehicleList.Length) % vehicleList.Length;
            UpdateVehicleDisplay();
        }

    }

    public void OnRight(CallbackContext context)
    {
        if(!selectionEnabled){ return; }
        if(context.performed){
            // select next vehicle
            vehicleList[currentVehicleIndex].SetActive(false);
            currentVehicleIndex = (currentVehicleIndex + 1) % vehicleList.Length;
            UpdateVehicleDisplay();
        }

    }

    public void Click(AudioSource buttonAudio){
        //Debug.Log("Click: " + st);
        buttonAudio.Play();
    }

    public void SetVehicle(GameObject vehicle){
        if(!inputEnabled){ return; }

        // display ready menu
        menuPnl.SetActive(false);
        readyPnl.SetActive(true);
        // invoke vehicle selected event
        vehicleSelected?.Invoke(playerIndex, vehicle);
        // disable selection and reset input delay
        selectionEnabled = false;
        // select ready button
        Select(readyBtn.gameObject);
    }

    public void Back(){
        if(!inputEnabled){ return; }
        // display selection menu
        readyPnl.SetActive(false);
        menuPnl.SetActive(true);
        // enable selection
        selectionEnabled = true;
        UpdateVehicleDisplay();
    }

    public void ReadyPlayer(){
        if(!inputEnabled){ return; }
        // hide ready button
        readyBtn.gameObject.SetActive(false);
        // ready up player
        playerReady?.Invoke(playerIndex);
    }


    private void UpdateVehicleDisplay()
    {
        // Destroy previous gear previews safely
        for (int i = currentPreviews.Count - 1; i >= 0; i--)
        {
            Destroy(currentPreviews[i]);
        }
        currentPreviews.Clear(); // Clear after destroying

        // Deactivate all vehicles
        foreach (var vehicle in vehicleList)
        {
            vehicle.SetActive(false);
        }

        // Activate the newly selected vehicle
        vehicleList[currentVehicleIndex].SetActive(true);
        Select(vehicleList[currentVehicleIndex]);

        // Reapply all previously equipped gears safely
        List<Gear> gearsToReapply = new List<Gear>(equippedGears); // Make a copy to avoid modifying while iterating
        equippedGears.Clear(); // Clear old list so we only add valid ones

        foreach (var gear in gearsToReapply)
        {
            UpdateGearPreview(gear);
        }
    }




    private void Select(GameObject button)
    {
        if (gameObject.GetComponentInParent<SetupMenuController>().playerIndex == playerIndex)
        {
            eventSystem.SetSelectedGameObject(button);
        }
    }

    private void FindAttachmentPoints()
    {
        Debug.Log(vehicleList[currentVehicleIndex].transform.Find("TAXI"));
    }

    public void UpdateGearPreview(Gear gear)
    {
        if (gear == null || gear.Model == null)
        {
            Debug.LogError("Invalid gear provided.");
            return;
        }

        // Remove any previously attached version of this gear type
        RemoveGearPreview(gear.GearID);

        // Store the equipped gear
        equippedGears.Add(gear);

        // Get the currently active vehicle
        GameObject activeVehicle = vehicleList[currentVehicleIndex];

        // Find the first child of the vehicle (the unique child before AttachmentsPos)
        if (activeVehicle.transform.childCount == 0)
        {
            Debug.LogError("No child found in " + activeVehicle.name);
            return;
        }

        Transform vehicleRoot = activeVehicle.transform.GetChild(0); // First unique child
        Transform attachmentParent = vehicleRoot.Find("AttachmentsPos");

        if (attachmentParent == null)
        {
            Debug.LogError("AttachmentsPos not found in " + vehicleRoot.name);
            return;
        }

        // Find the specific attachment point for the gear
        Transform attachmentPoint = attachmentParent.Find(gear.GearID);

        if (attachmentPoint == null)
        {
            Debug.LogError("Attachment point not found for Gear ID: " + gear.GearID + " in " + vehicleRoot.name);
            return;
        }

        // Instantiate and store the preview
        GameObject newPreview = Instantiate(gear.Model, attachmentPoint);
        newPreview.transform.SetParent(attachmentPoint);
        currentPreviews.Add(newPreview);
    }


    private void RemoveGearPreview(string gearID)
    {
        for (int i = equippedGears.Count - 1; i >= 0; i--) // Iterate backwards
        {
            if (equippedGears[i].GearID == gearID)
            {
                // Ensure index exists in both lists
                if (i < currentPreviews.Count)
                {
                    Destroy(currentPreviews[i]); // Destroy the instantiated preview
                    currentPreviews.RemoveAt(i); // Remove from the preview list
                }

                equippedGears.RemoveAt(i); // Remove from the equipped gear list
            }
        }
    }



}
