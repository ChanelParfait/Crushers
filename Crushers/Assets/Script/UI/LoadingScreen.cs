using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject panel; 
    [SerializeField] private Image progress; 
    
    public void DisplayScreen(){
        panel.SetActive(true);
    }

    public void UpdateProgress(float fill){

        progress.fillAmount =  fill;
    }
}
