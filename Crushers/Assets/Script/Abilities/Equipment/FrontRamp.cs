using UnityEngine;


[CreateAssetMenu(fileName = "FrontRamp", menuName = "Abilities/FrontRamp")]
public class FrontRamp : AbilityBase
{
    [Space(10)]
    [Header("Ability parameters")]
    [Space(20)]
    [SerializeField] private GameObject frontRampPrefab;
    [SerializeField] private Vector3 positionAdjustmentVector;

    public override void Use(GameObject controlledCar)
    {
        Transform attachment = controlledCar.transform.Find("AttachmentsPos/" + attachmentPos.ToString());
        Quaternion spawnRotation = Quaternion.Euler(6, -90, 0);
        Vector3 spawnPosition = attachment.transform.position + positionAdjustmentVector;
        GameObject springInstance = Instantiate(frontRampPrefab, spawnPosition, spawnRotation);
        springInstance.transform.SetParent(attachment);
    }
}
