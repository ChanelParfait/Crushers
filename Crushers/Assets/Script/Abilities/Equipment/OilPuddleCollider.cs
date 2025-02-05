using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class OilPuddleCollider : MonoBehaviour
{
    private BoxCollider puddleCollider;
    private VisualEffect oilPuddleVFX;
    private float puddleLifeTime;

    [SerializeField] private SlipperyEffect slipperyEffectPrefab; 

    private void Start()
    {
        puddleCollider = GetComponent<BoxCollider>();
        oilPuddleVFX = GetComponent<VisualEffect>();
       
        puddleLifeTime = oilPuddleVFX.GetFloat("PuddleLifetime");

        puddleCollider.enabled = false;
        ApplyEffect();
    }

    //Applying effect of the loose traction to the vehicle
    //After the effect is applied destroy the game object of the puddle. It's equals to the effect lifetime
    public void ApplyEffect()
    {
        puddleCollider.enabled = true;

        StartCoroutine(DestroyPuddleAfterTime(puddleLifeTime));
    }

    //On enter attach the script that will cause the effect
    private void OnTriggerEnter(Collider other)
    {
        var wheelColliders = other.GetComponentsInChildren<WheelCollider>();

        if (wheelColliders.Length > 0)
        {
            
            if (slipperyEffectPrefab != null)
            {
                var slipperyEffect = other.gameObject.AddComponent<SlipperyEffect>();
                slipperyEffect.Initialize(wheelColliders); // Pass the WheelColliders to the SlipperyEffect
            }
            else
            {
                Debug.LogWarning("SlipperyEffect prefab is not assigned in the OilPuddleCollider script.");
            }
        }
    }

    //On exit start coroutine to restore friction to the original values
    private void OnTriggerExit(Collider other)
    {
        var wheelColliders = other.GetComponentsInChildren<WheelCollider>();

        if (wheelColliders.Length > 0)
        { 
            var slipperyEffect = other.gameObject.GetComponent<SlipperyEffect>();

            if (slipperyEffect != null)
            {
                slipperyEffect.RestoreFriction();
            }
        }
    }

    private IEnumerator DestroyPuddleAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}