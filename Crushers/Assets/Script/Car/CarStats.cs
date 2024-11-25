using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum VehicleType{
    Standard,
    Small,
    Big,
    Police,
    Unknown
}

public class CarStats : MonoBehaviour
{
    private VehicleType vehicleType;

    private Rigidbody rb;

    private PrometeoCarController carController;


    //NEED TO CREATE A SCORE MANAGER 
    [SerializeField] private float score;


    [Header("----------Stats-----------")]

    [SerializeField] private float speed;

    [SerializeField] private Vector3 centreMass;


    [Header("----------UI Elements-----------")]

    [SerializeField] private TextMeshProUGUI scoreText;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        centreMass = rb.centerOfMass;
        carController = GetComponent<PrometeoCarController>();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayScore();
    }
    public void SetVehicleType(VehicleType type)
    {
        vehicleType = type;
        switch (vehicleType)
        {

            case VehicleType.Standard:
                break;
            case VehicleType.Small:
                break;
            case VehicleType.Big:
                break;
            case VehicleType.Police:
                break;
            case VehicleType.Unknown:
                Debug.LogWarning("Vehicle type unkown, setting as default");
                break;
        }
    }

    public float GetSpeed(){
        return carController.GetCarSpeed();
    }

    public void AddCentreOfMass(float damage){
        float increase = damage / 100f;
        centreMass.y += increase;
        if(centreMass.y > 3.0f){
            centreMass.y = 3.0f;
        }
        rb.centerOfMass = centreMass;
        
    }
    public void ResetMass(){
        centreMass.y = 0f;
        rb.centerOfMass = centreMass;
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
