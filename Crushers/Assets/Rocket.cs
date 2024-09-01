using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Rigidbody rB;
    [SerializeField] private float _Speed = 10f;

    private void Awake()
    {
        rB = GetComponent<Rigidbody>();
        
    }
}
