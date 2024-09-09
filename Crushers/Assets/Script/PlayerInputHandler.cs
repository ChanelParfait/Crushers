using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
public class PlayerInputHandler : MonoBehaviour
{
    private PlayerControls controls; 
    private PrometeoCarController carController; 
    private PickUpManager pickUpManager;


    private void Awake(){
        // initialise controls 
        controls = new PlayerControls();
    }
    private void OnEnable(){
        controls.Enable();
    } 

    private void OnDisable(){
        controls.Disable();
    } 

    // Start is called before the first frame update
    void Start()
    {
        
        carController = GetComponent<PrometeoCarController>();
        pickUpManager = GetComponent<PickUpManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnForward(CallbackContext context)
    {
        //Debug.Log("Move Forward");

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
                Debug.Log("RecoverTraction");
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

    
}
