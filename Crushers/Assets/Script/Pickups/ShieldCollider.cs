using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class ShieldCollider : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    private AudioSource audioSource;
    [SerializeField] private AudioClip bubble;
    [SerializeField] private AudioClip pop;
    void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
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
