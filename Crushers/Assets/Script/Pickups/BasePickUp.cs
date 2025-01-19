using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BasePickUp : MonoBehaviour
{
    [SerializeField] private PickupType SelectedPU;

    public PickupType GetPickupType(){
        Debug.Log(SelectedPU);
        return SelectedPU;
    }
    /*private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Picked Up by" + other.gameObject.name);
            other.gameObject.transform.root.GetComponentInChildren<PickUpManager>().SetPickup(SelectedPU);
            Destroy(this.gameObject);
        }
    }*/
    
    
}
