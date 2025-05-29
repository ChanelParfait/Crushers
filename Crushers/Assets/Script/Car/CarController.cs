using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public enum VehicleType
{
    Standard,
    Small,
    Big,
    Police,
    Lightning,
    Hook,
    Surf,
    Vampire,
    Unknown
}

public class CarController : MonoBehaviour
{
    private VehicleType vehicleType;

    [SerializeField] private Car car;
    // create public getter for car in order to retrive the cars index
    public Car GetCar() { return car; }

    [SerializeField] private int playerIndex;

    private ImpactController impactController;

    private CameraController cameraController;

    [HideInInspector] public bool isMovingForward;
    [HideInInspector] public bool isReversing;
    [HideInInspector] public bool isBraking;
    [HideInInspector] public bool isTurning;
    [HideInInspector] public float steeringAngle;
    [HideInInspector] public bool isDecelerating;
    [HideInInspector] public bool isGrounded = true;

    // Event
    public static UnityAction hitGround;

    //BASE STATS / CAR DATA
    [Header("CAR DATA")]

    [Header("Acceleration")]
    [Space(10)]

    [SerializeField] private int activeMaxSpeed;
    [SerializeField] private int activeMaxReverseSpeed;
    [SerializeField] private int activeAccelerationMultiplier;
    [SerializeField] private float carSpeed;

    [Space(10)]
    [Header("Steering")]
    [Space(10)]

    [SerializeField] private int activeMaxSteeringAngle;
    [SerializeField] private float activeSteeringSpeed;

    [Space(10)]
    [Header("Deceleration")]
    [Space(10)]

    [SerializeField] private int activeBrakeForce;
    [SerializeField] private float activeDecelerationMultiplier;
    [SerializeField] private int activeHandbrakeDriftMultiplier;

    [Space(10)]
    [Header("Deceleration")]
    [Space(10)]

    private Vector3 activeBodyMassCenter;
    [SerializeField] private int activeBodyMass;
    [SerializeField] private int activeGravityMultiplier;
    [SerializeField] private float activeDamageMultiplier;

    [HideInInspector] public bool isDrifting; // Used to know whether the car is drifting or not.
    [HideInInspector] public bool isTractionLocked; // Used to know whether the traction of the car is locked or not.
    
    //WHEELS

    [Header("WHEELS SETUP")]
    [Space(10)]
    /*
    The following variables are used to store the wheels' data of the car. We need both the mesh-only game objects and wheel
    collider components of the wheels. The wheel collider components and 3D meshes of the wheels cannot come from the same
    game object; they must be separate game objects.
    */
    public GameObject frontLeftMesh;
      public WheelCollider frontLeftCollider;
      [Space(10)]
      public GameObject frontRightMesh;
      public WheelCollider frontRightCollider;
      [Space(10)]
      public GameObject rearLeftMesh;
      public WheelCollider rearLeftCollider;
      [Space(10)]
      public GameObject rearRightMesh;
      public WheelCollider rearRightCollider;

    //PARTICLE SYSTEMS

      [Space(10)]
      [Header("EFFECTS SETUP")]
      [Space(10)]
      //The following variable lets you to set up particle systems in your car
      public bool useEffects = false;

      // The following particle systems are used as tire smoke when the car drifts.
      public ParticleSystem RLWParticleSystem;
      public ParticleSystem RRWParticleSystem;

      [Space(10)]
      // The following trail renderers are used as tire skids when the car loses traction.
      public TrailRenderer RLWTireSkid;
      public TrailRenderer RRWTireSkid;

      [Space(10)]
      //Speedlines       
      public SpeedLines speedLines;


      [Space(20)]
      [Header("UI SETUP")]
      [Space(10)]
      //The following variable lets you to set up a UI text to display the speed of your car.
      public bool useUI = false;
      public TextMeshProUGUI carSpeedText; // Used to store the UI object that is going to show the speed of the car.

    //SOUNDS
      [Space(10)]
      [Header("AUDIO SETUP")]
      [Space(10)]
      [SerializeField] private bool useSounds = false;
      [SerializeField] private AudioSource carEngineSound;
      [SerializeField] private AudioSource driftingSound;
      [SerializeField] private AudioSource hornSound;
      [SerializeField] private float vehiclePitch; 
     // private float initialCarEngineSoundPitch;


