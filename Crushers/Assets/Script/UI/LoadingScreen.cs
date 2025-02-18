using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject playPnl; 
    [SerializeField] private GameObject controlPnl; 
    [SerializeField] private Image loadingCircle;


    void Update()
    {
        //Debug.Log("Update");
        if(loadingCircle.IsActive())
        {
            loadingCircle.rectTransform.Rotate(0, 0, -1);
        }
       
    }


    public void DisplayScreen(int sceneIndex){
        //Debug.Log("Display Screen");

        playPnl.SetActive(true);
        loadingCircle.gameObject.SetActive(true);
        StartCoroutine(DelayScreen(3, sceneIndex));
    }

    
    private IEnumerator DelayScreen(int delay, int buildIndex)
    {
        //Debug.Log("Delay Screen");

        yield return new WaitForSeconds(delay);
        playPnl.SetActive(false);
        controlPnl.SetActive(true);
        yield return new WaitForSeconds(delay);
        SceneManager.LoadSceneAsync(buildIndex);
    }

    public void UpdateProgress(float fill){

       // progress.fillAmount =  fill;
    }
}
