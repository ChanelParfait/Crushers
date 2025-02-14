using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomGravity : MonoBehaviour
{
    private List<Rigidbody> rigidbodies;
    [SerializeField] private float range;
    [SerializeField] private float intensity;
    private float distanceToPlayer;
    private Vector2 pullForce;

    private void Awake()
    {
        range = GetComponent<SphereCollider>().radius;
    }
    void Update()
    {
        if (rigidbodies != null)
        {
            foreach (Rigidbody player in rigidbodies)
            {
                DoGravity(player);
            }
        }
    }

    private void DoGravity(Rigidbody player)
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= range)
        {
            pullForce = (transform.position - player.position).normalized / distanceToPlayer * intensity;
            player.AddForce(pullForce, ForceMode.Force);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // Debug.Log(other); particles are being collided with 
        if (other.gameObject.CompareTag("Player"))
        {
            rigidbodies.Add(other.gameObject.GetComponent<Rigidbody>());
        }
    }
}
