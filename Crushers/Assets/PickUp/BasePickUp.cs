using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BasePickUp : MonoBehaviour
{
    [SerializeField] private PickupType SelectedPU;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TESt");
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.root.GetComponentInChildren<PickUpManager>().SetPickup(SelectedPU);
            Destroy(this.gameObject);
        }
        
        
    }
    
    
}
