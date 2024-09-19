using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;
public class PlayerInputHandler : MonoBehaviour
{
   // private PlayerControls controls; 
    [SerializeField] private PrometeoCarController carController; 
    [SerializeField] private PickUpManager pickUpManager;
    private bool ahh = false; 

    private void Awake(){
        // initialise controls 
        //controls = new PlayerControls();
    }
    private void OnEnable(){
        //controls.Enable();
    } 

    private void OnDisable(){
        //controls.Disable();
    } 

    // Start is called before the first frame update
    void Start()
    {
        
        //carController = GetComponent<PrometeoCarController>();
        //pickUpManager = GetComponent<PickUpManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("kill me: " + ahh);

        //carController. = ahh;
    }

    public void SetCarController(PrometeoCarController cc){

        carController = cc; 
        
        Debug.Log("Car: " + carController.gameObject.name);

    }

    public void SetPickupManager(PickUpManager pm){
        pickUpManager = pm; 
    }

    public void OnReloadLevel(CallbackContext context)
    {
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }


    public void OnForward(CallbackContext context)
    {

        //WTFFFF
        // try invoking C# events instead to fix issue
        Debug.Log("aHHHHHHH");
            ahh = true;
        
        if(carController)  
        { 
            //Debug.Log("Move Forward");
            //carController.isMovingForward = context.ReadValueAsButton();

            //Debug.Log("Move Forward: " + carController.isMovingForward);
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
