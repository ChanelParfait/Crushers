using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarStats : MonoBehaviour
{
    private Rigidbody rb;
    private PrometeoCarController carController;
    [Header("----------Stats-----------")]
    [SerializeField]public float score;
    [SerializeField]private float damage;
    [SerializeField]private float speed;
    [SerializeField]private Vector3 centreMass;
    [SerializeField] private GameObject lastCollidedPlayer;
    [SerializeField] private float lastCollisionTime;

    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private TextMeshProUGUI damageText;
    private float absoluteCarSpeed;


    // Start is called before the first frame update
    void Start()
    {
        lastCollisionTime = -1f;
        rb = GetComponent<Rigidbody>();
        centreMass = rb.centerOfMass;
        carController = GetComponent<PrometeoCarController>();
                

    }

    // Update is called once per frame
    void Update()
    {
        displayDamage();
        displayScore();
        
    }
    public float getSpeed(){
        return carController.GetCarSpeed();
    }

    public void addCentreOfMass(float damage){
        float increase = damage / 100f;
        centreMass.y += increase;
        if(centreMass.y > 2.0f){
            centreMass.y = 2.0f;
        }
        rb.centerOfMass = centreMass;
        
    }
    public void resetMass(){
        centreMass.y = 0f;
        rb.centerOfMass = centreMass;
    }

    public GameObject getLastCollided(){
        return lastCollidedPlayer;
    }
    public void setLastCollided(GameObject lastCollided){
        lastCollidedPlayer = lastCollided;
        lastCollisionTime = Time.time;
    }
    public float getLastCollisionTime(){
        return lastCollisionTime;
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
