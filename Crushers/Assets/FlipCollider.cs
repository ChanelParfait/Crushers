using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.name == "RespawnCollider")
        {
            GetComponentInParent<CarRespawn>().Respawn();
        }
    }
}
