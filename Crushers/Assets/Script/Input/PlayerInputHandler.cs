using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : NetworkBehaviour
{
    private InputActionAsset controls; 
    private InputActionMap player; 

    private PlayerInput input;
    private int playerIndex; 
    public GameObject PlayerModel;


    [SerializeField] private CarController carController; 
    [SerializeField] private PickUpManager pickUpManager;
    [SerializeField] private AbilityManager abilityManager;
    [SerializeField] private CameraInputHandler freelookCam; 

    private bool canJump = true;
    // Actions
    public static UnityAction<int> Pause; 


    private void Awake(){
        // initialise controls and enable them 
        input = GetComponent<PlayerInput>();
        controls = input.actions;
        playerIndex = input.playerIndex;
        Debug.Log("Owned: " + isOwned);

        
        player = controls.FindActionMap("Player");
        player.Enable();

    }

    // Start is called before the first frame update
    void Start()
    {
        if(!isOwned)
        {
            input.enabled = false;
        }
        // try to find vehicle components
        // only for testing individual vehicles not vehicles in conjuction with player config object
        //carController = GetComponent<PrometeoCarController>();
        //pickUpManager = GetComponent<PickUpManager>();
        //freelookCam = GetComponentInChildren<CameraInputHandler>();
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 3 || SceneManager.GetActiveScene().buildIndex == 4){
            // when in game scene enable all player visuals and scripts
            if(PlayerModel.activeSelf == false)
            {
                Debug.Log("Set Position");
                SetPosition();
                PlayerModel.SetActive(true);
            }
            
        }
    }
    
    // Set player to a Random Position within -5, 0, -5 and 5, 0, 5 
    public void SetPosition(){
        transform.position = new Vector3(167,18,77);
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
        if(canJump && carController && isOwned){
            canJump = false;
            carController.gameObject.transform.position += carController.gameObject.transform.up * 5;
            StartCoroutine(JumpCooldown(5));
        }
    }

    private IEnumerator JumpCooldown(float delay){
        yield return new WaitForSeconds(delay); 
        canJump = true;
    }

    public void OnForward(CallbackContext context)
    {
        Debug.Log("Pressing W");
        if(carController && isOwned)
        {
            Debug.Log("Moving Forward");

            carController.isMovingForward = context.ReadValueAsButton();
        }
    }

    public void OnReverse(CallbackContext context)
    {
        if(carController && isOwned)
        {
            carController.isReversing = context.ReadValueAsButton();
        }
    }

    public void OnTurn(CallbackContext context)
    {
        if(carController && isOwned)
        {
            Vector2 turn = context.ReadValue<Vector2>();
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
