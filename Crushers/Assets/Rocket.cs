using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float _Speed = 1000f;

    
    private void Start()
    {
        this.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * _Speed, ForceMode.Force);
    }

    private void OnCollisionEnter(Collision other)
    {
        ExplosionDamage(this.transform.position, 20f);
        Destroy(this.gameObject);
    }

    private void ExplosionDamage(Vector3 postion, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(postion, radius);
        foreach (var hitcollider in hitColliders)
        {
            if (hitcollider.transform.root.gameObject.CompareTag("Player"))
            {
                Debug.Log(hitcollider.transform.root.gameObject);
                hitcollider.transform.root.GetComponent<Rigidbody>().AddForce(hitcollider.transform.root.up  * 500000f, ForceMode.Force);
                
            }
        }
    }
}
