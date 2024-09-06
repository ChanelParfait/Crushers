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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnForward(CallbackContext context)
    {
        //context.action.canceled

        Debug.Log("Forward Value as Button: " + context.ReadValueAsButton());


        //Debug.Log("Reverse: " + (controls.Player.Reverse.ReadValue<float>() > 0f));

            if(carController)  
            { 
                if(context.action.IsInProgress()){
                    carController.GoForward();
                } else {
                    carController.StopForward();
                }
        }
        
    }

    public void OnReverse(CallbackContext context)
    {
        Debug.Log("Reverse: " + context.ReadValueAsButton());
        if(carController)
        {
            if(context.action.IsInProgress()){
                carController.GoReverse();
            } else {
                carController.StopReverse();
            }
        }
    }

    public void OnTurn(CallbackContext context)
    {
        Vector2 turn = context.ReadValue<Vector2>();

        //Debug.Log("X Turn: " + turn.x);
        //Debug.Log("Y Turn: " + turn.y);
        if(carController){
            if(turn.x < 0){
                carController.TurnLeft();
            }
            else if(turn.x > 0){
                carController.TurnRight();
            }   
            else if(turn.x == 0){
                carController.StopTurning();
            }
        }
        
    }

    public void OnBrake(CallbackContext context)
    {
        Debug.Log("Brake: " + context.ReadValueAsButton());

        if(carController){
            if(context.action.IsInProgress()){
                carController.Handbrake();
            }
            else{
                carController.RecoverTraction();
            }
        }

        
    }

    
}
