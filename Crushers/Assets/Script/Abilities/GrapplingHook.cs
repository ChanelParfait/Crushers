using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "GrapplingHook", menuName = "Abilities/GrapplingHook")]
public class GrapplingHook : AbilityBase
{
    [Space(10)]
    [Header("Ability parameters")]
    [Space(20)]
    [SerializeField] private GameObject grapplingHookPrefab;
    [SerializeField] private float hookSpeed = 1f;
    [SerializeField] private float pullSpeed = 10f;
    [SerializeField] private int hookingDistance = 25;

    public override void Use(GameObject controlledCar)
    {
        Transform attachment = controlledCar.transform.Find("AttachmentsPos/" + attachmentPos.ToString());
        Vector3 spawnPosition = attachment.transform.position;
        Quaternion spawnRotation = Quaternion.Euler(0, 0, 90);


        CarController closestEnemy = FindNearestEnemy(controlledCar);
        if (closestEnemy == null) return;


        //Instantiate hookProjectile entity 
        GameObject hookInstance = Instantiate(grapplingHookPrefab, spawnPosition, spawnRotation);

        Transform ropeStart = hookInstance.transform.Find("RopeStart");
        if (ropeStart != null)
        {
            ropeStart.SetParent(attachment,false);
        }
        GrapplingHookProjectile hookProjectile = hookInstance.AddComponent<GrapplingHookProjectile>();
        hookProjectile.Initialize(closestEnemy.transform.position + new Vector3(0,2,0), hookSpeed, controlledCar, closestEnemy, pullSpeed);
        hookProjectile.transform.SetParent(closestEnemy.transform);
    }

    //Search for a collider that has carController, if yes, search for a nearest on to player to grapple
    private CarController FindNearestEnemy(GameObject controlledCar)
    {
        Collider[] colliderArray = Physics.OverlapSphere(controlledCar.transform.position, hookingDistance);
        CarController nearestCarController = null;
        float shortestDistance = float.MaxValue;

        foreach (Collider collider in colliderArray)
        {
            
            if (collider.TryGetComponent<CarController>(out CarController carController) && collider.gameObject != controlledCar)
            {
                float distance = Vector3.Distance(controlledCar.transform.position, carController.transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestCarController = carController;
                }
            }
        }
        return nearestCarController;
    }
}
