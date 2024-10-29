using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pausePnl; 

    public void SetActive(bool isActive){
        pausePnl.SetActive(isActive);
    }

    
    public void Unpause(){
        SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void Exit(){
        Time.timeScale = 1.0f;
        PlayerManager pm = FindObjectOfType<PlayerManager>();
        if(pm){
            // remove player objects
            pm.DestroyConfigs();
            // destroy player input manager
            DestroyImmediate(pm.gameObject);
        }
        // load main menu
        SceneManager.LoadScene(0);
    }

}
