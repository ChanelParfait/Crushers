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
    private float flipProtectionTime = 5f;

    void Start(){
        //save position and rotation for respawn
        startPosition = transform.position;
        startRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();

        carStats = GetComponent<CarStats>();
    }
    
    public void Respawn(){
        transform.position = startPosition;
        transform.rotation = startRotation;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;


            carStats.decreaseDamage(carStats.getDamage());

            if(Time.time - carStats.getLastCollisionTime() > flipProtectionTime){
                carStats.decreaseScore(1);
                
            }
            else{
                GameObject lastCollidedPlayer = carStats.getLastCollided();
                if(lastCollidedPlayer == null) return;
                CarStats otherCarStats = lastCollidedPlayer.GetComponent<CarStats>();
                if(otherCarStats == null) return;

                otherCarStats.increaseScore(1);
                //Debug.Log("Awarded 1 points to " + lastCollidedPlayer.name);
            }
            
            carStats.resetMass();
            //Debug.Log("Respawning");
            //Debug.Log("Your score is: " + carStats.getScore());
            //Debug.Log("Your damage is: " + carStats.getDamage());
    }
    
    void FixedUpdate()
    {
        //check if below threshold OR upside down and hitting the ground. 
        //still want to be able to flip even if not in contact with ground

        if (carStats.getDamage() < threshold)
        {
            Respawn();
        }
    }

}
