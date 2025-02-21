using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BasePickUp : MonoBehaviour
{
    [SerializeField] private PickupType SelectedPU;
    
    // Event triggered when the pickup is collected
    public event Action OnPickupCollected;

    public PickupType GetPickupType(){
        Debug.Log(SelectedPU);
        return SelectedPU;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Triggered");
            OnPickupCollected?.Invoke();
            Destroy(gameObject);
        }
    }
    
    
}
