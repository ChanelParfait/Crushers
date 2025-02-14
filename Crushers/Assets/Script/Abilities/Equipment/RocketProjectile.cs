using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Collider = UnityEngine.Collider;
using Random = UnityEngine.Random;

public class RocketProjectile : MonoBehaviour
{
    [SerializeField] private GameObject ExplosionVFX;
    [SerializeField] private ScoreKeeper FiredBy;
    [SerializeField] private AudioClip[] explosionSFX;

    [SerializeField] private GameObject carcontroller;

    [SerializeField] private float _Speed;
    private float _HitRadius;
    
    public void Initialize(float speed, float hitRadius, GameObject FiredFrom ) {
        _Speed = speed;
        _HitRadius = hitRadius;
        FiredBy = FiredFrom.GetComponent<ScoreKeeper>();
        carcontroller = FiredFrom;
    }

    private void OnEnable()
    {
        /*Debug.Log("test1");
        Collider rocketCollider = this.gameObject.GetComponent<Collider>();
        Collider carCollider = carcontroller.gameObject.GetComponent<Collider>();

        if (carCollider != null && rocketCollider != null)
        {
            Debug.Log("test2");
            Physics.IgnoreCollision(rocketCollider, carCollider);
        }
            
        Collider[] carChildrenColliders = this.GetComponentsInChildren<Collider>();
        foreach (var childCollider in carChildrenColliders)
        {
            Physics.IgnoreCollision(rocketCollider, childCollider);
            Debug.Log("test3");
            Debug.Log(childCollider.gameObject.name + "Collider");
        }*/
    }

    private void Start()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * _Speed, ForceMode.Force);
    }
    public ScoreKeeper GetFiredBy()
    {
        return FiredBy;
    }

    private void OnTriggerEnter(Collider other)
    {
        ExplosionDamage(this.transform.position, _HitRadius);
        Debug.Log(other.name + " Exploded On");
        Destroy(this.gameObject);

    }

    private void OnDestroy()
    {
        // select random explosion SFX
        AudioClip activeClip = explosionSFX[Random.Range(0, explosionSFX.Length - 1)];
        GameObject listener = Camera.main.gameObject;
        AudioSource.PlayClipAtPoint(activeClip, listener.transform.position, 10);
        Debug.Log("Explode: " + listener);
    }

    private void ExplosionDamage(Vector3 postion, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(postion, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponentInParent<CarController>() != null &&
                hitCollider.GetComponentInParent<CarController>().gameObject.CompareTag("Player"))
            {
                // Check if the player's shield is not active
                if (hitCollider.GetComponentInParent<PickUpManager>().State != Shield.IsOn)
                {
                    // Apply explosion force to the player
                    hitCollider.GetComponentInParent<Rigidbody>().AddExplosionForce(100000, gameObject.transform.position, radius, 10, ForceMode.Force);
                    // set last collided vehicle of player
                    hitCollider.GetComponentInParent<ImpactController>().SetLastCollidedVehicle(FiredBy);
                    //set last collided vehicle death type to rocket
                    hitCollider.gameObject.GetComponentInParent<ImpactController>().SetDeathType(TypeOfDeath.Rocket);
                }
            }
            if (ExplosionVFX)
            {
                GameObject Explosion = Instantiate(ExplosionVFX, this.gameObject.transform.position, this.gameObject.transform.rotation);
                Explosion.GetComponent<Explosion>().SetTimeBeforeDestruction(1);
            }
        }
    }
}
