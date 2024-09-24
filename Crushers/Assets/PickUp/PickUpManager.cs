using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class PickUpManager : MonoBehaviour
{
    [SerializeField] private GameObject shieldGO; 
    [SerializeField] private GameObject Rocket;

    [SerializeField] private PickupType Pickup;

    [SerializeField] public Shield State;

    [SerializeField] private float ShieldTimer = 10;
    public bool useItem = false;


    // Visual Gameobjects
    private GameObject shield; 
    // UI Components
    [SerializeField] private Image pickUpImage;
    [SerializeField] private List<Sprite> pickupSprites;
    //For Camera need to change it to Same Level Camera as Player. Not the CineMachine one. 
    public void SetPickup(PickupType PickUpPowerup)
    {
        Pickup = PickUpPowerup;
        Debug.Log("Pickup: " + PickUpPowerup.ToString());
        //Debug.Log("Gameobject" + gameObject.name);
        UpdateSprite(PickUpPowerup);
    }
    
    private void Start(){
        UpdateSprite(PickupType.None);

    }
    
    private void Update()
    {
        if (useItem)
        {
            UpdateSprite(PickupType.None);
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

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.CompareTag("Pickup"))
        {
            Debug.Log("Picked Up by" + other.gameObject.name);
            SetPickup(other.GetComponent<BasePickUp>().GetPickupType());
            Destroy(other.gameObject);
        }
        
        
    }
    

    private void UseRocket()
    {
        
        Vector3 newLocation = this.gameObject.transform.position - GetComponentInChildren<CinemachineFreeLook>().GetComponent<Transform>().transform.position;
        newLocation.y = 0f;
            GameObject RocketGm = Instantiate(Rocket,transform.position + transform.up * 2f, transform.rotation);
            RocketGm.GetComponent<Rocket>().SetFiredBy(this.gameObject); 
            RocketGm.transform.rotation = Quaternion.LookRotation(newLocation);
            
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

    private void UpdateSprite(PickupType pickUpIndex){
        // set sprite using pickup sprite index
        /*  None = 0
            Speed = 1
            Rocket = 2
            Shield = 3 
        */
    
        pickUpImage.sprite = pickupSprites[(int)pickUpIndex];
    }

  
}
