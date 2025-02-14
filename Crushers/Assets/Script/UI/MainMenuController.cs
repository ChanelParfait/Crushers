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

    [SerializeField] private GameObject[] menuPanels;
    [SerializeField] private Button[] menuButtons;
    private int activeMenuIndex = 0;
    [SerializeField] private GameObject mainPnl; 
    [SerializeField] private GameObject levelSelectPnl; 
    [SerializeField] private Button level1Btn; 
    [SerializeField] private Button startBtn; 
    private int vehicleSelectSceneIndex = 1; 

    public static UnityAction<int> levelSelected; 
    private bool inMainMenu = true; 

    public void SwitchMenu()
    {
        inMainMenu = !inMainMenu;
        mainPnl.SetActive(inMainMenu);
        levelSelectPnl.SetActive(!inMainMenu);
        if(!inMainMenu){
            level1Btn.Select();
        } else {
            startBtn.Select();
        }
    }

    public void OpenPanel(int index)
    {   
        // disable currently open menu panel
        menuPanels[activeMenuIndex].SetActive(false);
        // enable new menu panel and set active index
        activeMenuIndex = index; 
        menuPanels[activeMenuIndex].SetActive(true);
        // select default menu button
        if(menuButtons[activeMenuIndex])
        {
            menuButtons[activeMenuIndex].Select();
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
        SceneManager.LoadSceneAsync(vehicleSelectSceneIndex);
    }

}
