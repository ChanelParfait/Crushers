using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Generic : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {


        if (other.CompareTag("Player"))
        {
            //Debug.Log("Trap Triggered ");
            
            if (other.gameObject.GetComponentInParent<PickUpManager>().State == Shield.IsOff)
            {
                //Debug.Log("Other Obj: " + other.gameObject.GetComponentInParent<CarStats>().gameObject);
                
                // Find CarStats in Parent objects
                var carStats = other.gameObject.GetComponentInParent<CarStats>();
            
                // with how the car is designed + this code check, this deals 3x DMG
                carStats.IncreaseDamage(-100);
            }
            
        }
    }
}
