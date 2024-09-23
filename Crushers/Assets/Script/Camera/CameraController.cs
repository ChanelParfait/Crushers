using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour, AxisState.IInputAxisProvider
{
    public static CameraController Instance { get; private set; }

    private CinemachineFreeLook freeLookCamera;
    private float shakeTimer;
    private float startingAmplitude;
    private float startingFrequency;
    private float shakeTimerTotal;

    [HideInInspector]
    public InputAction horizontal;
    [HideInInspector]
    public InputAction vertical;

    private void Awake()
    {
        Instance = this;
        freeLookCamera = GetComponent<CinemachineFreeLook>();
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

            CinemachineBasicMultiChannelPerlin perlin =
                freeLookCamera.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            if (perlin != null)
            {
                // Gradually reduce amplitude and frequency
                perlin.m_AmplitudeGain = Mathf.Lerp(startingAmplitude, 0f, 1 - (shakeTimer / shakeTimerTotal));
                perlin.m_FrequencyGain = Mathf.Lerp(startingFrequency, 0f, 1 - (shakeTimer / shakeTimerTotal));
            }
        }
    }

    public float GetAxisValue(int axis)
    {
        switch (axis)
        {
            case 0: return horizontal.ReadValue<Vector2>().x;
            case 1: return horizontal.ReadValue<Vector2>().y;
            case 2: return vertical.ReadValue<float>();
        }

        return 0;
    }

    public void ShakeCamera(float amplitude, float frequency, float time)
    {
        CinemachineBasicMultiChannelPerlin perlin =
            freeLookCamera.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if (perlin != null)
        {
            perlin.m_AmplitudeGain = amplitude;
            perlin.m_FrequencyGain = frequency;

            startingAmplitude = amplitude;
            startingFrequency = frequency;
            shakeTimerTotal = time;
            shakeTimer = time;
        }

        Debug.Log($"Camera Shake Amplitude: {amplitude}, Frequency: {frequency}, Duration: {time}");
    }
}