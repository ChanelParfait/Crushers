using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class SetupMenuController : MonoBehaviour
{
private int playerIndex;
public static UnityAction<int, GameObject> vehicleSelected; 
public static UnityAction<int> playerReady; 


     [SerializeField] private TextMeshProUGUI titleTxt; 
     [SerializeField] private GameObject readyPnl; 
     [SerializeField] private GameObject menuPnl; 
     [SerializeField] private Button readyBtn; 


     private float ignoreInputTime = 1.5f;
     private bool inputEnabled;


    public void SetPlayerIndex(int pi)
    {
        playerIndex = pi;
        titleTxt.SetText("Player" + (pi + 1).ToString());
        ignoreInputTime = Time.time + ignoreInputTime;
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

    public void SetVehicle(GameObject vehicle){

        if(!inputEnabled){ return; }

        readyPnl.SetActive(true);
        menuPnl.SetActive(false);
        readyBtn.Select();

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


    // create function to update canvas scale
}
