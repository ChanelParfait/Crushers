using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float Timer;

    [SerializeField] private GameObject Explosion1;
    
    [SerializeField] private GameObject Explosion2;
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

    private void OnEnable()
    {
        if (Explosion1 && Explosion1)
        {
            int randomNum = UnityEngine.Random.Range(0, 1);
            if (randomNum == 0)
            {
                Explosion1.SetActive(true);
            }

            if (randomNum == 1)
            {
                Explosion2.SetActive(true);
            }
        }
        
    }
}
