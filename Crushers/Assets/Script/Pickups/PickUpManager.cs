using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum PickupType
{
    None = 0,
    Speed = 1,
    Rocket = 2,
    Shield = 3,
    KamiKaze = 4,
    Stun = 5,
}

public class PickUpManager : MonoBehaviour
{

    [SerializeField] private PickupType Pickup;

    [SerializeField] public Shield State;

    [SerializeField] private float ShieldTimer = 10;

    [SerializeField] private float KamiKazeTimer = 10;
    // Input Manager Flag
    public bool useItem = false;

    // Visual Gameobjects
    [SerializeField] private GameObject shieldGO;
    [SerializeField] private GameObject Rocket;

    [SerializeField] private GameObject Stun;

    // Pickup Object References
    private GameObject shield; 

    // UI Components
    [SerializeField] private Image pickUpImage;
    [SerializeField] private List<Sprite> pickupSprites;

    // SFX
    [SerializeField] private AudioSource audioSource;

    

    //For Camera need to change it to Same Level Camera as Player. Not the CineMachine one. 

    
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
                case PickupType.KamiKaze:
                    UseKamiKaze();
                    break;
                case PickupType.Stun:
                    UseStun();
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

    private void SetPickup(PickupType PickUpPowerup)
    {
        Pickup = PickUpPowerup;
        //Debug.Log("Pickup: " + PickUpPowerup.ToString());
        //Debug.Log("Gameobject" + gameObject.name);
        UpdateSprite(PickUpPowerup);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            //Debug.Log("Picked Up by" + other.gameObject.name);
            // Play item picked up SFX
            PlayAudio();
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
            shield.GetComponent<ShieldCollider>().PlayAudio(1, 0);
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
        yield return new WaitForSeconds(Delay - 0.2f);
        shield.GetComponent<ShieldCollider>().PlayAudio(2, 0);
        yield return new WaitForSeconds(0.2f);

        State = Shield.IsOff;
        Destroy(shield);
        shield = null;
    }

    private void UseKamiKaze()
    {
        
        StartCoroutine(KamiKazeExplosionTimer(KamiKazeTimer));
        Pickup = PickupType.None;
    }

    IEnumerator KamiKazeExplosionTimer(float Delay)
    {
        yield return new WaitForSeconds(Delay);
        Collider[] hitColliders = Physics.OverlapSphere(this.gameObject.transform.position, 30f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponentInParent<PickUpManager>() != null && 
                hitCollider.GetComponentInParent<PickUpManager>().gameObject.CompareTag("Player"))
            {
                // Check if the player's shield is not active
                if (hitCollider.GetComponentInParent<PickUpManager>().State != Shield.IsOn)
                {
                    // Apply explosion force to the player
                    
                    hitCollider.GetComponentInParent<Rigidbody>().AddExplosionForce(200000, gameObject.transform.position + Vector3.back * 2f , 30f, 5, ForceMode.Force);
                }
            }
            /*
            if (ExplosionVFX)
            {
                GameObject Explosion =  Instantiate(ExplosionVFX, this.gameObject.transform.position, this.gameObject.transform.rotation);
                Explosion.GetComponent<Explosion>().SetTimeBeforeDestruction(1);
            }
            */
        }
    }

    private void UseStun()
    {
        Vector3 newLocation = this.gameObject.transform.position - GetComponentInChildren<CinemachineFreeLook>().GetComponent<Transform>().transform.position;
        newLocation.y = 0f;
        GameObject StunGm = Instantiate(Stun,transform.position + transform.up * 2f, transform.rotation);
        StunGm.GetComponent<Stun>().SetFiredBy(this.gameObject); 
        StunGm.transform.rotation = Quaternion.LookRotation(newLocation);
            
        Collider rocketCollider = StunGm.GetComponent<Collider>();

       
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

    private void UpdateSprite(PickupType pickUpIndex){
        // set sprite using pickup index
        /*  None = 0,
            Speed = 1,
            Rocket = 2,
            Shield = 3,
            KamiKaze = 4,
            Stun = 5,
        */
    
        pickUpImage.sprite = pickupSprites[(int)pickUpIndex];
    }

    private void PlayAudio(){
        audioSource.Play();
    }

  
}
