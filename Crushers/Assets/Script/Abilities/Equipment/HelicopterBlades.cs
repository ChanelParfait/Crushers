using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HelicopterBlades", menuName = "Abilities/HelicopterBlades")]
public class HelicopterBlades : AbilityBase
{
    [Space(10)]
    [Header("Equipment parameters")]
    [Space(20)]
    [SerializeField] private GameObject helicopterPrefab;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float activeTime;

    [SerializeField] private float hoverForce = 10f; // Upward force applied to make the car hover
    [SerializeField] private float hoverHeight = 5f; // Desired height for hovering
    
    public override void Use(GameObject controlledCar)
    {
        Transform attachment = controlledCar.transform.Find("AttachmentsPos/" + attachmentPos.ToString());
        Vector3 spawnPosition = attachment.transform.position;
        Quaternion spawnRotation = Quaternion.Euler(0, 0, 0);

        GameObject helicopterBladesInstance = Instantiate(helicopterPrefab, spawnPosition, spawnRotation);
        helicopterBladesInstance.transform.SetParent(attachment);

        Rigidbody carRigidbody = controlledCar.GetComponent<Rigidbody>();
        if (carRigidbody != null)
        {
            controlledCar.GetComponent<MonoBehaviour>().StartCoroutine(ApplyHoverForce(carRigidbody));
        }
        Destroy(helicopterBladesInstance, activeTime);  
    }

    private IEnumerator ApplyHoverForce(Rigidbody carRigidbody)
    {
        float timer = 0f;

        while (timer < activeTime)
        {
            Ray ray = new Ray(carRigidbody.transform.position, Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                float distanceToGround = hit.distance;
                if (distanceToGround < hoverHeight)
                {
                    
                    float forceMagnitude = hoverForce * (hoverHeight - distanceToGround) - carRigidbody.velocity.y;
                    carRigidbody.AddForce(Vector3.up * forceMagnitude, ForceMode.Acceleration);
                    carRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                }
            }

            timer += Time.deltaTime;
            yield return null;
        }
        carRigidbody.constraints = RigidbodyConstraints.None;
    }
}
