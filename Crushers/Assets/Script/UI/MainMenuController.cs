using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    
     [SerializeField] private GameObject mainPnl; 
     [SerializeField] private GameObject levelSelectPnl; 
     [SerializeField] private Button level1Btn; 
     [SerializeField] private Button startBtn; 


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

    public void QuitGame(){
        Application.Quit();
    }

    public void LoadLevel(int buildindex){
        SceneManager.LoadScene(buildindex);
    }

}
