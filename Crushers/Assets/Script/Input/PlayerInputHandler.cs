using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    private InputActionAsset controls; 
    private InputActionMap player; 
    private int playerIndex; 

    [SerializeField] private CarController carController; 
    [SerializeField] private PickUpManager pickUpManager;
    [SerializeField] private AbilityManager abilityManager;
    [SerializeField] private CameraInputHandler freelookCam; 

    private bool canJump = true;
    // Actions
    public static UnityAction<int> Pause; 


    private void Awake(){
        // initialise controls and enable them 
        PlayerInput input = GetComponent<PlayerInput>();
        controls = input.actions;
        playerIndex = input.playerIndex;
        player = controls.FindActionMap("Player");
        player.Enable();

    }

    // Start is called before the first frame update
    void Start()
    {
        // try to find vehicle components
        // only for testing individual vehicles not vehicles in conjuction with player config object
        //carController = GetComponent<PrometeoCarController>();
        //pickUpManager = GetComponent<PickUpManager>();
        //freelookCam = GetComponentInChildren<CameraInputHandler>();
    }

    public void SetCarController(CarController cc)
    {
        // set car controller
        carController = cc; 
    }

    public CarController GetCarController()
    {
        return carController;
    }

    public CameraInputHandler GetFreelook()
    {
        return freelookCam;
    }

    public void SetPickupManager(PickUpManager pm){
        // set pickup manager
        pickUpManager = pm; 
    }

    public void SetAbilityManager(AbilityManager am) {
        abilityManager = am;
    }

    public void SetCameraInputHandler(CameraInputHandler cip){
        // set camera input handler
        freelookCam = cip; 
    }

    // Input Events
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

    public void OnJump(CallbackContext context){
        if(context.performed){
            if(canJump && carController != null){
                canJump = false;
                carController.gameObject.transform.position += carController.gameObject.transform.up * 5;
                StartCoroutine(JumpCooldown(5));
            }
        }
    }

    private IEnumerator JumpCooldown(float delay){
        yield return new WaitForSeconds(delay); 
        canJump = true;
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

    public void OnUseAbility(CallbackContext context) {
        if (abilityManager) {
            abilityManager.UseAbility();
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

    public void OnPause(CallbackContext context){
        if(context.performed){
            // invoke a pause event
            Pause?.Invoke(playerIndex);
        }
    }


    public void OnHonk() {
        if (carController) {    
            carController.HonkHorn(); 
        }
    }
    
}
