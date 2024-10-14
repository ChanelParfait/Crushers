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

       // Debug.Log($"Camera Shake Amplitude: {amplitude}, Frequency: {frequency}, Duration: {time}");
    }

    public void ShakeCameraOnAcceleration(float speed) {

        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
       freeLookCamera.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        float shakeSpeed = speed * Time.deltaTime;

        float maxAplitude = 1f;

        float targetAmplitude = Mathf.Lerp(0, maxAplitude, shakeSpeed);

        if (targetAmplitude > cinemachineBasicMultiChannelPerlin.m_AmplitudeGain)
        {
            // Accelerating: Increase shake quickly
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain,
                targetAmplitude,
                Time.deltaTime * 5f  
            );
        }
        else
        {
            // Decelerating: Decrease shake faster
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain,
                targetAmplitude,
                Time.deltaTime * 10f 
            );
        }

        //Debug.Log("Camera shake: " + cinemachineBasicMultiChannelPerlin.m_AmplitudeGain);
    }
}