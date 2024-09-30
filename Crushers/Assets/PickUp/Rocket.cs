using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float _Speed = 1000f;
    [SerializeField] private GameObject FiredBy;
    [SerializeField] private GameObject ExplosionVFX;

    
    private void Start()
    {
        this.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * _Speed, ForceMode.Force);
    }

    public void SetFiredBy(GameObject FiredFrom)
    {
        FiredBy = FiredFrom;
    }
    
    

    private void OnTriggerEnter(Collider other)
    {
        ExplosionDamage(this.transform.position, 10f);
        Destroy(this.gameObject);
            
    }

    private void OnDestroy()
    {
        
        
    }

    private void ExplosionDamage(Vector3 postion, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(postion, radius);
        foreach (var hitCollider in hitColliders)
        {
            
            /*
            if (hitcollider.transform.root.gameObject.CompareTag("Player"))
            {
                if (hitcollider.transform.root.gameObject.GetComponent<PickUpManager>().State != Shield.IsOn)
                {
                    //hitcollider.transform.root.GetComponent<Rigidbody>().AddForce(hitcollider.transform.root.up  * 100000f, ForceMode.Force);
                    hitcollider.transform.root.GetComponent<Rigidbody>().AddExplosionForce(100000, gameObject.transform.position, radius, 10, ForceMode.Force);

                }
                
            }
            */
            
            if (hitCollider.GetComponentInParent<PickUpManager>() != null && 
                hitCollider.GetComponentInParent<PickUpManager>().gameObject.CompareTag("Player"))
            {
                // Check if the player's shield is not active
                if (hitCollider.GetComponentInParent<PickUpManager>().State != Shield.IsOn)
                {
                    // Apply explosion force to the player
                    hitCollider.GetComponentInParent<Rigidbody>().AddExplosionForce(100000, gameObject.transform.position, radius, 10, ForceMode.Force);
                }
            }
            if (ExplosionVFX)
            {
                GameObject Explosion =  Instantiate(ExplosionVFX, this.gameObject.transform.position, this.gameObject.transform.rotation);
                Explosion.GetComponent<Explosion>().SetTimeBeforeDestruction(1);
            }
        }
    }
}
