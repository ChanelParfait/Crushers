using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class CameraController : MonoBehaviour
{

    private CinemachineFreeLook freeLookCamera;
    private GameObject cameraLookAtPoint;

    private void Awake()
    { 
        freeLookCamera = GetComponent<CinemachineFreeLook>();
        cameraLookAtPoint = transform.parent.gameObject.transform.Find("CameraLookAtPoint").gameObject;
    }

    public void ShakeCameraOnImpact(CinemachineImpulseSource impulseSource, float impulseForce)
    {
        impulseSource.GenerateImpulseWithForce(impulseForce);
    }

    //CameraShake on acceleration is supposed to be slow to build up and easy to lose 
    public void ShakeCameraOnAcceleration(float speed) {

        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
       freeLookCamera.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
       
        //Sets max target amplitude to be half of the vehicles speed 
        float shakeSpeed = (speed * 0.5f ) * Time.deltaTime;

        float maxAplitude = 0.6f;

        float targetAmplitude = Mathf.Lerp(0, maxAplitude, shakeSpeed);
        //Debug.Log("Target amplitude: " + targetAmplitude);
        if (targetAmplitude > cinemachineBasicMultiChannelPerlin.m_AmplitudeGain)
        {
            // Accelerating: Increase shake slowly
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

        //Debug.Log("Camera shake: " + cinemachineBasicMultiChannelPerlin.m_AmplitudeGain);
    }

/*    public void MoveCameraLookAtPoint() {
        Vector3 cameraLookAtPointPos = cameraLookAtPoint.transform.position;
        cameraLookAtPointPos.x = Mathf.Clamp(cameraLookAtPointPos.x, -1.5f, 1.5f);
        cameraLookAtPoint.transform.position = cameraLookAtPointPos;
    }*/
}