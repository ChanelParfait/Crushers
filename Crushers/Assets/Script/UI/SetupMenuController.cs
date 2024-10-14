using ShapesFX;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
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


     private float ignoreInputTime = 1.5f;
     private bool inputEnabled;


/*    public void SetPlayerIndex(int pi)
    {
        playerIndex = pi;
        
    }*/

    void Start(){
        // find Player Index
        playerIndex = GetComponentInParent<PlayerInput>().playerIndex; 
        titleTxt.SetText("Player" + (playerIndex + 1).ToString());
        ignoreInputTime = Time.time + ignoreInputTime;

        UpdateVehicleDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > ignoreInputTime){
            inputEnabled = true;
        }
    }

    public void Click(string st){
        Debug.Log("Click: " + st);
    }

    //Show next vehicle
    public void PreviousVehicle()
    {
        if (!inputEnabled) return;

        vehicleList[currentVehicleIndex].SetActive(false);

        currentVehicleIndex = (currentVehicleIndex - 1 + vehicleList.Length) % vehicleList.Length;

        UpdateVehicleDisplay();
    }

    //Show previous vehicle
    public void NextVehicle()
    {
        if (!inputEnabled) return;

        vehicleList[currentVehicleIndex].SetActive(false);

        currentVehicleIndex = (currentVehicleIndex + 1) % vehicleList.Length;

        UpdateVehicleDisplay();
    }

    public void SetVehicle(GameObject vehicle){

        if(!inputEnabled){ return; }

        readyPnl.SetActive(true);
        menuPnl.SetActive(false);
        readyBtn.Select();
        nextBtn.SetActive(false);
        prevBtn.SetActive(false);


        if(vehicleSelected != null){
            vehicleSelected.Invoke(playerIndex, vehicle); 
        }
        
    }

    public void ReadyPlayer(){
        if(!inputEnabled){ return; }
        readyBtn.gameObject.SetActive(false);

        if(playerReady != null){
            playerReady.Invoke(playerIndex); 
        }

    }


    private void UpdateVehicleDisplay()
    {
        // display selected vehicle
        vehicleList[currentVehicleIndex].SetActive(true);
    }
}
