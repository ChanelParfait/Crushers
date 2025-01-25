using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FrontSpoiler", menuName = "Gears/FrontSpoiler")]
public class EQ_Front : GearDisplaySO
{
    public override void Use(GameObject controlledCar, string attachmentPoint)
    {
        Transform attachment = controlledCar.transform.Find(attachmentPoint);

        if (attachment == null)
        {
            Debug.LogError("Attachment point not found: " + attachmentPoint);
            return;
        }

        Vector3 spawnPosition = attachment.position + positionAdjustmentVector;
        Quaternion spawnRotation = Quaternion.Euler(rotationAdjustmentVector);

        GameObject gearInstance = Instantiate(gearPrefab, spawnPosition, spawnRotation);
        gearInstance.transform.SetParent(attachment);
    }

}
