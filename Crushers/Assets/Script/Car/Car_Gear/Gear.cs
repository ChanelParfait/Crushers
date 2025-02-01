using UnityEngine;

public class Gear : MonoBehaviour
{
    public string GearID;
    public string Name;
    public Sprite GearIcon;   // UI representation
    public GameObject Model;  // 3D model prefab
    public Quaternion gearRotation;
    public Vector3 gearPosition;
}
