using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRespawn : MonoBehaviour
{
    public float threshold;
    [SerializeField]public float flipThreshold = -1f;

    //Starting X,Y,Z positions 
    private Vector3 startPosition;
    private Quaternion startRotation;
    [SerializeField] private Collider flipCollider;

    [SerializeField] private bool isFlipped;
    

    void Start(){
        startPosition = transform.position;
        startRotation = transform.rotation;
        isFlipped = false;
        
    }
    
    
    void FixedUpdate()
    {
        if(transform.position.y < threshold || (IsUpsideDown() && isFlipped)){
            transform.position = startPosition;
            transform.rotation = startRotation;
            Debug.Log("Respawning");
            isFlipped = false;
            
        }

        

           
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other != flipCollider)
        {
            isFlipped = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
       if(collision.collider != flipCollider)
        {
            isFlipped = true;
        }
    }

    bool IsUpsideDown()
    {
        return Vector3.Dot(transform.up, Vector3.up) < flipThreshold;
        
    }
}