    //PRIVATE VARIABLES

      /*
      IMPORTANT: The following variables should not be modified manually since their values are automatically given via script.
      */
      Rigidbody carRigidbody; // Stores the car's rigidbody.
      float steeringAxis; // Used to know whether the steering wheel has reached the maximum value. It goes from -1 to 1.
      float throttleAxis; // Used to know whether the throttle has reached the maximum value. It goes from -1 to 1.
      float driftingAxis;
      float localVelocityZ;
      float localVelocityX;
      /*
      The following variables are used to store information about sideways friction of the wheels (such as
      extremumSlip,extremumValue, asymptoteSlip, asymptoteValue and stiffness). We change this values to
      make the car to start drifting.
      */
      WheelFrictionCurve FLwheelFriction;
      float FLWextremumSlip;
      WheelFrictionCurve FRwheelFriction;
      float FRWextremumSlip;
      WheelFrictionCurve RLwheelFriction;
      float RLWextremumSlip;
      WheelFrictionCurve RRwheelFriction;
      float RRWextremumSlip;
    private void Awake()
    {
        //We assign base car stats to the stats of the vehicle that will be controlled by player. All the rest methods will be affecting "Current" stats
        activeMaxSpeed = car.GetBaseMaxSpeed();
        activeMaxReverseSpeed = car.GetBaseMaxReverseSpeed();
        activeAccelerationMultiplier = car.GetBaseAccelerationMultiplier();
        activeMaxSteeringAngle = car.GetBaseMaxSteeringAngle();
        activeSteeringSpeed = car.GetBaseSteeringSpeed();
        activeBrakeForce = car.GetBaseBrakeForce();
        activeDecelerationMultiplier = car.GetBaseDecelerationMultiplier();
        activeHandbrakeDriftMultiplier = car.GetBaseHandbrakeDriftMultiplier();
        activeBodyMass = car.GetBaseBodyMass();
        activeGravityMultiplier = car.GetBaseGravityMultiplier();
        activeDamageMultiplier = car.GetBaseDamageMultiplier();
    }

    void OnDisable()
    {
        Debug.Log("Destroying Vehicle");
    }

