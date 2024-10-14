using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    private GameObject carOnPlatform;
    


    void Start()
    {
        carOnPlatform = this.gameObject;

    }

    void Update()
    {
        RotateCar();
        
    }

    public void RotateCar() {
        carOnPlatform.transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
