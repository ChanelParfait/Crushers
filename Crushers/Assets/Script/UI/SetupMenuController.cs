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
private int playerIndex;
public static UnityAction<int, GameObject> vehicleSelected; 
public static UnityAction<int> playerReady;

     //List of vehicles to choose from 
     [SerializeField] private GameObject[] vehicleList;
     private int currentVehicleIndex = 0;

     [SerializeField] private TextMeshProUGUI titleTxt; 
     [SerializeField] private GameObject readyPnl; 
     [SerializeField] private GameObject menuPnl; 
     [SerializeField] private Button readyBtn;
     [SerializeField] private GameObject nextBtn;
     [SerializeField] private GameObject prevBtn;

     private Button currentBtn;


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
        //nextBtn.GetComponent<Button>().Select();

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
            //prevBtn.GetComponent<Button>().onClick.Invoke();
        
            PreviousVehicle();
        }

    }

    public void OnRight(CallbackContext context)
    {
        if(!selectionEnabled){ return; }
        if(context.performed){
            //nextBtn.GetComponent<Button>().onClick.Invoke();

            NextVehicle();
        }

    }

    public void OnEnter(CallbackContext context){
        if(!inputEnabled){ return; }
        if(currentBtn && context.performed){
            currentBtn.interactable = inputEnabled; 
            Debug.Log("Enter: " + currentBtn);
            currentBtn.onClick.Invoke();
        }
    }

    public void Click(string st){
        Debug.Log("Click: " + st);
    }

    //Show next vehicle
    public void PreviousVehicle()
    {
        Debug.Log("PREV Vehicle " + currentVehicleIndex);

        

        vehicleList[currentVehicleIndex].SetActive(false);

        currentVehicleIndex = (currentVehicleIndex - 1 + vehicleList.Length) % vehicleList.Length;
        Debug.Log("New vehicle " + currentVehicleIndex);


        UpdateVehicleDisplay();
    }

    //Show previous vehicle
    public void NextVehicle()
    {
        
        Debug.Log("NXT Vehicle " + currentVehicleIndex);


        vehicleList[currentVehicleIndex].SetActive(false);

        currentVehicleIndex = (currentVehicleIndex + 1) % vehicleList.Length;
        
         Debug.Log("New vehicle " + currentVehicleIndex);
        UpdateVehicleDisplay();
    }

    public void SetVehicle(GameObject vehicle){

        if(!inputEnabled){ return; }
        Debug.Log("Set Vehicle: " + vehicle);
        // display ready menu
        menuPnl.SetActive(false);
        readyPnl.SetActive(true);
        // invoke vehicle selected event
        vehicleSelected?.Invoke(playerIndex, vehicle);
        // erase currend button
        currentBtn = null;
        // disable selection and reset input delay
        selectionEnabled = false;
        inputEnabled = false;
        ignoreInputTime = Time.time + 0.5f;
        // select ready button
        readyBtn.Select();


    }

    public void Back(){
        if(!inputEnabled){ return; }
        Debug.Log("Back");
        readyPnl.SetActive(false);
        menuPnl.SetActive(true);
        
        UpdateVehicleDisplay();
        selectionEnabled = true;
        inputEnabled = false;
        ignoreInputTime = Time.time + 0.5f;
        currentBtn.Select();

    }

    public void ReadyPlayer(){
        Debug.Log("Ready");
        if(!inputEnabled){ return; }
        readyBtn.gameObject.SetActive(false);

        playerReady?.Invoke(playerIndex);

    }


    private void UpdateVehicleDisplay()
    {
        // display selected vehicle
        Debug.Log("Vehicle: " + vehicleList[currentVehicleIndex]);
        vehicleList[currentVehicleIndex].SetActive(true);
        currentBtn =  vehicleList[currentVehicleIndex].GetComponent<Button>();
    }
}
