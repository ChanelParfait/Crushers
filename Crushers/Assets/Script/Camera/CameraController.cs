using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject playersCar;
    [SerializeField] private GameObject camera;
    [SerializeField] private Transform[] cameraLocations;
    [SerializeField] private PrometeoCarController carController;
    private int locationIndicator = 2;

    [Header("Camera Settings")]
    public Vector3 inputRotation;

    [Range(-1, 2)][SerializeField] private float sensetivity = 1f;
    [Range(0, 1)] public float smoothingTime = 0.5f;  

    private void Start()
    {
        cameraLocations = camera.GetComponentsInChildren<Transform>();
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    private void Update()
    {
        RotateCamera();
    }

    private void FixedUpdate()
    {
        SmoothCamera();
    }

    private void SmoothCamera()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (locationIndicator >= 4 || locationIndicator < 2) locationIndicator = 2;
            else locationIndicator++;
        }

        transform.position = cameraLocations[locationIndicator].position * (1 - smoothingTime) + transform.position * smoothingTime;
        transform.LookAt(cameraLocations[1].transform);

        smoothingTime = (carController.carSpeed >= 50f) ? Mathf.Abs((carController.carSpeed) / 50) - 0.85f : 0.45f;
    }

    public void RotateCamera() {

        if (Input.GetKey(KeyCode.Mouse2))
        {
            inputRotation.x += Input.GetAxis("Mouse X");
            inputRotation.y += Input.GetAxis("Mouse Y");
            transform.localRotation = Quaternion.Euler(
            inputRotation.y * sensetivity, inputRotation.x * sensetivity, 0);
        }
        
    }


}
