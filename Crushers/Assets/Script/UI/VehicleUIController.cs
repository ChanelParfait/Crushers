using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public class VehicleUIController : MonoBehaviour
{
   

    [SerializeField] private TextMeshProUGUI levelTimerTxt;
    [SerializeField] private GameObject startTimer;

    [SerializeField] private TextMeshProUGUI startTimerTxt;


       void OnEnable()
    {
        LevelManager.levelTimeChanged +=  UpdateLevelTimer;
        LevelManager.startTimeChanged +=  UpdateStartTimer;


    }

    void OnDisable()
    {
        LevelManager.levelTimeChanged -=  UpdateLevelTimer;
        LevelManager.startTimeChanged -=  UpdateStartTimer;

    }

    public void UpdateLevelTimer(int time){
        // convert time to minutes and seconds
        int minutes = time / 60;
        int seconds = time % 60;

        levelTimerTxt.SetText(minutes.ToString() + ":" + seconds.ToString());
    }

    public void UpdateStartTimer(int time){

        if(time == 0){
            // set text to "GO"
            startTimerTxt.SetText("GO!");
            startTimerTxt.color = Color.green;
            StartCoroutine(ClearStartTimer());
        } else {
            startTimerTxt.SetText(time.ToString());
        }
    }

    private IEnumerator ClearStartTimer(){
        yield return new WaitForSeconds(2);
        startTimer.SetActive(false);
    }


}
