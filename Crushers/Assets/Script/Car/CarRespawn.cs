using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarRespawn : MonoBehaviour
{
    //respawn Y threshold value 
    public float threshold;

    //Starting positions and rotation 
    private Transform respawn; 
    //private Vector3 startPosition;
    //private Quaternion startRotation;
    private Rigidbody rb;


    //setting up flip detection
    [SerializeField] private Collider flipCollider;


    //reference to the stats of each car
    private ImpactController impactController;
    
    
    void Start(){
        //save position and rotation for respawn
        respawn.position = transform.position;
        respawn.rotation = transform.rotation;
        rb = GetComponent<Rigidbody>();

        impactController = GetComponent<ImpactController>();
        //Debug.Log("Start Position: " + startPosition);
        
    }

    public void SetRespawn(Transform location){
        respawn = location;
    }
    
    public void Respawn(){
        // reset position, velocity and damage
        transform.position = respawn.position;
        transform.rotation = respawn.rotation;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if(impactController.GetLastCollidedVehicle() && impactController.GetLastCollidedVehicle() != this.gameObject.GetComponent<ScoreKeeper>()){
            impactController.GetLastCollidedVehicle().IncreaseScore(impactController.GetDeathType());
            //Debug.Log("Awarded 1 points to " + impactController.GetLastCollidedVehicle().gameObject.name);
        }
        
        impactController.ResetMass();
        //Debug.Log("Respawning");
        //Debug.Log("Your score is: " + carStats.getScore());
        //Debug.Log("Your damage is: " + carStats.getDamage());
    }

}
