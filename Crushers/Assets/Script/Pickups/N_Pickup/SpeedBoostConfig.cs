using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pickup/SpeedBoost")]
public class SpeedBoostConfig : ScriptableObject
{
    public string pickupName;
    public Sprite pickupIcon;
    public float speedMultiplier;
    public float duration;
}

