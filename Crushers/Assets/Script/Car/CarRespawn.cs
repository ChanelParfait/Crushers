using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public CarStats carStats;

    void Start(){
        //save position and rotation for respawn
        startPosition = transform.position;
        startRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();

        carStats = GetComponent<CarStats>();
    }
    
    public void Respawn(){
        // reset position, velocity and damage
        transform.position = startPosition;
        transform.rotation = startRotation;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        carStats.DecreaseDamage(carStats.GetDamage());

        if(carStats.GetLastCollidedVehicle()){
            carStats.GetLastCollidedVehicle().IncreaseScore(1);
            Debug.Log("Awarded 1 points to " + carStats.GetLastCollidedVehicle().gameObject.name);
        } else {
            // decrease car score by 1 if it is above 0
            if(carStats.score > 0){
                carStats.DecreaseScore(1);
            }
        }
        
        carStats.ResetMass();
        //Debug.Log("Respawning");
        //Debug.Log("Your score is: " + carStats.getScore());
        //Debug.Log("Your damage is: " + carStats.getDamage());
    }
    
    void FixedUpdate()
    {
        //check if below threshold OR upside down and hitting the ground. 
        //still want to be able to flip even if not in contact with ground

        if (carStats.GetDamage() < threshold)
        {
            Respawn();
        }
    }

}
