using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRespawn : MonoBehaviour
{
    public float threshold;

    //Starting X,Y,Z positions 
    private float startX;
    private float startY;
    private float startZ;

    void Start(){
        startX = transform.position.x;
        startY = transform.position.y;
        startZ = transform.position.z;
    }
    
    void FixedUpdate()
    {
        if(transform.position.y < threshold){
            transform.position = new Vector3(startX, startY, startZ);     
            Debug.Log("Respawning");
        }

           
    }
}
