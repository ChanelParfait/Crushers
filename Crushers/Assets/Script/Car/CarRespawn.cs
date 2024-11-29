using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms.Impl;

public class CarRespawn : MonoBehaviour
{
    //respawn Y threshold value 
    public float threshold;

    //Starting positions and rotation 
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Rigidbody rb;


    //setting up flip detection
    [SerializeField] private Collider flipCollider;


    //reference to the stats of each car
    private ImpactController impactController;
    private ScoreKeeper carStats;
    
    //event when Players respawn and score points
    public UnityEvent<GameObject, GameObject, TypeOfDeath> PlayerScored ;
    
    
    void Start(){
        //save position and rotation for respawn
        startPosition = transform.position;
        startRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();

        impactController = GetComponent<ImpactController>();
        carStats = GetComponent<ScoreKeeper>();
        
        
        PlayerScored.AddListener((scoringPlayer, defeatedPlayer, typeofdeath) =>
            GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>().TestInvoke(scoringPlayer, defeatedPlayer, typeofdeath));
    }
    
    public void Respawn(){
        // reset position, velocity and damage
        transform.position = startPosition;
        transform.rotation = startRotation;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if(impactController.GetLastCollidedVehicle()){
            impactController.GetLastCollidedVehicle().IncreaseScore(1);
            Debug.Log("Awarded 1 points to " + impactController.GetLastCollidedVehicle().gameObject.name);
            
            PlayerScored?.Invoke(impactController.GetLastCollidedVehicle().gameObject, this.gameObject, impactController.GetDeathType() );
        }
        
        impactController.ResetMass();
        //Debug.Log("Respawning");
        //Debug.Log("Your score is: " + carStats.getScore());
        //Debug.Log("Your damage is: " + carStats.getDamage());
    }
}
