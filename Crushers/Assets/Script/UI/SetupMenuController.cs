using ShapesFX;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.Rendering;
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

     //List of vehicles to choose from
     [SerializeField] private GameObject[] vehicleList;
     private int currentVehicleIndex = 0;

     /// UI Gameobjects //
    [SerializeField] private TextMeshProUGUI titleTxt; 
    [SerializeField] private GameObject readyPnl; 
    [SerializeField] private GameObject menuPnl; 
    [SerializeField] private Button readyBtn;


     private float ignoreInputTime = 0.1f;
     private bool inputEnabled;
     private bool selectionEnabled;

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
        // display selected vehicle
        vehicleList[currentVehicleIndex].SetActive(true);
        Select(vehicleList[currentVehicleIndex]);
    }


    private void Select(GameObject button){
        if(gameObject.GetComponentInParent<SetupMenuController>().playerIndex == playerIndex){
            eventSystem.SetSelectedGameObject(button);
            Debug.Log("Player " + playerIndex + " Selected: " + eventSystem.currentSelectedGameObject); 
        }
        
    }
}
