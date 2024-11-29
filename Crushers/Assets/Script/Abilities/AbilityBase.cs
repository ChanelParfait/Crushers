using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class AbilityBase : ScriptableObject
{
    [Header("Ability Info")]
    [SerializeField] string title;
    [SerializeField] public Sprite icon;

    public abstract void Use(); 


}
