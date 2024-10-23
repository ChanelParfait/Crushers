using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    private CinemachineFreeLook freeLookCamera;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
        }
        
        freeLookCamera = GetComponent<CinemachineFreeLook>();
    }


    public void ShakeCameraOnImpact(CinemachineImpulseSource impulseSource, float impulseForce)
    {
        impulseSource.GenerateImpulseWithForce(impulseForce);
    }

    public void ShakeCameraOnAcceleration(float speed) {

        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
       freeLookCamera.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        float shakeSpeed = (speed * 0.5f ) * Time.deltaTime;

        float maxAplitude = 0.6f;

        float targetAmplitude = Mathf.Lerp(0, maxAplitude, shakeSpeed);
        Debug.Log("Target amplitude: " + targetAmplitude);
        if (targetAmplitude > cinemachineBasicMultiChannelPerlin.m_AmplitudeGain)
        {
            // Accelerating: Increase shake quickly
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain,
                targetAmplitude,
                Time.deltaTime * 0.25f  
            );
        }
        else
        {
            // Decelerating: Decrease shake faster
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain,
                targetAmplitude,
                Time.deltaTime * 100f 
            );
        }

        Debug.Log("Camera shake: " + cinemachineBasicMultiChannelPerlin.m_AmplitudeGain);
    }
}