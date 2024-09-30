using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollider : MonoBehaviour
{
    [SerializeField] private GameObject Player;

    public void SetPlayer(GameObject player)
    {
        Player = player;
    }
    
    

    
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody otherRB = other.gameObject.GetComponentInParent<Rigidbody>();
        if(otherRB){
            if (otherRB.gameObject != Player && other.gameObject.CompareTag("Player"))
            {
                
                otherRB.AddExplosionForce(100000, gameObject.transform.position, 10f, 10, ForceMode.Force);
            }
        }
    }
    
   
}