    // Start is called before the first frame update
    void Start()
    {
      impactController = GameObject.FindObjectOfType<ImpactController>();
      cameraController = GetComponentInChildren<CameraController>();
      //In this part, we set the 'carRigidbody' value with the Rigidbody attached to this
      //gameObject. Also, we define the center of mass of the car with the Vector3 given
      //in the inspector.
      carRigidbody = gameObject.GetComponent<PredictedRigidbody>().predictedRigidbody;
      carRigidbody.centerOfMass = activeBodyMassCenter;
      carRigidbody.mass = activeBodyMass;
      isGrounded = true;
      //Initial setup to calculate the drift value of the car. This part could look a bit
      //complicated, but do not be afraid, the only thing we're doing here is to save the default
      //friction values of the car wheels so we can set an appropiate drifting value later.
      FLwheelFriction = new WheelFrictionCurve ();
        FLwheelFriction.extremumSlip = frontLeftCollider.sidewaysFriction.extremumSlip;
        FLWextremumSlip = frontLeftCollider.sidewaysFriction.extremumSlip;
        FLwheelFriction.extremumValue = frontLeftCollider.sidewaysFriction.extremumValue;
        FLwheelFriction.asymptoteSlip = frontLeftCollider.sidewaysFriction.asymptoteSlip;
        FLwheelFriction.asymptoteValue = frontLeftCollider.sidewaysFriction.asymptoteValue;
        FLwheelFriction.stiffness = frontLeftCollider.sidewaysFriction.stiffness;
      FRwheelFriction = new WheelFrictionCurve ();
        FRwheelFriction.extremumSlip = frontRightCollider.sidewaysFriction.extremumSlip;
        FRWextremumSlip = frontRightCollider.sidewaysFriction.extremumSlip;
        FRwheelFriction.extremumValue = frontRightCollider.sidewaysFriction.extremumValue;
        FRwheelFriction.asymptoteSlip = frontRightCollider.sidewaysFriction.asymptoteSlip;
        FRwheelFriction.asymptoteValue = frontRightCollider.sidewaysFriction.asymptoteValue;
        FRwheelFriction.stiffness = frontRightCollider.sidewaysFriction.stiffness;
      RLwheelFriction = new WheelFrictionCurve ();
        RLwheelFriction.extremumSlip = rearLeftCollider.sidewaysFriction.extremumSlip;
        RLWextremumSlip = rearLeftCollider.sidewaysFriction.extremumSlip;
        RLwheelFriction.extremumValue = rearLeftCollider.sidewaysFriction.extremumValue;
        RLwheelFriction.asymptoteSlip = rearLeftCollider.sidewaysFriction.asymptoteSlip;
        RLwheelFriction.asymptoteValue = rearLeftCollider.sidewaysFriction.asymptoteValue;
        RLwheelFriction.stiffness = rearLeftCollider.sidewaysFriction.stiffness;
      RRwheelFriction = new WheelFrictionCurve ();
        RRwheelFriction.extremumSlip = rearRightCollider.sidewaysFriction.extremumSlip;
        RRWextremumSlip = rearRightCollider.sidewaysFriction.extremumSlip;
        RRwheelFriction.extremumValue = rearRightCollider.sidewaysFriction.extremumValue;
        RRwheelFriction.asymptoteSlip = rearRightCollider.sidewaysFriction.asymptoteSlip;
        RRwheelFriction.asymptoteValue = rearRightCollider.sidewaysFriction.asymptoteValue;
        RRwheelFriction.stiffness = rearRightCollider.sidewaysFriction.stiffness;

        // We invoke 2 methods inside this script. CarSpeedUI() changes the text of the UI object that stores
        // the speed of the car and CarSounds() controls the engine and drifting sounds. Both methods are invoked
        // in 0 seconds, and repeatedly called every 0.1 seconds.
        if(useUI){
          InvokeRepeating("CarSpeedUI", 0f, 0.1f);
        }else if(!useUI){
          if(carSpeedText != null){
            carSpeedText.text = "0";
          }
        }

        if(useSounds){
          InvokeRepeating("CarSounds", 0f, 0.1f);
        }else if(!useSounds)
        {
          if(carEngineSound != null){
            carEngineSound.Stop();
          }
          if(driftingSound != null){
                driftingSound.Stop();
          }
        }

        if(!useEffects){
          if(RLWParticleSystem != null){
            RLWParticleSystem.Stop();
          }
          if(RRWParticleSystem != null){
            RRWParticleSystem.Stop();
          }
          if(RLWTireSkid != null){
            RLWTireSkid.emitting = false;
          }
          if(RRWTireSkid != null){
            RRWTireSkid.emitting = false;
          }
        }

    }

    public void SetVehicleType(VehicleType type)
    {
        vehicleType = type;
        switch (vehicleType)
        {

            case VehicleType.Standard:
                break;
            case VehicleType.Small:
                break;
            case VehicleType.Big:
                break;
            case VehicleType.Police:
                break;
            case VehicleType.Lightning:
                break;
            case VehicleType.Hook:
                break;
            case VehicleType.Vampire:
                break;
            case VehicleType.Surf:
                break;
            case VehicleType.Unknown:
                Debug.LogWarning("Vehicle type unkown, setting as default");
                break;
        }
    }

    public void SetPlayerIndex(int index){
      playerIndex = index; 
    }

    public int GetPlayerIndex(){
      return playerIndex; 
    }

