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
    [SerializeField] string title;
    [SerializeField] public Sprite icon;
    public AttachmentPos attachmentPos;
    public abstract void Use(GameObject controlledCar); 


}
