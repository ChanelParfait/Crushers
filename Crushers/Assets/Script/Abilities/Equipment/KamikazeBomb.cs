using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "KamikazeBomb", menuName = "Abilities/KamikazeBomb")]
public class KamikazeBomb : AbilityBase
{
    [Space(10)] 
    [Header("Ability parameters")] 
    [Space(20)]
    [SerializeField] private GameObject kazikazebombPrefab;
    [SerializeField] private float kazikazebombTimer;
    [SerializeField] private float kazikazebombRadius;
    [SerializeField] private GameObject kazikazebombInstance;
    public override void Use(GameObject controlledCar)
    {
        Transform attachment = controlledCar.transform.Find("AttachmentsPos/" + attachmentPos.ToString());
        Vector3 spawnPosition = attachment.transform.position;
        Quaternion spawnRotation = Quaternion.Euler(0, 0, 0);
        
        if (!kazikazebombInstance)
        {
            kazikazebombInstance = Instantiate(kazikazebombPrefab,spawnPosition, spawnRotation);
            kazikazebombInstance.transform.SetParent(attachment);
            
            MonoBehaviour monoBehaviour = controlledCar.GetComponent<MonoBehaviour>();
            if (monoBehaviour)
            {
                monoBehaviour.StartCoroutine(KamiKazeExplosionTimer(kazikazebombTimer, controlledCar));
            }
        }
        
    }
    
    IEnumerator KamiKazeExplosionTimer(float Delay, GameObject controlledCar)
    {
        yield return new WaitForSeconds(Delay);
        Collider[] hitColliders = Physics.OverlapSphere(controlledCar.transform.position, kazikazebombRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponentInParent<PickUpManager>() != null && 
                hitCollider.GetComponentInParent<PickUpManager>().gameObject.CompareTag("Player"))
            {
                // Check if the player's shield is not active
                if (hitCollider.GetComponentInParent<PickUpManager>().State != Shield.IsOn)
                {
                    // Apply explosion force to the player
                    
                    hitCollider.GetComponentInParent<Rigidbody>().AddExplosionForce(200000, controlledCar.transform.position + Vector3.back * 2f , 30f, 5, ForceMode.Force);

                    if (hitCollider.GetComponentInParent<ScoreKeeper>() != controlledCar.GetComponent<ScoreKeeper>())
                    {
                        hitCollider.gameObject.GetComponentInParent<ImpactController>().SetLastCollidedVehicle(controlledCar.GetComponent<ScoreKeeper>());
                        
                        hitCollider.gameObject.GetComponentInParent<ImpactController>().SetDeathType(TypeOfDeath.Rocket);
                    }
                }
            }
        }
        Destroy(kazikazebombInstance);
        
    }
}