    void Update()
    {
        GroundChecker();
        GravityModifier();


    //CAR DATA

    // We determine the speed of the car.
    // swap out variable
    carSpeed = GetComponent<PredictedRigidbody>().predictedRigidbody.velocity.magnitude * 3.6f;
        // Save the local velocity of the car in the x axis. Used to know if the car is drifting.
        localVelocityX = transform.InverseTransformDirection(GetComponent<PredictedRigidbody>().predictedRigidbody.velocity).x;
        // Save the local velocity of the car in the z axis. Used to know if the car is going forward or backwards.
        localVelocityZ = transform.InverseTransformDirection(GetComponent<PredictedRigidbody>().predictedRigidbody.velocity).z;

        //CAR PHYSICS

        /*
        The next part is regarding to the car controller. First, it checks if the user wants to use touch controls (for
        mobile devices) or analog input controls (WASD + Space).

        The following methods are called whenever a certain key is pressed. For example, in the first 'if' we call the
        method GoForward() if the user has pressed W.

        In this part of the code we specify what the car needs to do if the user presses W (throttle), S (reverse),
        A (turn left), D (turn right) or Space bar (handbrake).
        */


        if (isMovingForward && !isReversing)
        {
            CancelInvoke("DecelerateCar");
            isDecelerating = false;
            GoForward();
        }
        if (isReversing)
        {
            CancelInvoke("DecelerateCar");
            isDecelerating = false;
            GoReverse();
        }

        if (isBraking)
        {
            CancelInvoke("DecelerateCar");
            isDecelerating = false;
            Handbrake();
        }

        if (!isReversing && !isMovingForward)
        {
            ThrottleOff();
        }
        if (!isReversing && !isMovingForward && !isBraking && !isDecelerating)
        {
            InvokeRepeating("DecelerateCar", 0f, 0.2f);

        }

      // update wheels
      UpdateWheels();
      // We call the method AnimateWheelMeshes() in order to match the wheel collider movements with the 3D meshes of the wheels.
      AnimateWheelMeshes();

      //speedLines.scaleSpeedLinesOnAcceleration(carSpeed);
    }

    private void FixedUpdate()
    {
        cameraController.ShakeCameraOnAcceleration(carSpeed);
    }

    // This method converts the car speed data from float to string, and then set the text of the UI carSpeedText with this value.
    public void CarSpeedUI(){

      if(useUI){
          try{
            float absoluteCarSpeed = Mathf.Abs(carSpeed);
            carSpeedText.text = Mathf.RoundToInt(absoluteCarSpeed).ToString() + " KM/H";
          }catch(Exception ex){
            Debug.LogWarning(ex);
          }
      }
    }

    // This method controls the car sounds. For example, the car engine will sound slow when the car speed is low because the
    // pitch of the sound will be at its lowest point. On the other hand, it will sound fast when the car speed is high because
    // the pitch of the sound will be the sum of the initial pitch + the car speed divided by 100f.
    // Apart from that, the tireScreechSound will play whenever the car starts drifting or losing traction.
    public void CarSounds(){

      if(useSounds){
        try{
          if(carEngineSound != null){
            float engineSoundPitch = 1 + (Mathf.Abs(carRigidbody.velocity.magnitude) / 25f);
            carEngineSound.pitch = engineSoundPitch;
            if(!carEngineSound.isPlaying){
              carEngineSound.Play();
            }
          }
          if((isDrifting) || (isTractionLocked && Mathf.Abs(carSpeed) > 12f)){
            if(!driftingSound.isPlaying){
              // randomly pitch drift sound up and down
              driftingSound.pitch = UnityEngine.Random.Range(vehiclePitch - 0.2f, vehiclePitch + 0.2f); 
              //driftingSound.pitch = vehiclePitch; 
              
              driftingSound.Play();
            }
          }else if((!isDrifting) && (!isTractionLocked || Mathf.Abs(carSpeed) < 12f)){
            driftingSound.Stop();
          }
        }catch(Exception ex){
          Debug.LogWarning(ex);
        }
      }else if(!useSounds)
        {
        if(carEngineSound != null && carEngineSound.isPlaying){
          carEngineSound.Stop();
        }
        if(carEngineSound != null && carEngineSound.isPlaying){
          driftingSound.Stop();
        }
      }

    }

    public void HonkHorn() {
        if (!hornSound.isPlaying) {
                //Debug.Log("Hooooooonk");

            hornSound.pitch = UnityEngine.Random.Range(vehiclePitch - 0.2f, vehiclePitch + 0.2f);
            hornSound.Play();
        }
    }

    // JUMP to get unstuck
    public void Jump(){
      //Debug.Log("Jump");
      GetComponent<PredictedRigidbody>().predictedRigidbody.AddForce(GetComponent<PredictedRigidbody>().predictedRigidbody.transform.forward * 1000); 
    }

    //
    //STEERING METHODS
    //

