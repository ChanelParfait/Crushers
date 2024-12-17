using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Random = UnityEngine.Random;

public class ImpactController : MonoBehaviour
{
    private CinemachineImpulseSource impulseSource;

    private CarController carController;
    private CameraController cameraController;

    private Vector3 centerOfMassY;
    [SerializeField] private ScoreKeeper lastCollidedVehicle;

    [SerializeField] private TypeOfDeath DeathType;

    [SerializeField] private AudioSource crashAudio; 
    [SerializeField] private List<AudioClip> crashSFX;
    [SerializeField] private List<AudioClip> landSFX;


    [SerializeField] private ParticleSystem impactSpark;

    [SerializeField] private Collider bumperCollider;

    void OnEnable()
    {
        //PlayerManager.ArenaLevelLoaded +=  DisableSetupComponents;
    }

    void OnDisable()
    {
        CarController.hitGround -= OnGroundHit;

    }

    private void Start()
    {
        carController = GetComponent<CarController>(); 
        impulseSource = GetComponent<CinemachineImpulseSource>();
        cameraController = GetComponentInChildren<CameraController>();
        CarController.hitGround += OnGroundHit;

    }

    private void OnCollisionEnter(Collision collision)
    {
        // play impact audio
        // alter this to adjust volume for speed
        PlayAudio();
        Hit(collision);
    }

    // Checks if you hit the player
    // If yes, applies force and changes center of mass of the player you hit
    private void Hit(Collision collision)
    {
        //Calculate a hitForce based on the speed 
        float hitForce = carController.CalculateHitForce();
        hitForce = Mathf.Max(hitForce, 0f);

        //Calculate hit amplitude for CAMERA SHAKE
        float hitAmplitude = hitForce * 0.00008f;
        //Debug.Log("hitAmplitude: " + hitAmplitude);
        cameraController.ShakeCameraOnImpact(impulseSource, hitAmplitude);

        if (collision.gameObject.CompareTag("Player"))
        {

            //Get the rb of hit Object
            Rigidbody rb = collision.gameObject.GetComponentInParent<Rigidbody>();
            //Debug.Log("RB: " + rb.gameObject);
 
            if (rb != null)
            {
                CheckFrontBumperCollision(collision);
                PlayHitEffect(collision);

                Vector3 forceDirection = transform.forward;

                // Calculate new center of mass
                Vector3 newCenterOfMass = rb.centerOfMass + new Vector3(0, hitForce * 0.0004f, 0);
                newCenterOfMass.y = Mathf.Max(newCenterOfMass.y, 0f); 

                rb.centerOfMass = newCenterOfMass;
                Debug.Log("New center of mass on Y: " + newCenterOfMass);
                Debug.Log("hitforce: " + hitForce);
                rb.AddForce(forceDirection * hitForce, ForceMode.Impulse);
            }
        }
    }

    //We are setting a last collided vehicle with the player 
    private void CheckFrontBumperCollision(Collision collision) {

        ScoreKeeper collidedVehicle = collision.gameObject.GetComponentInParent<ScoreKeeper>();
        //Debug.Log("Collision with Player" + collidedVehicle.name);

        if (collidedVehicle)
        {
            // Set this vehicles last collided to the collided player
            SetLastCollidedVehicle(collidedVehicle);

            if (collidedVehicle.GetComponentInParent<ImpactController>().GetDeathType() != TypeOfDeath.Flip)
            {
                collidedVehicle.GetComponentInParent<ImpactController>().SetDeathType(TypeOfDeath.Flip);
            }
        }

    }

    public ScoreKeeper GetLastCollidedVehicle()
    {
        return lastCollidedVehicle;
    }
    public void SetLastCollidedVehicle(ScoreKeeper lastCollided)
    {
        if (lastCollided)
        {
            //Debug.Log("Set last Collided");
            StopCoroutine(ClearLastCollided(10f));
            lastCollidedVehicle = lastCollided;
            // Start coroutine to clear the last collided player after 5 seconds
            StartCoroutine(ClearLastCollided(10f));
        }

    }

    private IEnumerator ClearLastCollided(float delay)
    {
        yield return new WaitForSeconds(delay);
        lastCollidedVehicle = null;
        ResetMass();
    }

    public void ResetMass()
    {
        centerOfMassY.y = 0f;
        carController.SetActiveBodyMassCenterY(centerOfMassY);
    }

    private void OnGroundHit(){
        crashAudio.clip  = landSFX[Random.Range(0, landSFX.Count) ];
        crashAudio.volume = 0.35f;
        crashAudio.Play();

    }

    private void PlayAudio(){
        
        if(crashAudio){      
            float hitForce = carController.CalculateHitForce();
            // Map Hitforce to a value between 0 and 1
            hitForce = Mathf.Max(hitForce, 0f);
            
            float volume = Utility.Map(hitForce, 0, 6000, 0.1f, 0.5f);

            crashAudio.clip  = crashSFX[Random.Range(0, crashSFX.Count) ];
            crashAudio.volume = volume;
            crashAudio.Play();
        }
    }

    private void PlayHitEffect(Collision collision) {
        Vector3 impactPoint = collision.contacts[0].point;

        if (impactSpark != null)
        {
            ParticleSystem effect = Instantiate(impactSpark, impactPoint, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration + effect.main.startLifetime.constantMax);
        }
    }

    public TypeOfDeath GetDeathType()
    {
        return DeathType;
    }

    public void SetDeathType(TypeOfDeath setDeathType)
    {
        StopCoroutine(ClearSetDeathType(10f));
        DeathType = setDeathType;
        StartCoroutine(ClearSetDeathType(10f));
    }
    
    private IEnumerator ClearSetDeathType(float delay)
    {
        yield return new WaitForSeconds(delay);
        DeathType = TypeOfDeath.Flip;
    }
    

    
}
