using System;
using System.Collections;
using System.Collections.Generic;
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
    }

    private void UseRocket()
    {
        GameObject RocketGm = Instantiate(Rocket,transform.position + transform.forward * 10f + transform.up * 2f ,transform.rotation);
        
        RocketGm.GetComponent<Rocket>().SetFiredBy(this.gameObject);
        Pickup = PickupType.None;
    }

    private void UseShield()
    {
        State = Shield.IsOn;
        Pickup = PickupType.None;
        
        // spawn in a shield object
        if(!shield){
            shield = Instantiate(shieldGO , transform.position + new Vector3(0, 1, 0.25f),transform.rotation, transform);
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
