using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class SetupMenuController : MonoBehaviour
{
private int playerIndex;

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

    public void SetColour(Material colour){

        if(!inputEnabled){ return; }

        //PlayerManager.Instance.SetPlayerColour(playerIndex, colour);
        readyPnl.SetActive(true);
        menuPnl.SetActive(false);
        readyBtn.Select();
    }

    public void ReadyPlayer(){
        if(!inputEnabled){ return; }

        //PlayerManager.Instance.ReadyPlayer(playerIndex);
        readyBtn.gameObject.SetActive(false);
    }
}
