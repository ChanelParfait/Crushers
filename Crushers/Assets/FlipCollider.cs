using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlipCollider : MonoBehaviour
{
    // ideally this component should have access to the correct player index
    // and trigger this event when a respawn is required
    //public static UnityAction<int> RespawnTriggered; 

    [SerializeField] private CarRespawn respawn;
    private void OnTriggerEnter(Collider other)
    {

       // Debug.Log("Trigger");

        if (other.gameObject.CompareTag("Ground") || other.gameObject.name == "RespawnCollider")
        {
            //Debug.Log("Ground Hit");
            
            respawn.Respawn();
        }
    }
}
