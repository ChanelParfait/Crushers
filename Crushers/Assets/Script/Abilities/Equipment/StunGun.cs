using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "StunGun", menuName = "Abilities/StunGun")]
public class StunGun : AbilityBase
{
    [Space(10)] 
    [Header("Ability parameters")] 
    [Space(20)]
    [SerializeField] private GameObject stungunPrefab;
    [SerializeField] private AudioClip sfx;
    [SerializeField] private GameObject stungunInstance;
    
    public override void Use(GameObject controlledCar)
    {
        Transform attachment = controlledCar.transform.Find("AttachmentsPos/" + attachmentPos.ToString());
        Vector3 spawnPosition = attachment.transform.position;
        

        if (!stungunInstance)
        {
            controlledCar.GetComponent<PickUpManager>().PlayAudio(sfx);
            stungunInstance = Instantiate(stungunPrefab,spawnPosition, controlledCar.transform.rotation);
            stungunInstance .GetComponent<Stun>().SetFiredBy(controlledCar.GetComponent<ScoreKeeper>()); 
        
            Collider rocketCollider =  stungunInstance.GetComponent<Collider>();
            Collider spawnerCollider = controlledCar.GetComponent<Collider>();

            if (spawnerCollider != null && rocketCollider != null)
            {
                Physics.IgnoreCollision(rocketCollider, spawnerCollider);
            }
            
            Collider[] spawnerChildrenColliders = controlledCar.GetComponentsInChildren<Collider>();
            foreach (var childCollider in spawnerChildrenColliders)
            {
                Physics.IgnoreCollision(rocketCollider, childCollider);
            }
            
        }
        
    }
    
    
}
