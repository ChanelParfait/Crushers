using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rocket", menuName = "Abilities/Rocket")]
public class Rocket : AbilityBase
{
    [Space(10)]
    [Header("Equipment parameters")]
    [Space(20)]
    [SerializeField] private GameObject rocketEquipmentPrefab;
    [SerializeField] private float rocketSpeed = 100f;
    [SerializeField] private GameObject explosionVFX;
    public override void Use(GameObject controlledCar)
    {
        Transform attachment = controlledCar.transform.Find("AttachmentsPos/" + attachmentPos.ToString());
        Vector3 spawnPosition = attachment.transform.position;
        Quaternion spawnRotation = Quaternion.Euler(0, -90, 0);
        GameObject rocketInstance = Instantiate(rocketEquipmentPrefab, spawnPosition, spawnRotation);
        RocketProjectile rocketProjectile = rocketInstance.AddComponent<RocketProjectile>();
        rocketProjectile.Initialize(rocketSpeed);
    }


}
