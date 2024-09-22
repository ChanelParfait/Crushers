using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickUpManager : MonoBehaviour
{
    [SerializeField] private GameObject shieldGO; 
    [SerializeField] private GameObject Rocket;

    [SerializeField] private PickupType Pickup;

    [SerializeField] public Shield State;

    [SerializeField] private float ShieldTimer = 10;

    private GameObject shield; 
    public bool useItem = false;
    
    public void SetPickup(PickupType PickUpPowerup)
    {
        Pickup = PickUpPowerup;
    }
    
    
    private void Update()
    {
        if (useItem)
        {
            switch (Pickup)
            {
                case PickupType.Rocket:
                    UseRocket();
                    break;
                case PickupType.Shield:
                    UseShield();
                    break;
                case PickupType.Speed:
                    UseSpeed();
                    break;
            }
        }

        if (Pickup == PickupType.Rocket)
        {
            //Turn CrossHair UI On
        }
        else
        {
            //Turn CrossHair UI Off
        }
    }
    

    private void UseRocket()
    {
        //RaycastHit hit;
        //if(Physics.Raycast(GetComponentInChildren<Camera>().gameObject.transform.position, GetComponentInChildren<Camera>().gameObject.transform.TransformDirection(Vector3.forward), out hit , 500f, LayerMask.GetMask("Ground")))
        {
            //Vector3 DirectHit = hit.point - transform.position;
            //Debug.DrawLine(GetComponentInChildren<Camera>().gameObject.transform.position, hit.point);
            //GameObject RocketGm = Instantiate(Rocket,transform.position + transform.up * 2f,transform.rotation);
            GameObject RocketGm = Instantiate(Rocket,transform.position + transform.up * 2f ,transform.rotation);
            RocketGm.GetComponent<Rocket>().SetFiredBy(this.gameObject); 
 
            
            Collider rocketCollider = RocketGm.GetComponent<Collider>();

       
            Collider spawnerCollider = this.GetComponent<Collider>();

            if (spawnerCollider != null && rocketCollider != null)
            {
                Physics.IgnoreCollision(rocketCollider, spawnerCollider);
            }
            
            Collider[] spawnerChildrenColliders = this.GetComponentsInChildren<Collider>();
            foreach (var childCollider in spawnerChildrenColliders)
            {
                Physics.IgnoreCollision(rocketCollider, childCollider);
            }
            Pickup = PickupType.None; 
            
        }
        
        
    }

    private void UseShield()
    {
        State = Shield.IsOn;
        Pickup = PickupType.None;
        
        // spawn in a shield object
        if(!shield){
            shield = Instantiate(shieldGO , transform.position + new Vector3(0, 1, 0.25f),transform.rotation, transform);
            shield.GetComponent<ShieldCollider>().SetPlayer(this.gameObject);
        }
        

        StartCoroutine(UndoShield(ShieldTimer));
    }

    private void UseSpeed()
    {
        this.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 10000f, ForceMode.Impulse);
        Pickup = PickupType.None;
    }

    IEnumerator UndoShield(float Delay)
    {
        yield return new WaitForSeconds(Delay);
        State = Shield.IsOff;
        Destroy(shield);
        shield = null;
    }

  
}
