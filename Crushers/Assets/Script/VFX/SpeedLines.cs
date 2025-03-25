using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLines : MonoBehaviour
{
    [SerializeField] private ParticleSystem speedLines;
    private ParticleSystem.EmissionModule emissionModule;

    [SerializeField] private float speedThreshold = 50f;
    [SerializeField] private float emmisionsPerFrame = 10f;


    private void Start()
    {
        speedLines = gameObject.GetComponent<ParticleSystem>();
        emissionModule = speedLines.emission;
    }

    public void scaleSpeedLinesOnAcceleration(float speed)
    {
        if(speedLines){
            if (!speedLines.isPlaying) {
                speedLines.Play();
            }

            if (speed > speedThreshold)
            {
                float emissionRate = Mathf.Lerp(0f, 100f, emmisionsPerFrame *Time.deltaTime);
                emissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(emissionRate);
            }
            else
            {
                emissionModule.rateOverTime = 0f;
            }
        } 
        /*else {
            speedLines = gameObject.GetComponent<ParticleSystem>();
        }*/

    }
}
