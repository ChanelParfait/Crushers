using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "FrontRamp", menuName = "Abilities/FrontRamp")]
public class FrontRamp : AbilityBase
{
    [Space(10)]
    [Header("Ability parameters")]
    [Space(20)]
    [SerializeField] private GameObject frontRampPrefab;

    public override void Use(GameObject controlledCar)
    {
        Transform attachment = controlledCar.transform.Find("AttachmentsPos/" + attachmentPos.ToString());
        Quaternion spawnRotation = Quaternion.Euler(6, -90, 0);
        Vector3 spawnPosition = attachment.transform.position + new Vector3(0.14f,-0.65f,-1);
        GameObject springInstance = Instantiate(frontRampPrefab, spawnPosition, spawnRotation);
        springInstance.transform.SetParent(attachment);
    }
}
