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
    private InputActionAsset controls; 
    private InputActionMap player; 

    [SerializeField] private PrometeoCarController carController; 
    [SerializeField] private PickUpManager pickUpManager;
    [SerializeField] private CameraInputHandler freelookCam; 
    [SerializeField] private int playerIndex;


    private void Awake(){
        // initialise controls and enable them 
        controls = GetComponent<PlayerInput>().actions;
        player = controls.FindActionMap("Player");
        player.Enable();

    }
    private void OnEnable(){

    }
    private void OnDisable(){
        
    } 


    // Start is called before the first frame update
    void Start()
    {
        // try to find vehicle components
        // only for testing individual vehicles not vehicles in conjustion with player config object
        carController = GetComponent<PrometeoCarController>();
        pickUpManager = GetComponent<PickUpManager>();
        freelookCam = GetComponentInChildren<CameraInputHandler>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    // set player index for input handler
    // this should match the player input index
    // for debugging
    public void SetPlayerIndex(int index){
        playerIndex = index; 
    }

    public void SetCarController(PrometeoCarController cc, int index)
    {
        // set car controller
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

    public void OnForward(CallbackContext context)
    {
        //Debug.Log("Moving Forward:  " + playerIndex);

        if(carController)  
        { 
            carController.isMovingForward = context.ReadValueAsButton();
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
        // get on look value and pass it to the free look camera
        var read = context.ReadValue<Vector2>();
        if(freelookCam){
            freelookCam.horizontal = read;
        }
    }

    
}
