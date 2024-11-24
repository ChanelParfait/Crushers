using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostPickup : Pickup
{
    public float speedMultiplier = 2f;
    public float duration = 5f;

    public override void ApplyEffect(GameObject target)
    {
        // Logic to apply speed boost
        Debug.Log($"Applying Speed Boost to {target.name}");
        // Example logic: target.GetComponent<PlayerController>().BoostSpeed(speedMultiplier, duration);
    }
}
