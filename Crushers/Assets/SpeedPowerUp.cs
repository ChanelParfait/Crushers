using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SpeedPowerUp : MonoBehaviour
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
            //other.getPickUp(Speed);
            
            //other.setMovementSpeed(?);
            
            //Destroy(this);
        }
    }
}
