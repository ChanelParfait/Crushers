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
        Debug.Log("Forward");
        if(carController)
            carController.GoForward();
        
    }

    public void OnReverse(CallbackContext context)
    {
        Debug.Log("Forward");

        if(carController)
            carController.GoReverse();
        
    }

    public void OnTurn(CallbackContext context)
    {
        Vector2 turn = context.ReadValue<Vector2>();

        Debug.Log("X Turn: " + turn.x);
        Debug.Log("Y Turn: " + turn.y);
        if(carController){
            if(turn.x < 0){
                carController.TurnLeft();
            }
            else if(turn.x > 0){
                carController.TurnRight();
            }   
        }
        
    }

    
}
