using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PickupConfig))]
public class PickUpConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PickupConfig config = (PickupConfig)target;

        // Draw the PickupType enum dropdown
        config.PUtype = (PUtype)EditorGUILayout.EnumPopup("Pickup Type", config.PUtype);

        // Draw relevant fields based on the selected PickupType
        switch (config.PUtype)
        {
            case PUtype.Traps:
                config.DmgMultiplier = EditorGUILayout.Slider("Damage Multiplier", config.DmgMultiplier, 1f, 2f);
                config.speedMultiplier = EditorGUILayout.FloatField("Speed Duration", config.speedMultiplier);
                config.duration = EditorGUILayout.FloatField("Duration", config.duration);
                break;

            case PUtype.Stats:
                config.StatsMultiplier = EditorGUILayout.FloatField("Stats Amount", config.StatsMultiplier);
                config.duration = EditorGUILayout.FloatField("Duration", config.duration);
                break;

            case PUtype.Projectile:
                config.DmgMultiplier = EditorGUILayout.Slider("Damage Multiplier", config.DmgMultiplier, 1f, 2f);
                config.speedMultiplier = EditorGUILayout.FloatField("Projectile Speed", config.speedMultiplier);
                config.duration = EditorGUILayout.FloatField("Duration", config.duration);
                config.radius = EditorGUILayout.Slider("Radius", config.radius, 1f, 30f);
                config.homing = EditorGUILayout.Toggle("Homing", config.homing);
                if (config.homing)
                {
                    //onfig.homingSpeed = EditorGUILayout.FloatField("Homing Speed", config.homingSpeed);
                    //config.homingAccuracy = EditorGUILayout.FloatField("Homing Accuracy", config.homingAccuracy);
                }
                break;
        }

        // Save changes made in the editor
        if (GUI.changed)
        {
            EditorUtility.SetDirty(config);
        }
    }
}
