using System;
using UnityEngine;

public class PickUpBehavior : MonoBehaviour
{
    // Event triggered when the pickup is collected
    public event Action OnPickupCollected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPickupCollected?.Invoke();
            
            Destroy(gameObject);
        }
    }
}
