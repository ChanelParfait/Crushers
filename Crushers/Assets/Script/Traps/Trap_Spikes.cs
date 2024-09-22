using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Spikes : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.root.GetComponentInChildren<PickUpManager>().State == Shield.IsOff)
            {
                // Find CarStats in Parent objects
                var carStats = other.transform.root.GetComponentInChildren<CarStats>();
            
                // with how the car is designed + this code check, this deals 3x DMG
                carStats.increaseDamage(-25);
            }
            
        }
    }
}
