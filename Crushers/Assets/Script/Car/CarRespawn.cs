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
    
    void Respawn(){
        transform.position = startPosition;
            transform.rotation = startRotation;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;


            carStats.decreaseDamage(carStats.getDamage());
            carStats.decreaseScore(1);

            Debug.Log("Respawning");
            Debug.Log("Your score is: " + carStats.getScore());
            Debug.Log("Your damage is: " + carStats.getDamage());
    }
    
    void FixedUpdate()
    {
        //check if below threshold OR upside down and hitting the ground. 
        //still want to be able to flip even if not in contact with ground
        if(transform.position.y < threshold){
            
            
            
        }
        if (carStats.getDamage() < threshold)
        {
            Respawn();
        }
    
        

           
    }
    //check flipCollider hits something (ground normally)
    private void OnTriggerEnter(Collider other)
    {
        if(other != flipCollider)
        {
            if(other.gameObject.CompareTag("Ground") || other.gameObject.name == "RespawnCollider"){
                
                
                Respawn();
            }
        }
    }

}
