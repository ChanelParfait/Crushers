using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class ScoreKeeper : MonoBehaviour
{
    //NEED TO CREATE A SCORE MANAGER 
    [SerializeField] private float score;

    public UnityEvent<GameObject, GameObject, TypeOfDeath> OnPlayerScored ;
    
    [Header("----------UI Elements-----------")]

    [SerializeField] private TextMeshProUGUI scoreText;
    

    // Update is called once per frame
    void Update()
    {
        DisplayScore();
    }

    public void IncreaseScore(float num){
        score = score + num;
        OnPlayerScored.Invoke(this.gameObject, GetComponent<ImpactController>().GetLastCollidedVehicle().gameObject,GetComponent<ImpactController>().GetDeathType() );
    }

    public float GetScore(){
        return score;
    }

    private void DisplayScore()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString("");  
        }
    }

   

    public void TestInvoke(GameObject ScoringPlayer, GameObject DefeatedPlayer, TypeOfDeath Death)
    {
        //Debug.Log(ScoringPlayer.name + " Has Scored A Point Against " + DefeatedPlayer.name + " By " + Death.ToString());
        string scoringPlayerLayerName = LayerMask.LayerToName(ScoringPlayer.layer);
        string defeatedPlayerLayerName = LayerMask.LayerToName(DefeatedPlayer.layer);
        
        
        Debug.Log(scoringPlayerLayerName + " Has Scored A Point Against " + defeatedPlayerLayerName + " By " + Death.ToString());
    }
}
