using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollider : MonoBehaviour
{
    private GameObject Player;

    public void SetPlayer(GameObject player)
    {
        Player = player;
    }
    
    

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.root.gameObject != Player && other.gameObject.CompareTag("Player"))
        {
            other.transform.root.GetComponent<Rigidbody>().AddExplosionForce(100000, gameObject.transform.position, 10f, 10, ForceMode.Force);
        }
    }
    
   
}
