using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLines : MonoBehaviour
{
    private ParticleSystem speedLines;
    private ParticleSystem.EmissionModule emissionModule;
    private ParticleSystem.MainModule mainModule;

    private void Start()
    {
        speedLines = this.gameObject.GetComponent<ParticleSystem>();
        emissionModule = speedLines.emission;
        mainModule = speedLines.main;
    }

    public void scaleSpeedLinesOnAcceleration(float speed)
    {
        emissionModule.rateOverTime = Mathf.Lerp(10f, 30f, speed * Time.deltaTime);

        mainModule.startLifetime = Mathf.Lerp(0.5f, 1.5f, speed * Time.deltaTime);
        mainModule.startSpeed = Mathf.Lerp(20f, 40f, speed * Time.deltaTime);

        if (speed > 50)
        {
            speedLines.Play();    
        }
        else speedLines.Pause();
        
    }
}
