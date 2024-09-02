using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SpeedPowerUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.CompareTag("Player"))
        {
            //other.getPickUp(Speed);
            
            other.transform.root.GetComponent<Rigidbody>().AddForce(other.transform.forward * 10000f, ForceMode.Impulse);
            
            
            //Debug.Log(other.transform.root.GetComponent<Rigidbody>().velocity);
            Destroy(this.gameObject);
        }
    }
    
}
