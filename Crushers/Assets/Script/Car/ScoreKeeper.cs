using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    //NEED TO CREATE A SCORE MANAGER 
    [SerializeField] private float score;

    [Header("----------UI Elements-----------")]

    [SerializeField] private TextMeshProUGUI scoreText;


    // Update is called once per frame
    void Update()
    {
        DisplayScore();
    }

    public void IncreaseScore(float num){
        score = score + num;
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
    
    
}
