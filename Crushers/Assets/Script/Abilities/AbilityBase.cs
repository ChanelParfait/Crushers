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
    [Space(20)]
    [SerializeField] public AttachmentPos attachmentPos;
    [SerializeField] public string title;
    [SerializeField] public Sprite icon;
    [SerializeField] private int cooldownTime;

    [SerializeField] private ParticleSystem hitVFX;
    [SerializeField] private AudioClip[] launchSFX;
    [SerializeField] private AudioClip[] hitSFX;
    
    public abstract void Use(GameObject controlledCar);

    public int GetCooldownTime() {
        return cooldownTime;
    }
}
