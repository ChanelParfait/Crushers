using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    public string pickupName;
    public Sprite pickupIcon;

    public abstract void ApplyEffect(GameObject target);

    public virtual void TestEffect(GameObject target)
    {
        Debug.Log($"Testing {pickupName} effect.");
        ApplyEffect(target);
    }
}
