using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private Rigidbody rB;
    [SerializeField] private float _Speed = 10f;

    private void Awake()
    {
        
    }

    private void Update()
    {
        rB.AddForce(transform.forward * _Speed, ForceMode.Force);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);
    }
}
