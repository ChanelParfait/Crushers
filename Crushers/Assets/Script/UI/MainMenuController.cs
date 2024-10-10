using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    
     [SerializeField] private GameObject mainPnl; 
     [SerializeField] private GameObject levelSelectPnl; 
     [SerializeField] private Button level1Btn; 
     [SerializeField] private Button startBtn; 
     private int VehicleMenuIndex = 1; 

     public static UnityAction<int> levelSelected; 
    private bool inMainMenu = true; 

    public void SwitchMenu(){
        inMainMenu = !inMainMenu;
        mainPnl.SetActive(inMainMenu);
        levelSelectPnl.SetActive(!inMainMenu);
        if(!inMainMenu){
            level1Btn.Select();
        } else {
            startBtn.Select();

        }
    }

    // Create a UI Controller base class with a generic click function 
    public void Click(AudioSource buttonAudio){
        //Debug.Log("Click: " + st);
        buttonAudio.Play();
    }

    public void QuitGame(){
        Application.Quit();
    }

    public void SelectLevel(int buildindex){
        // inform the player manager of the level selection made
        if(levelSelected != null){
            levelSelected.Invoke(buildindex);
        }
        // load vehicle selection menu 
        SceneManager.LoadScene(VehicleMenuIndex);
    }

}
