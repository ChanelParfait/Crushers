using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollider : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    private AudioSource audioSource;
    [SerializeField] private AudioClip bubble;
    [SerializeField] private AudioClip pop;

    public LayerMask groundLayer;
    
    void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Collider shieldCollider = GetComponent<Collider>();
        
        if (shieldCollider != null)
        {
            // Get all colliders on the ground layer
            
            Collider[] allColliders = FindObjectsOfType<Collider>();

            foreach (Collider col in allColliders)
            {
                // Check if the collider belongs to the ground layer using LayerMask
                if (groundLayer == (groundLayer | (1 << col.gameObject.layer)))
                {
                    Physics.IgnoreCollision(shieldCollider, col);
                }
            }

            // Ignore collisions with all player child colliders
            Collider[] playerColliders = Player.GetComponentsInChildren<Collider>();
            foreach (Collider playerCollider in playerColliders)
            {
                Physics.IgnoreCollision(shieldCollider, playerCollider);
            }
        }
        
    }

    void OnDisable()
    {

    }

    public void SetPlayer(GameObject player)
    {
        Player = player;
    }

    public void PlayAudio(int clipNum, int delay)
    {
        if(clipNum == 1){
            audioSource.clip = bubble;
        } else if(clipNum == 2){
            audioSource.clip = pop;
        }
        if(delay > 0){
            audioSource.PlayDelayed(delay);
        } else {
            audioSource.Play();
        }
    }
    
    
    private void OnCollisionEnter(Collision other)
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
