using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactController : MonoBehaviour
{
    [SerializeField] private float forceMagnitude = 2000f; 

    private void OnCollisionEnter(Collision collision)
    {
      
        if (collision.gameObject.CompareTag("Player"))
        {
           
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                
                Vector3 forceDirection = transform.up + collision.contacts[0].normal;
                rb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
            }
        }
    }
}
