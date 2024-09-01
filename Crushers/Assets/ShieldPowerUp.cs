using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : MonoBehaviour
{
    private BoxCollider bC;
    private void Start()
    {
        bC = this.GetComponent<BoxCollider>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //other.getPickUp(Shield);
            
            //other.setShield(true);
            
            //Destroy(this);
        }
    }
}
