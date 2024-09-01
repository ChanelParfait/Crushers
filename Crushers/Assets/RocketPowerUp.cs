using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketPowerUp : MonoBehaviour
{
    private BoxCollider bC;
    private void Start()
    {
        bC = this.GetComponent<BoxCollider>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //other.getPickUp(Rocket);
            
            //;
            
            //Destroy(this);
        }
    }
}
