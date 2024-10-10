using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactController : MonoBehaviour
{
    private PrometeoCarController carController;

    private float hitTimer = 5f;
    private bool gotHit = false;

    private void Start()
    {
        carController = GetComponent<PrometeoCarController>();
    }

    private void Update()
    {
        // Timer that checks if player got hit
        // If yes, starts the countdown to 0, to reset gotHit
        if (gotHit)
        {
            hitTimer -= Time.deltaTime;
            if (hitTimer <= 0f)
            {
                hitTimer = 0f;
                gotHit = false;
                //Debug.Log("Got hit reset.");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Hit(collision);
        CheckForHit(collision);
    }

    // Checks if you hit the player
    // If yes, applies force and changes center of mass of the player you hit
    private void Hit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            float hitForce = carController.CalculateHitForce();
            hitForce = Mathf.Max(hitForce, 0f);

            float hitAmplitude = hitForce * 0.002f;
            //Debug.Log("hitForce: " + hitForce);
            CameraController.Instance.ShakeCamera(hitAmplitude, 1f, 1f);

            Rigidbody rb = collision.gameObject.GetComponentInParent<Rigidbody>();
            //Debug.Log("RB: " + rb.gameObject);

            if (rb != null)
            {
                Vector3 forceDirection = transform.forward;

                // Calculate new center of mass
                Vector3 newCenterOfMass = rb.centerOfMass + new Vector3(0, hitForce * 0.0001f, 0);
                newCenterOfMass.y = Mathf.Max(newCenterOfMass.y, 0f); 

                rb.centerOfMass = newCenterOfMass;
                //Debug.Log("New center of mass on Y: " + newCenterOfMass);
                rb.AddForce(forceDirection * hitForce, ForceMode.Impulse);
            }
        }
    }

    // Checks if you got hit
    // If yes, set gotHit to true 
    // The logic that resets center of mass of your car is in PrometeoCarController
    private void CheckForHit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gotHit = true;
            hitTimer = 5f;
        }
    }

    public bool GetGotHit()
    {
        return gotHit;
    }
}
