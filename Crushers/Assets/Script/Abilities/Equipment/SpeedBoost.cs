using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeedBoost", menuName = "Abilities/SpeedBoost")]

public class SpeedBoost : AbilityBase
{
    [Space(10)] 
    [Header("Ability parameters")] 
    [Space(20)]
    //[SerializeField] private GameObject speedboostPrefab;
    [SerializeField] private AudioClip sfx;
     public override void Use(GameObject controlledCar)
    {
        /*Transform attachment = controlledCar.transform.Find("AttachmentsPos/" + attachmentPos.ToString());
        Vector3 spawnPosition = attachment.transform.position;
        Quaternion spawnRotation = Quaternion.Euler(0, 0, 0);
        */
        controlledCar.GetComponent<PickUpManager>().PlayAudio(sfx);
        controlledCar.GetComponent<Rigidbody>().AddForce(controlledCar.transform.forward * 10000f, ForceMode.Impulse);
    }
     
    
    
    
}
