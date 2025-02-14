using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class CameraController : MonoBehaviour
{

    private CinemachineFreeLook freeLookCamera;
    private GameObject cameraLookAtPoint;
    private Vector3 cameraLookAtPointPos; 

    private void Awake()
    { 
        freeLookCamera = GetComponent<CinemachineFreeLook>();
        cameraLookAtPoint = transform.parent.gameObject.transform.Find("CameraLookAtPoint").gameObject;
    }
    private void Start()
    {
       cameraLookAtPointPos = cameraLookAtPoint.transform.localPosition;
    }

    private void Update()
    {
        centerCameraAssist();
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

    //lerp needs to be run in update 
    public void moveCameraAssist(float cameraSpeed)
    {
        float currentPos;
        float updatedPos;

        currentPos = cameraLookAtPointPos.x;
        if (freeLookCamera.transform.localPosition.x > -2 && freeLookCamera.transform.localPosition.x < 2 && freeLookCamera.transform.localPosition.z < 0)
        {
            cameraLookAtPointPos.x += Time.deltaTime * cameraSpeed;

            cameraLookAtPointPos.x = Mathf.Clamp(cameraLookAtPointPos.x, -0.5f, 0.5f);

            updatedPos = cameraLookAtPointPos.x;

            Mathf.Lerp(currentPos, updatedPos, 0.1f * Time.deltaTime);

            updatedPos = currentPos;
        }
        cameraLookAtPoint.transform.localPosition = cameraLookAtPointPos;
    }

    public void centerCameraAssist() {

        if ((freeLookCamera.transform.localPosition.x > 2 
            || freeLookCamera.transform.localPosition.x < -2)
            && (cameraLookAtPointPos.x > 0.0001f || cameraLookAtPointPos.x < -0.0001f)) {
            Debug.Log("centering");
            cameraLookAtPointPos.x = Mathf.Lerp(cameraLookAtPointPos.x, 0, 5f * Time.deltaTime);
            cameraLookAtPoint.transform.localPosition = cameraLookAtPointPos;
        }
    }

    /*    public void MoveCameraAssist(float cameraAssistSpeed) {
            // Get the current local position of the freeLookCamera
            Vector3 localPosition = freeLookCamera.transform.localPosition;

            if (Mathf.Round(localPosition.x) >= -2f - 0.7f && Mathf.Round(localPosition.x) <= 2f + 0.7f && localPosition.x < 0)
            {
                // Adjust the x position with assistance speed
                freeLookCamera.m_XAxis.Value += Time.deltaTime * cameraAssistSpeed;

                // Clamp the x position within the range [-2, 2]
                localPosition.x = Mathf.Clamp(localPosition.x, -2f, 2f);
            }


            // Update the local position of the camera
            freeLookCamera.transform.localPosition = localPosition;
        }*/
}