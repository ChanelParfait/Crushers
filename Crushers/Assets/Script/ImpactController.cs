using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactController : MonoBehaviour
{
    public float forceMagnitude = 1000f; // Adjust this value to control the force applied

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object collided with has a specific tag or component to identify the fragile collider
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the Rigidbody of the other object (assuming it has one)
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Calculate a force vector to flip the object over
                Vector3 forceDirection = transform.up + collision.contacts[0].normal;
                rb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
            }
        }
    }
}