    public void SetSteeringAngle(Vector2 direction){

      ////Debug.Log("Wheel Direction: " + direction);
      steeringAxis = direction.x;
      steeringAngle = steeringAxis * activeMaxSteeringAngle;

        if (direction != Vector2.zero)
        {
            //cameraController.moveCameraAssist(steeringAngle/10);
        }
    }

    public void UpdateWheels(){
      frontLeftCollider.steerAngle = Mathf.Lerp(frontLeftCollider.steerAngle, steeringAngle, activeSteeringSpeed);
      frontRightCollider.steerAngle = Mathf.Lerp(frontRightCollider.steerAngle, steeringAngle, activeSteeringSpeed);
    }


    // This method matches both the position and rotation of the WheelColliders with the WheelMeshes.
    void AnimateWheelMeshes(){
      try{
        Quaternion FLWRotation;
        Vector3 FLWPosition;
        frontLeftCollider.GetWorldPose(out FLWPosition, out FLWRotation);
        frontLeftMesh.transform.position = FLWPosition;
        frontLeftMesh.transform.rotation = FLWRotation;

        Quaternion FRWRotation;
        Vector3 FRWPosition;
        frontRightCollider.GetWorldPose(out FRWPosition, out FRWRotation);
        frontRightMesh.transform.position = FRWPosition;
        frontRightMesh.transform.rotation = FRWRotation;

        Quaternion RLWRotation;
        Vector3 RLWPosition;
        rearLeftCollider.GetWorldPose(out RLWPosition, out RLWRotation);
        rearLeftMesh.transform.position = RLWPosition;
        rearLeftMesh.transform.rotation = RLWRotation;

        Quaternion RRWRotation;
        Vector3 RRWPosition;
        rearRightCollider.GetWorldPose(out RRWPosition, out RRWRotation);
        rearRightMesh.transform.position = RRWPosition;
        rearRightMesh.transform.rotation = RRWRotation;
      }catch(Exception ex){
        Debug.LogWarning(ex);
      }
    }

    //
    //ENGINE AND BRAKING METHODS
    //

    // This method apply positive torque to the wheels in order to go forward.
    public void GoForward(){

      //If the forces aplied to the rigidbody in the 'x' asis are greater than
      //3f, it means that the car is losing traction, then the car will start emitting particle systems.
      if(Mathf.Abs(localVelocityX) > 2.5f){
        isDrifting = true;
        DriftCarPS();
      }else{
        isDrifting = false;
        DriftCarPS();
      }
      // The following part sets the throttle power to 1 smoothly.
      throttleAxis = throttleAxis + (Time.deltaTime * 3f);
      if(throttleAxis > 1f){
        throttleAxis = 1f;
      }
      //If the car is going backwards, then apply brakes in order to avoid strange
      //behaviours. If the local velocity in the 'z' axis is less than -1f, then it
      //is safe to apply positive torque to go forward.
      if(localVelocityZ < -1f){
        Brakes();
      }else{
        if(Mathf.RoundToInt(carSpeed) < activeMaxSpeed)
            {
          //Apply positive torque in all wheels to go forward if maxSpeed has not been reached.
          frontLeftCollider.brakeTorque = 0;
          frontLeftCollider.motorTorque = (activeAccelerationMultiplier * 50f) * throttleAxis;
          frontRightCollider.brakeTorque = 0;
          frontRightCollider.motorTorque = (activeAccelerationMultiplier * 50f) * throttleAxis;
          rearLeftCollider.brakeTorque = 0;
          rearLeftCollider.motorTorque = (activeAccelerationMultiplier * 50f) * throttleAxis;
          rearRightCollider.brakeTorque = 0;
          rearRightCollider.motorTorque = (activeAccelerationMultiplier * 50f) * throttleAxis;
        }else {
          // If the maxSpeed has been reached, then stop applying torque to the wheels.
          // IMPORTANT: The maxSpeed variable should be considered as an approximation; the speed of the car
          // could be a bit higher than expected.
    			frontLeftCollider.motorTorque = 0;
    			frontRightCollider.motorTorque = 0;
          rearLeftCollider.motorTorque = 0;
    			rearRightCollider.motorTorque = 0;
    		}
      }
    }

