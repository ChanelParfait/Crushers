using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpManager : MonoBehaviour
{
    [SerializeField] private PickupType Pickup;
    [SerializeField] private GameObject Rocket;


    [SerializeField] public Shield State;

    [SerializeField] private float ShieldTimer;
    public void SetPickup(PickupType PickUpPowerup)
    {
        Pickup = PickUpPowerup;
    }
    
    private void Update()
    {
        if (Input.GetKey(KeyCode.Q))
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
    }

  
}
