using System;
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
    [SerializeField]private float damageTaken;
    [SerializeField]private float speed;
    [SerializeField]private Vector3 centreMass;
    [SerializeField] private CarStats lastCollidedVehicle;
    //[SerializeField] private float lastCollisionTime;

    [Header("----------UI Elements-----------")]

    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private TextMeshProUGUI damageText;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        centreMass = rb.centerOfMass;
        carController = GetComponent<PrometeoCarController>();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayDamage();
        DisplayScore();
    }
    public float GetSpeed(){
        return carController.GetCarSpeed();
    }

    public void AddCentreOfMass(float damage){
        float increase = damage / 100f;
        centreMass.y += increase;
        if(centreMass.y > 2.0f){
            centreMass.y = 2.0f;
        }
        rb.centerOfMass = centreMass;
        
    }
    public void ResetMass(){
        centreMass.y = 0f;
        rb.centerOfMass = centreMass;
    }

    public CarStats GetLastCollidedVehicle(){
        return lastCollidedVehicle;
    }
    public void SetLastCollidedVehicle(CarStats lastCollided){
        Debug.Log("Set last Collided");
        StopCoroutine(ClearLastCollided(5f));
        lastCollidedVehicle = lastCollided;
        // Start coroutine to clear the last collided player after 5 seconds
        StartCoroutine(ClearLastCollided(5f));
    }

    private IEnumerator ClearLastCollided(float delay)
    {
        yield return new WaitForSeconds(delay);
        lastCollidedVehicle = null;
        Debug.Log("Cleared last collided player");
    }

    public void IncreaseDamage(float newDamage){
        damageTaken = damageTaken + newDamage;
        //can respawn if flipped after period of time
    }
    public float GetDamage(){
        return damageTaken;
    }
    public void DecreaseDamage(float newDamage){
        damageTaken = damageTaken - newDamage;
    }

    public void IncreaseScore(float num){
        score = score + num;
    }
    public void DecreaseScore(float num){
        score = score - num;
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

    private void DisplayDamage()
    {
        if (damageText != null)
        {
            damageText.text = "Damage: " + damageTaken.ToString(""); 
        }
    }
}