    // This method apply negative torque to the wheels in order to go backwards.
    public void GoReverse(){
        //Debug.Log("Is going reverse");
      //If the forces aplied to the rigidbody in the 'x' asis are greater than
      //3f, it means that the car is losing traction, then the car will start emitting particle systems.
      if(Mathf.Abs(localVelocityX) > 2.5f){
        isDrifting = true;
        DriftCarPS();
      }else{
        isDrifting = false;
        DriftCarPS();
      }
      // The following part sets the throttle power to -1 smoothly.
      throttleAxis = throttleAxis - (Time.deltaTime * 3f);
      if(throttleAxis < -1f){
        throttleAxis = -1f;
      }
      //If the car is still going forward, then apply brakes in order to avoid strange
      //behaviours. If the local velocity in the 'z' axis is greater than 1f, then it
      //is safe to apply negative torque to go reverse.
      if(localVelocityZ > 1f){
        Brakes();
      }else{
        if(Mathf.Abs(Mathf.RoundToInt(carSpeed)) < activeMaxReverseSpeed){
          //Apply negative torque in all wheels to go in reverse if maxReverseSpeed has not been reached.
          frontLeftCollider.brakeTorque = 0;
          frontLeftCollider.motorTorque = (activeAccelerationMultiplier * 50f) * throttleAxis;
          frontRightCollider.brakeTorque = 0;
          frontRightCollider.motorTorque = (activeAccelerationMultiplier * 50f) * throttleAxis;
          rearLeftCollider.brakeTorque = 0;
          rearLeftCollider.motorTorque = (activeAccelerationMultiplier * 50f) * throttleAxis;
          rearRightCollider.brakeTorque = 0;
          rearRightCollider.motorTorque = (activeAccelerationMultiplier * 50f) * throttleAxis;
        }else {
          //If the maxReverseSpeed has been reached, then stop applying torque to the wheels.
          // IMPORTANT: The maxReverseSpeed variable should be considered as an approximation; the speed of the car
          // could be a bit higher than expected.
    			frontLeftCollider.motorTorque = 0;
    			frontRightCollider.motorTorque = 0;
          rearLeftCollider.motorTorque = 0;
    			rearRightCollider.motorTorque = 0;
    		}
      }
    }


    //The following function set the motor torque to 0 (in case the user is not pressing either W or S).
    public void ThrottleOff(){
      frontLeftCollider.motorTorque = 0;
      frontRightCollider.motorTorque = 0;
      rearLeftCollider.motorTorque = 0;
      rearRightCollider.motorTorque = 0;
    }

    // The following method decelerates the speed of the car according to the decelerationMultiplier variable, where
    // 1 is the slowest and 10 is the fastest deceleration. This method is called by the function InvokeRepeating,
    // usually every 0.1f when the user is not pressing W (throttle), S (reverse) or Space bar (handbrake).
    public void DecelerateCar(){
      isDecelerating = true;
      //Debug.Log("Decelerating");
      if(Mathf.Abs(localVelocityX) > 2.5f){
        isDrifting = true;
        DriftCarPS();
      }else{
        isDrifting = false;
        DriftCarPS();
      }
      // The following part resets the throttle power to 0 smoothly.
      if(throttleAxis != 0f){
        if(throttleAxis > 0f){
          throttleAxis = throttleAxis - (Time.deltaTime * 10f);
        }else if(throttleAxis < 0f){
            throttleAxis = throttleAxis + (Time.deltaTime * 10f);
        }
        if(Mathf.Abs(throttleAxis) < 0.15f){
          throttleAxis = 0f;
        }
      }

      GetComponent<PredictedRigidbody>().predictedRigidbody.velocity = GetComponent<PredictedRigidbody>().predictedRigidbody.velocity * (1f / (1f + (0.025f * activeDecelerationMultiplier)));
      // Since we want to decelerate the car, we are going to remove the torque from the wheels of the car.
      frontLeftCollider.motorTorque = 0;
      frontRightCollider.motorTorque = 0;
      rearLeftCollider.motorTorque = 0;
      rearRightCollider.motorTorque = 0;
      // If the car speed is less than 15f (very slow velocity), then stop the car completely and
      // also cancel the invoke of this method.
      if(carSpeed < 15f){
        GetComponent<PredictedRigidbody>().predictedRigidbody.velocity = Vector3.zero;
        CancelInvoke("DecelerateCar");
      }
    }

