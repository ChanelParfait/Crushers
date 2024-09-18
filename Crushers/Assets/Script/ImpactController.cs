using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactController : MonoBehaviour
{
    private PrometeoCarController carController;
    private void Start()
    {
        carController = GetComponent<PrometeoCarController>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        float hitForce = carController.CalculateHitForce();

        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 forceDirection = transform.up + collision.contacts[0].normal;
                rb.AddForce(forceDirection * hitForce, ForceMode.Impulse);
            }
        }
    }
}
