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
    private bool inMainMenu = true; 

    public void SwitchMenu(){
        inMainMenu = !inMainMenu;
        mainPnl.SetActive(inMainMenu);
        levelSelectPnl.SetActive(!inMainMenu);
    }

    public void QuitGame(){
        Application.Quit();
    }

    public void LoadLevel(int buildindex){
        SceneManager.LoadScene(buildindex);
    }

}
