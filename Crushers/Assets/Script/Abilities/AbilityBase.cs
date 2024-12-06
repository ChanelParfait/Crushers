using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public enum AttachmentPos
{
    Back,
    Bottom,
    Left,
    Right,
    Top,
    Front
}

public abstract class AbilityBase : ScriptableObject
{
    [Header("Ability Info")]
    [SerializeField] public AttachmentPos attachmentPos;
    [SerializeField] public string title;
    [SerializeField] public Sprite icon;
    
    public abstract void Use(GameObject controlledCar); 


}
