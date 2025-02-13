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

    //public UnityEvent<GameObject, GameObject, TypeOfDeath> OnPlayerScored ;
    
    public static UnityEvent<GameObject, GameObject, TypeOfDeath> OnGlobalPlayerScored = new UnityEvent<GameObject, GameObject, TypeOfDeath>();

    
    [Header("----------UI Elements-----------")]

    [SerializeField] private TextMeshProUGUI scoreText;
    

    // Update is called once per frame
    void Update()
    {
        DisplayScore();
    }

    public void IncreaseScore(TypeOfDeath DeathType)
    {
        //New IncreaseScore is based off DeathType and can be increase/decrease.
        
        //OnPlayerScored.Invoke(this.gameObject, GetComponent<ImpactController>().GetLastCollidedVehicle().gameObject, DeathType);
        
        OnGlobalPlayerScored.Invoke(this.gameObject, GetComponent<ImpactController>().GetLastCollidedVehicle().gameObject, DeathType);
        int num = 1;
        switch (DeathType)
        {
            case TypeOfDeath.Flip:
                //num = ?
                score = score + num;
                break;
            case TypeOfDeath.Spike:
                //num = ?
                score = score + num;
                break;
            case TypeOfDeath.Explosion:
                //num = ?
                score = score + num;
                break;
            case TypeOfDeath.Rocket:
                //num = ?
                score = score + num;
                break;
            case TypeOfDeath.Void:
                //num = ?
                score = score + num;
                break;
        }
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
        
        Debug.Log(scoringPlayerLayerName + " Has Scored Against " + defeatedPlayerLayerName + " By " + Death.ToString());
    }
}
