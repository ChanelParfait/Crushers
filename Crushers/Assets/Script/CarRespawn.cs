using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRespawn : MonoBehaviour
{
    //respawn Y threshold value 
    public float threshold;
    //detects if player is upside down (-1 is upside down, 1 is right side up)
    [SerializeField]public float flipThreshold = -1f;

    //Starting positions and rotation 
    private Vector3 startPosition;
    private Quaternion startRotation;

    //setting up flip detection
    [SerializeField] private Collider flipCollider;

    [SerializeField] private bool isFlipped;

    //reference to the stats of each car
    public CarStats carStats;
    

    void Start(){
        //save position and rotation for respawn
        startPosition = transform.position;
        startRotation = transform.rotation;
        isFlipped = false;
        carStats = GetComponent<CarStats>();
        Debug.Log(carStats.getScore());
    }
    
    
    void FixedUpdate()
    {
        //check if below threshold OR upside down and hitting the ground. 
        //still want to be able to flip even if not in contact with ground
        if(transform.position.y < threshold || (IsUpsideDown() && isFlipped)){
            transform.position = startPosition;
            transform.rotation = startRotation;


            carStats.decreaseDamage(carStats.getDamage());
            carStats.decreaseScore(1);

            Debug.Log("Respawning");
            Debug.Log("Your score is: " + carStats.getScore());
            Debug.Log("Your damage is: " + carStats.getDamage());
            isFlipped = false;
            
        }
    
        

           
    }
    //check flipCollider hits something (ground normally)
    private void OnTriggerEnter(Collider other)
    {
        if(other != flipCollider)
        {
            isFlipped = true;
        }
    }

    //check flipCollider hits something (ground normally)

    private void OnCollisionEnter(Collision collision)
    {
       if(collision.collider != flipCollider)
        {
            isFlipped = true;
        }
    }

    //check if upside down
    bool IsUpsideDown()
    {
        return Vector3.Dot(transform.up, Vector3.up) < flipThreshold;
        
    }
}
