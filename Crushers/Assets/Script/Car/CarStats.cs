using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarStats : MonoBehaviour
{
    [Header("----------Stats-----------")]
    [SerializeField]public float score;
    [SerializeField]private float damage;
    [SerializeField]private float speed;

    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private TextMeshProUGUI damageText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        displayDamage();
        displayScore();
        
    }
    public float getSpeed(){
        return speed;
    }

   
    public void increaseDamage(float newDamage){
        damage = damage + newDamage;
        //can respawn if flipped after period of time
    }
    public float getDamage(){
        return damage;
    }
    public void decreaseDamage(float newDamage){
        damage = damage - newDamage;
    }

    public void increaseScore(float num){
        score = score + num;
    }
    public void decreaseScore(float num){
        score = score - num;
    }
    public float getScore(){
        return score;
    }

    public void displayScore()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString("");  
        }
    }

    public void displayDamage()
    {
        if (damageText != null)
        {
            damageText.text = "Damage: " + damage.ToString(""); 
        }
    }
}