using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldBubble", menuName = "Abilities/ShieldBubble")]
public class ShieldBubble : AbilityBase
{
    [Space(10)] 
    [Header("Ability parameters")] 
    [Space(20)] 
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private float ShieldTimer;
    [SerializeField] private GameObject shieldInstance;
    public override void Use(GameObject controlledCar)
    {
        Transform attachment = controlledCar.transform.Find("AttachmentsPos/" + attachmentPos.ToString());
        Vector3 spawnPosition = attachment.transform.position;
        Quaternion spawnRotation = Quaternion.Euler(0, 0, 0);
        
        
        if (!shieldInstance)
        {
            controlledCar.GetComponent<PickUpManager>().State = Shield.IsOn;
            shieldInstance = Instantiate(shieldPrefab, spawnPosition, spawnRotation);
            shieldInstance.transform.SetParent(attachment);
            shieldInstance.GetComponent<ShieldCollider>().SetPlayer(controlledCar);
            shieldInstance.GetComponent<ShieldCollider>().PlayAudio(1, 0);
            
            MonoBehaviour monoBehaviour = controlledCar.GetComponent<MonoBehaviour>();
            if (monoBehaviour)
            {
                monoBehaviour.StartCoroutine(UndoShield(ShieldTimer, controlledCar));
            }

        }
        
    }

    IEnumerator UndoShield(float Delay, GameObject controlledCar)
    {
        yield return new WaitForSeconds(Delay);
        controlledCar.GetComponent<PickUpManager>().State = Shield.IsOff;
        Destroy(shieldInstance);
        shieldInstance = null;
        
    }
}
