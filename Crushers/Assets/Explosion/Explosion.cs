using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float Timer;

    private float TimeBeforeDestruction;

    private void Update()
    {
        Timer = Timer + Time.deltaTime;

        if (Timer > TimeBeforeDestruction)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetTimeBeforeDestruction(float time)
    {
        TimeBeforeDestruction = time;
    }
    
    
}