    // This function applies brake torque to the wheels according to the brake force given by the user.
    public void Brakes(){
      frontLeftCollider.brakeTorque = activeBrakeForce;
      frontRightCollider.brakeTorque = activeBrakeForce;
      rearLeftCollider.brakeTorque = activeBrakeForce;
      rearRightCollider.brakeTorque = activeBrakeForce;

        //If the vehicle is still going forward, apply reverse speed to the ridigbody to stop vehicle faster
        if (localVelocityZ>1f)
        {
            Vector3 reverseForce = -transform.forward * activeBrakeForce * 100 * Time.deltaTime;
            GetComponent<PredictedRigidbody>().predictedRigidbody.AddForce(reverseForce, ForceMode.Force);
        }
    }

    // This function is used to make the car lose traction. By using this, the car will start drifting. The amount of traction lost
    // will depend on the handbrakeDriftMultiplier variable. If this value is small, then the car will not drift too much, but if
    // it is high, then you could make the car to feel like going on ice.
    public void Handbrake(){
      CancelInvoke("RecoverTraction");
    // We are going to start losing traction smoothly, there is were our 'driftingAxis' variable takes
    // place. This variable will start from 0 and will reach a top value of 1, which means that the maximum
    // drifting value has been reached. It will increase smoothly by using the variable Time.deltaTime.
      driftingAxis += Time.deltaTime;
      driftingAxis = Mathf.Clamp(driftingAxis, 0f, 1f);
      float secureStartingPoint = driftingAxis * FLWextremumSlip * activeHandbrakeDriftMultiplier;
    
      if(secureStartingPoint < FLWextremumSlip){
        driftingAxis = FLWextremumSlip / (FLWextremumSlip * activeHandbrakeDriftMultiplier);
      }
      if(driftingAxis > 1f){
        driftingAxis = 1f;
      }
      //If the forces aplied to the rigidbody in the 'x' asis are greater than
      //3f, it means that the car lost its traction, then the car will start emitting particle systems.
      if(Mathf.Abs(localVelocityX) > 2.5f){
        isDrifting = true;
      }else{
        isDrifting = false;
      }
      //If the 'driftingAxis' value is not 1f, it means that the wheels have not reach their maximum drifting
      //value, so, we are going to continue increasing the sideways friction of the wheels until driftingAxis
      // = 1f.
      if(driftingAxis < 1f){
        float factor = activeHandbrakeDriftMultiplier * driftingAxis;

        FLwheelFriction.extremumSlip = FLWextremumSlip * factor;
        frontLeftCollider.sidewaysFriction = FLwheelFriction;

        FRwheelFriction.extremumSlip = FRWextremumSlip * factor;
        frontRightCollider.sidewaysFriction = FRwheelFriction;

        RLwheelFriction.extremumSlip = RLWextremumSlip * factor;
        rearLeftCollider.sidewaysFriction = RLwheelFriction;

        RRwheelFriction.extremumSlip = RRWextremumSlip * factor;
        rearRightCollider.sidewaysFriction = RRwheelFriction;
      }

      // Whenever the player uses the handbrake, it means that the wheels are locked, so we set 'isTractionLocked = true'
      // and, as a consequense, the car starts to emit trails to simulate the wheel skids.
      isTractionLocked = true;
      DriftCarPS();

    }


    // This function is used to emit both the particle systems of the tires' smoke and the trail renderers of the tire skids
    // depending on the value of the bool variables 'isDrifting' and 'isTractionLocked'.
    public void DriftCarPS(){

      if(useEffects){
            try{
                if (isGrounded){
                    if (isDrifting){
                        RLWParticleSystem.Play();
                        RRWParticleSystem.Play();
                    }

                    else{
                        RLWParticleSystem.Stop();
                        RRWParticleSystem.Stop();
                    }
                }
                else{
                    RLWParticleSystem.Stop();
                    RRWParticleSystem.Stop();
                }

            }
              catch(Exception ex){
              Debug.LogWarning(ex);
            }

            try{
                if(isGrounded && (isTractionLocked || Mathf.Abs(localVelocityX) > 5f) && Mathf.Abs(carSpeed) > 12f){

                    RLWTireSkid.emitting = true;
                    RRWTireSkid.emitting = true;
                }
                else{
                    RLWTireSkid.emitting = false;
                    RRWTireSkid.emitting = false;
                }
            }
            catch(Exception ex){
                Debug.LogWarning(ex);
            }
        }
        else if(!useEffects){
        if(RLWParticleSystem != null){
          RLWParticleSystem.Stop();
        }
        if(RRWParticleSystem != null){
          RRWParticleSystem.Stop();
        }
        if(RLWTireSkid != null){
          RLWTireSkid.emitting = false;
        }
        if(RRWTireSkid != null){
          RRWTireSkid.emitting = false;
        }
      }

    }

