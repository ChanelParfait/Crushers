using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float _Speed = 1000f;
    [SerializeField] private GameObject FiredBy;

    
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
       
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject != FiredBy)
            {
                Debug.Log(other.gameObject);
                ExplosionDamage(this.transform.position, 5f);
                Destroy(this.gameObject);
            }
           
        }
        
    }

    private void ExplosionDamage(Vector3 postion, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(postion, radius);
        foreach (var hitcollider in hitColliders)
        {
            if (hitcollider.transform.root.gameObject.CompareTag("Player"))
            {
                if (hitcollider.transform.root.gameObject.GetComponent<PickUpManager>().State != Shield.IsOn)
                {
                    //hitcollider.transform.root.GetComponent<Rigidbody>().AddForce(hitcollider.transform.root.up  * 100000f, ForceMode.Force);
                    hitcollider.transform.root.GetComponent<Rigidbody>().AddExplosionForce(100000, gameObject.transform.position, radius, 10, ForceMode.Force);

                }
                
            }
        }
    }
}
