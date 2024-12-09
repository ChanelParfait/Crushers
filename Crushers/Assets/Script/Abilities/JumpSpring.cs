using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "JumpSpring", menuName = "Abilities/JumpSpring")]
public class JumpSpring : AbilityBase
{
    [Space(10)]
    [Header("Ability parameters")]
    [Space(20)]
    [SerializeField] GameObject jumpSpringPrefab;
    [SerializeField] float jumpSpringForce;
    [SerializeField] float activatedTime;

    public override void Use(GameObject controlledCar)
    {
        //Here we find attribute child object to spawn out prefab 
        Transform attachment = controlledCar.transform.Find("AttachmentsPos/" + attachmentPos.ToString());
       
        // We get the Rigidbody from the parent GameObject
        Rigidbody carRigidBody = controlledCar.GetComponent<Rigidbody>();

        // Calculate spawn position on the point of the "Attributes"
        Vector3 spawnPosition = attachment.transform.position;
        Quaternion spawnRotation = Quaternion.Euler(-180,0,0);

        GameObject springInstance = Instantiate(jumpSpringPrefab, spawnPosition, spawnRotation);
        springInstance.transform.SetParent(attachment);

        // Apply force
        carRigidBody.AddForce(Vector3.up * jumpSpringForce, ForceMode.Impulse);

        Destroy(springInstance, activatedTime);
    }


}

