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
                var carRespawn = other.gameObject.GetComponentInParent<CarRespawn>();
                carRespawn.Respawn();
            }
            
        }
    }
}
