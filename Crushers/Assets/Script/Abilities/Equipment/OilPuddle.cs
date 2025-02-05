using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
[CreateAssetMenu(fileName = "OilPuddle", menuName = "Abilities/OilPuddle")]
public class OilPuddle : AbilityBase
{
    [Space(10)]
    [Header("Equipment parameters")]
    [Space(20)]
    [SerializeField] private GameObject oilPuddlePrefab;
    [SerializeField] private Vector3 positionAdjustmentVector;
    [SerializeField] private Quaternion rotationAdjustmentQuaternion;


    public override void Use(GameObject controlledCar)
    {
        Transform attachment = controlledCar.transform.Find("AttachmentsPos/" + attachmentPos.ToString());
        Vector3 spawnPosition = attachment.transform.position + positionAdjustmentVector;
        Quaternion spawnRotation = rotationAdjustmentQuaternion;
        VisualEffect oilPuddleVFX = oilPuddlePrefab.GetComponentInChildren<VisualEffect>();

        //We instatiating the game object of oil emisson and playing effect
        oilPuddleVFX = Instantiate(oilPuddleVFX, spawnPosition, spawnRotation);
        oilPuddleVFX.Play();
    }
}
