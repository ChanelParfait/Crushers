using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Abilities/Make New Ability", order = 0)]

public class Ability : ScriptableObject {

    [Header("Ability Info")]

    [SerializeField] string title;

    [SerializeField] Sprite icon;


}
