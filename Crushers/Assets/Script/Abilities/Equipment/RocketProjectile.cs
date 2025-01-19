using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketProjectile : MonoBehaviour
{
    [SerializeField] private GameObject ExplosionVFX;
    [SerializeField] private ScoreKeeper FiredBy;
    [SerializeField] private float hitRadius;
    [SerializeField] private AudioClip[] explosionSFX;

    public void Initialize(float speed) {
        gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * speed, ForceMode.Force);
    }

    public void SetFiredBy(ScoreKeeper FiredFrom)
    {
        FiredBy = FiredFrom;
    }

    public ScoreKeeper GetFiredBy()
    {
        return FiredBy;
    }

    private void OnTriggerEnter(Collider other)
    {
        ExplosionDamage(this.transform.position, hitRadius);
        Destroy(this.gameObject);

    }

    private void OnDestroy()
    {
        // select random explosion SFX
        AudioClip activeClip = explosionSFX[Random.Range(0, explosionSFX.Length - 1)];
        GameObject listener = Camera.main.gameObject;
        AudioSource.PlayClipAtPoint(activeClip, listener.transform.position, 10);
        Debug.Log("Explode: " + listener);
    }

    private void ExplosionDamage(Vector3 postion, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(postion, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponentInParent<CarController>() != null &&
                hitCollider.GetComponentInParent<CarController>().gameObject.CompareTag("Player"))
            {
                // Check if the player's shield is not active
                if (hitCollider.GetComponentInParent<PickUpManager>().State != Shield.IsOn)
                {
                    // Apply explosion force to the player
                    hitCollider.GetComponentInParent<Rigidbody>().AddExplosionForce(100000, gameObject.transform.position, radius, 10, ForceMode.Force);
                    // set last collided vehicle of player
                    hitCollider.GetComponentInParent<ImpactController>().SetLastCollidedVehicle(FiredBy);
                }
            }
            if (ExplosionVFX)
            {
                GameObject Explosion = Instantiate(ExplosionVFX, this.gameObject.transform.position, this.gameObject.transform.rotation);
                Explosion.GetComponent<Explosion>().SetTimeBeforeDestruction(1);
            }
        }
    }
}