    // This function is used to recover the traction of the car when the user has stopped using the car's handbrake.
    public void RecoverTraction(){
      isTractionLocked = false;
      driftingAxis = driftingAxis - (Time.deltaTime );
      if(driftingAxis < 0f){
        driftingAxis = 0f;
      }
      //If the 'driftingAxis' value is not 0f, it means that the wheels have not recovered their traction.
      //We are going to continue decreasing the sideways friction of the wheels until we reach the initial
      // car's grip.
      if(FLwheelFriction.extremumSlip > FLWextremumSlip){
        FLwheelFriction.extremumSlip = FLWextremumSlip * activeHandbrakeDriftMultiplier * driftingAxis;
        frontLeftCollider.sidewaysFriction = FLwheelFriction;

        FRwheelFriction.extremumSlip = FRWextremumSlip * activeHandbrakeDriftMultiplier * driftingAxis;
        frontRightCollider.sidewaysFriction = FRwheelFriction;

        RLwheelFriction.extremumSlip = RLWextremumSlip * activeHandbrakeDriftMultiplier * driftingAxis;
        rearLeftCollider.sidewaysFriction = RLwheelFriction;

        RRwheelFriction.extremumSlip = RRWextremumSlip *    activeHandbrakeDriftMultiplier  * driftingAxis;
        rearRightCollider.sidewaysFriction = RRwheelFriction;

        Invoke("RecoverTraction", Time.deltaTime);

      }else if (FLwheelFriction.extremumSlip < FLWextremumSlip){
        FLwheelFriction.extremumSlip = FLWextremumSlip;
        frontLeftCollider.sidewaysFriction = FLwheelFriction;

        FRwheelFriction.extremumSlip = FRWextremumSlip;
        frontRightCollider.sidewaysFriction = FRwheelFriction;

        RLwheelFriction.extremumSlip = RLWextremumSlip;
        rearLeftCollider.sidewaysFriction = RLwheelFriction;

        RRwheelFriction.extremumSlip = RRWextremumSlip;
        rearRightCollider.sidewaysFriction = RRwheelFriction;

        driftingAxis = 0f;
      }
    }

    //check if any of the wheel is collided with thes ground, set isGrounded to true
    //mb use events ?
    public void GroundChecker()
    {
      WheelHit hit;
      bool newIsGrounded = frontLeftCollider.GetGroundHit(out hit) || frontRightCollider.GetGroundHit(out hit) || rearLeftCollider.GetGroundHit(out hit) || rearRightCollider.GetGroundHit(out hit); 
      if(!isGrounded && newIsGrounded){
        hitGround?.Invoke();
      }
        isGrounded = newIsGrounded;
    }

    public void GravityModifier()
    {
        if (!isGrounded)
        { 
            float airGravityModifier = (-1 * activeGravityMultiplier) * Time.deltaTime;
            GetComponent<PredictedRigidbody>().predictedRigidbody.AddForce(0, airGravityModifier, 0, ForceMode.Acceleration);
            ////Debug.Log("Current Gravity Applied: " + airGravityModifier);
        }
    }


    public float CalculateHitForce() {

      return carSpeed * car.GetBaseDamageMultiplier() * Time.deltaTime;

    }

    public float GetCarSpeed() {
        return carSpeed;
    }

    public void SetActiveBodyMassCenterY(Vector3 activeBodyMassCenterY) {
        GetComponent<PredictedRigidbody>().predictedRigidbody.centerOfMass = activeBodyMassCenterY;
    }
}
