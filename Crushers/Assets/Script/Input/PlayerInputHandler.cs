using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
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

    private void Awake(){
        // initialise controls and enable them 
        controls = GetComponent<PlayerInput>().actions;
        player = controls.FindActionMap("Player");
        player.Enable();

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

    public void SetCarController(PrometeoCarController cc)
    {
        // set car controller
        carController = cc; 
    }

    public PrometeoCarController GetCarController()
    {
        return carController;
    }

    public void SetPickupManager(PickUpManager pm){
        // set pickup manager
        pickUpManager = pm; 
    }

    public void SetCameraInputHandler(CameraInputHandler cip){
        // set camera input handler
        freelookCam = cip; 
    }

    // Input Events // 
    public void OnReloadLevel(CallbackContext context)
    {
        if(context.performed){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void OnRespawn(CallbackContext context)
    {
       if(gameObject.GetComponentInChildren<CarRespawn>()){
            if(context.performed){
                gameObject.GetComponentInChildren<CarRespawn>().Respawn();
            }
       }
    }

    public void OnForward(CallbackContext context)
    {
        if(carController)  
        { 
            carController.isMovingForward = context.ReadValueAsButton();
        }
    }

    public void OnReverse(CallbackContext context)
    {
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
            carController.SetSteeringAngle(turn);
        }
        
    }

    public void OnBrake(CallbackContext context)
    {

        if(carController){
            carController.isBraking =  context.ReadValueAsButton();
            if(context.canceled){
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
