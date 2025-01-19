using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
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
    [SerializeField] private GameObject shieldGO; 
    [SerializeField] private GameObject Rocket;
    [SerializeField] private GameObject KamiKazeGo;
    [SerializeField] private GameObject Stun;

    [SerializeField] private PickupType Pickup;

    [SerializeField] public Shield State;

    [SerializeField] private float ShieldTimer = 7;

    [SerializeField] private float KamiKazeTimer = 10;

    [SerializeField] private float KamiKazeRadius;
    public bool useItem = false;


    [SerializeField] public GameObject DisabledEffect;

    // Visual Gameobjects
    private GameObject shield;

    private GameObject KamiKaze;
    // UI Components
    [SerializeField] private Image pickUpImage;
    [SerializeField] private List<Sprite> pickupSprites;

    // SFX
    [SerializeField] private AudioSource audioSource;
    // 0 = glimmer //  1 = speed // 2 = stun //
    [SerializeField] private AudioClip[] sfx;


    //For Camera need to change it to Same Level Camera as Player. Not the CineMachine one. 
    public void SetPickup(PickupType PickUpPowerup)
    {
        Pickup = PickUpPowerup;
        //Debug.Log("Pickup: " + PickUpPowerup.ToString());
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

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.CompareTag("Pickup"))
        {
            //Debug.Log("Picked Up by" + other.gameObject.name);
            // play pickup audio
            PlayAudio(sfx[0]);
            SetPickup(other.GetComponent<BasePickUp>().GetPickupType());
            Destroy(other.gameObject);
        }
        
        
    }
    

    private void UseRocket()
    {
        
        /*Vector3 newLocation = this.gameObject.transform.position - GetComponentInChildren<CinemachineFreeLook>().GetComponent<Transform>().transform.position;
        newLocation.y = 0f;
        */
            GameObject RocketGm = Instantiate(Rocket,transform.position + transform.up * 2f + transform.forward, transform.rotation);
            RocketGm.GetComponent<RocketPickup>().SetFiredBy(this.gameObject.GetComponent<ScoreKeeper>()); 
            //RocketGm.transform.rotation = Quaternion.LookRotation(newLocation);
            
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
        PlayAudio(sfx[1]);
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

    private void UseKamiKaze()
    {
        if (!KamiKaze)
        {
            KamiKaze = Instantiate(KamiKazeGo, this.gameObject.transform.position + new Vector3(0, 7, 0.25f), transform.rotation, transform);
        }
        
        StartCoroutine(KamiKazeExplosionTimer(KamiKazeTimer));
        Pickup = PickupType.None;

    }

    IEnumerator KamiKazeExplosionTimer(float Delay)
    {
        yield return new WaitForSeconds(Delay);
        Collider[] hitColliders = Physics.OverlapSphere(this.gameObject.transform.position, KamiKazeRadius);
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
                    
                    hitCollider.gameObject.GetComponentInParent<ImpactController>().SetLastCollidedVehicle(gameObject.GetComponent<ScoreKeeper>());
                }
            }
        }
        Destroy(KamiKaze);
        
    }

    private void UseStun()
    {
        PlayAudio(sfx[2]);
        /*Vector3 newLocation = this.gameObject.transform.position - GetComponentInChildren<CinemachineFreeLook>().GetComponent<Transform>().transform.position;
        newLocation.y = 0f;*/
        GameObject StunGm = Instantiate(Stun,transform.position + transform.up * 2f + transform.forward, transform.rotation);
        StunGm.GetComponent<Stun>().SetFiredBy(this.gameObject.GetComponent<ScoreKeeper>()); 
        //StunGm.transform.rotation = Quaternion.LookRotation(newLocation);
            
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
        // set sprite using pickup sprite index
        /*  None = 0
            Speed = 1
            Rocket = 2
            Shield = 3 
        */
    
        pickUpImage.sprite = pickupSprites[(int)pickUpIndex];
    }

    
    private void PlayAudio(AudioClip clip){
        audioSource.clip = clip;
        audioSource.Play();
    }

  
}
