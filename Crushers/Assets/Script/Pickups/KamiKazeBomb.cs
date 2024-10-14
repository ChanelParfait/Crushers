using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamiKazeBomb : MonoBehaviour
{
    [SerializeField] private float growShrinkDuration = 10f;   // Duration before the bomb explodes
    [SerializeField] private Vector3 minScale = new Vector3(1f, 1f, 1f);  // Minimum scale
    [SerializeField] private Vector3 maxScale = new Vector3(2f, 2f, 2f);  // Maximum scale
    [SerializeField] private CarStats Player;
    public void SetPlayer(CarStats player)
    {
        Player = player;
    }
    
    void Update()
    {
        // Grow and shrink the bomb's scale
        float scaleFactor = Mathf.PingPong(Time.time, growShrinkDuration / 2f) / (growShrinkDuration / 2f);
        transform.localScale = Vector3.Lerp(minScale, maxScale, scaleFactor);
    }
}
