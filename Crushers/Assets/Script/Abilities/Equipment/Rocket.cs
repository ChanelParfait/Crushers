using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Rocket", menuName = "Abilities/Rocket")]
public class Rocket : AbilityBase
{
    [Space(10)]
    [Header("Equipment parameters")]
    [Space(20)]
    [SerializeField] private GameObject rocketEquipmentPrefab;
    [SerializeField] private float rocketSpeed = 1000f;
    [SerializeField] private GameObject explosionVFX;
    [SerializeField] private float hitRadius = 30f;
    public override void Use(GameObject controlledCar)
    {
        Transform attachment = controlledCar.transform.Find("AttachmentsPos/" + attachmentPos.ToString());
        Vector3 spawnPosition = attachment.transform.position;
        
        GameObject rocketInstance = Instantiate(rocketEquipmentPrefab, spawnPosition, controlledCar.transform.rotation);
        rocketInstance.GetComponent<RocketProjectile>().Initialize(rocketSpeed, hitRadius, controlledCar);
        
        Collider rocketCollider = rocketInstance.GetComponent<Collider>();
        Collider carCollider = controlledCar.GetComponent<Collider>(); 
        
        if (rocketCollider != null && carCollider != null)
        {
            Physics.IgnoreCollision(rocketCollider, carCollider);
        }
            
        Collider[] carChildrenColliders = controlledCar.GetComponentsInChildren<Collider>();
        foreach (var childCollider in carChildrenColliders)
        {
            Physics.IgnoreCollision(rocketCollider, childCollider);
        }
        
        
    }


}
