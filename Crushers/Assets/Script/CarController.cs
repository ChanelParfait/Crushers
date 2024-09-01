using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityStandardAssets.Vehicles.Car;

public class CarController : MonoBehaviour
{
    [Header("----------References-----------")]
    [SerializeField] private Rigidbody carRB;
    [SerializeField] private Transform[] rayPoints;
    [SerializeField] private LayerMask drivable;
    [SerializeField] private Transform acclerationPoint;

    [Header("-------Suspension Settings-----")]

    [SerializeField] private float springStiffness;
    [SerializeField] private float damperStiffness;

    [SerializeField] private float restLength;
    [SerializeField] private float springTravel;
    [SerializeField] private float wheelRadius;

    private int[] wheelsIsGrounded = new int[4];
    private bool isGrounded = false;

    [Header("--------------Input-------------")]
    private float moveInput = 0;
    private float steerInput = 0;

    [Header("--------------Input-------------")]
    [SerializeField] private float acceleration = 25f;
    [SerializeField] private float maxSpeed = 100f;
    [SerializeField] private float deceleration = 10f;

    private Vector3 currentCarLocalVelocity = Vector3.zero;
    private float carVelocityRatio = 0;

    void Start()
    {
        carRB = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Suspension();
        GroundCheck();
        CalculateCarVelocity();
        Move();
    }
    private void Update()
    {
        GetPlayerInput();
    }

    #region Movement

    private void Move() {
        if (isGrounded) {
            Accelerate();
            Decelerate();
        }
    }

    private void Accelerate() {
        carRB.AddForceAtPosition(acceleration * moveInput * transform.forward, acclerationPoint.position, ForceMode.Acceleration);
    }

    private void Decelerate() {
        carRB.AddForceAtPosition(deceleration * moveInput * -transform.forward, acclerationPoint.position, ForceMode.Acceleration);
    }

    #endregion

    #region Car Status Check

    private void GroundCheck() {
        int tempGroundedWheels = 0;

        for (int i = 0; i < wheelsIsGrounded.Length; i++)
        {
            tempGroundedWheels += wheelsIsGrounded[i];
        }

        if (tempGroundedWheels > 1) {
            isGrounded = true;
        }
        else { isGrounded = false; }    
    }

    private void CalculateCarVelocity() {
        currentCarLocalVelocity = transform.InverseTransformDirection(carRB.velocity);
        carVelocityRatio = currentCarLocalVelocity.z / maxSpeed;

    }

    #endregion



    #region Input Handling

    private void GetPlayerInput() {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
    }

    #endregion

    #region Suspension Function
    private void Suspension() {
        for (int i = 0; i < rayPoints.Length; i++) {
            RaycastHit hit;
            float maxLength = restLength + springTravel;

            if (Physics.Raycast(rayPoints[i].position, -rayPoints[i].up, out hit, maxLength + wheelRadius, drivable))
            {
                wheelsIsGrounded[i] = 1;

                float currentSpringLength = hit.distance - wheelRadius;
                float springCompression = restLength - currentSpringLength / springTravel;

                float springVelocity = Vector3.Dot(carRB.GetPointVelocity(rayPoints[i].position), rayPoints[i].up);
                float dampForce = damperStiffness + springVelocity;

                float springForce = springStiffness * springCompression;

                float netForce = springForce - dampForce;

                carRB.AddForceAtPosition(netForce* rayPoints[i].up, rayPoints[i].position);
                Debug.DrawLine(rayPoints[i].position, hit.point, Color.red);
            }
            else {
                wheelsIsGrounded[i] = 0; 
                
                Debug.DrawLine(rayPoints[i].position,rayPoints[i].position+(wheelRadius + maxLength)* -rayPoints[i].up, Color.green);
            }
        }
        

    }

    #endregion
}