using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    //[SerializeField] private PlayerControls controls; 
    private InputActionAsset controls; 
    private InputActionMap player; 

    [SerializeField] private PrometeoCarController carController; 
    [SerializeField] private PickUpManager pickUpManager;
    [SerializeField] private CameraInputHandler freelookCam; 
    [SerializeField] private int playerIndex;






    private void Awake(){
        // initialise controls 
        controls = GetComponent<PlayerInput>().actions;
        player = controls.FindActionMap("Player");
        player.Enable();

    }
    private void OnEnable(){
        // enable player controls and set callbacks to refer to this object
        // ensure prefab is set to Invoke C# Events
        //controls.SetCallbacks(this);


        //player.FindAction("Forward").started += OnForward;
        //player.FindAction("Forward").canceled += OnStopForward;
        
        //player.FindAction("Look").started += OnLook;

        //Debug.Log("Action:  " + player.FindAction("Forward"));
        //Debug.Log("Is Pressed:  " + player.FindAction("Forward").IsPressed());



    }

    private void OnStopForward(CallbackContext context)
    {
        if(carController)  
        { 
            carController.isMovingForward = false;
        }
    
    }

    private void OnDisable(){
        //controls.Player.Disable();
        
    } 

    // Start is called before the first frame update
    void Start()
    {
        
        carController = GetComponent<PrometeoCarController>();
        pickUpManager = GetComponent<PickUpManager>();
        freelookCam = GetComponentInChildren<CameraInputHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void SetPlayerIndex(int index){
        playerIndex = index; 
    }

    public void SetCarController(PrometeoCarController cc, int index)
    {
        // set car controller object
        carController = cc; 
    }

    public void SetPickupManager(PickUpManager pm){
        // set pickup manager
        pickUpManager = pm; 
    }

    public void SetCameraInputHandler(CameraInputHandler cip){
        // set camera input handler
        freelookCam = cip; 
    }

    public void OnReloadLevel(CallbackContext context)
    {
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void OnMove()
    {
        Debug.Log("Moving:  " + playerIndex);

        if(carController)  
        { 
            //Debug.Log("Car: " + carController.GetPlayerIndex());
            //Debug.Log("Forward Control: " + controls.GamepadScheme);
            //carController.isMovingForward = context.ReadValueAsButton();
            //carController.isMovingForward = true;
        }
    }
    public void OnForward(CallbackContext context)
    {
        Debug.Log("Moving:  " + playerIndex);

        if(carController)  
        { 
            //Debug.Log("Car: " + carController.GetPlayerIndex());
            //Debug.Log("Forward Control: " + controls.GamepadScheme);
            carController.isMovingForward = context.ReadValueAsButton();
            //carController.isMovingForward = true;
        }
    }

    public void OnReverse(CallbackContext context)
    {
        //Debug.Log("Reversing");
        if(carController)
        {
            carController.isReversing = context.ReadValueAsButton();
        }
    }

    public void OnTurn(CallbackContext context)
    {
        Vector2 turn = context.ReadValue<Vector2>();

        if(carController)
        {
            if(turn.x < 0){
                carController.TurnLeft();
            }
            else if(turn.x > 0){
                carController.TurnRight();
            }   
            else if(turn.x == 0){
                carController.isTurning = false;
            }
        }
        
    }

    public void OnBrake(CallbackContext context)
    {

        if(carController){
            carController.isBraking =  context.ReadValueAsButton();
            if(!context.action.IsInProgress()){
                //Debug.Log("RecoverTraction");
                carController.RecoverTraction();
            }
        }

    }

    public void OnUseItem(CallbackContext context)
    {

        if(pickUpManager){
            pickUpManager.useItem = context.ReadValueAsButton();
        }

        
    }

    public void OnLook(CallbackContext context)
    {
        //Debug.Log("Look");
        var read = context.ReadValue<Vector2>();
        if(freelookCam){
            freelookCam.horizontal = read;
        }
        //pi.Input.gameObject.GetComponentInChildren<CameraInputHandler>().horizontal = pi.Input.actions.FindAction("Look");
    }

    
}
