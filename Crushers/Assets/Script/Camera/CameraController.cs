using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    private CinemachineFreeLook freeLookCamera;
    private float shakeTimer;
    private float startingAmplitude;
    private float startingFrequency;
    private float shakeTimerTotal;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
        }
        
        freeLookCamera = GetComponent<CinemachineFreeLook>();
    }



    public void ShakeCamera(CinemachineImpulseSource impulseSource, float impulseForce)
    {
        impulseSource.GenerateImpulseWithForce(impulseForce);

       // Debug.Log($"Camera Shake Amplitude: {amplitude}, Frequency: {frequency}, Duration: {time}");
    }
}