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
    private PlayerInput input;
    private InputActionAsset inputAsset; 
    private InputActionMap player; 
    // Temporary Testing Value
    public GameObject PlayerModel;


    [SerializeField] private CarController carController; 
    [SerializeField] private PickUpManager pickUpManager;
    [SerializeField] private AbilityManager abilityManager;
    [SerializeField] private CameraInputHandler freelookCam; 
    [SerializeField] private VehicleUIController vehicleUIController; 

    private bool canJump = true;
    // Actions
    public static UnityAction Pause; 
    // Flag for Online / Offline Use
    // Set in Prefabs
    public bool isOnline = false; 



    private void Awake(){
        // initialise controls and enable them 
        input = GetComponent<PlayerInput>(); 
        inputAsset = input.actions;
        player = inputAsset.FindActionMap("Player");
    }

    private void OnEnable()
    {
        // add listeners to all input actions
        player.FindAction("Forward").started += OnForward;
        player.FindAction("Forward").canceled += OnForward;
        player.FindAction("Reverse").started += OnReverse;
        player.FindAction("Reverse").canceled += OnReverse;
        player.FindAction("Turn").started += OnTurn;
        player.FindAction("Turn").performed += OnTurn;
        player.FindAction("Turn").canceled += OnTurn;
        player.FindAction("Look").started += OnLook;
        player.FindAction("Look").performed += OnLook;
        player.FindAction("Look").canceled += OnLook;
        player.FindAction("Brake").started += OnBrake;
        player.FindAction("Brake").canceled += OnBrake;
        player.FindAction("UseItem").started += OnUseItem;
        player.FindAction("UseItem").canceled += OnUseItem;
        player.FindAction("Jump").performed += OnJump;
        player.FindAction("Honk").performed += OnHonk;
        player.FindAction("Pause").performed += OnPause;
        player.FindAction("ToggleControls").performed += OnToggleUI;
        //player.FindAction("UseAbility").performed += OnUseAbility;

        player.Enable();
    }

    private void OnDisable()
    {
        player.FindAction("Forward").started -= OnForward;
        player.FindAction("Forward").canceled -= OnForward;
        player.FindAction("Reverse").started -= OnReverse;
        player.FindAction("Reverse").canceled -= OnReverse;
        player.FindAction("Turn").started -= OnTurn;
        player.FindAction("Turn").canceled -= OnTurn;
        player.FindAction("Look").started -= OnLook;
        player.FindAction("Look").canceled -= OnLook;
        player.FindAction("Brake").started -= OnBrake;
        player.FindAction("Brake").canceled -= OnBrake;
        player.FindAction("UseItem").started -= OnUseItem;
        player.FindAction("UseItem").canceled -= OnUseItem;
        player.FindAction("Jump").performed -= OnJump;
        player.FindAction("Honk").performed -= OnHonk;
        player.FindAction("Pause").performed -= OnPause;
        player.FindAction("ToggleControls").performed -= OnToggleUI;
        //player.FindAction("UseAbility").performed -= OnUseAbility;



        player.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        // try to find vehicle components
        // only for testing individual vehicles not vehicles in conjuction with player config object
        if(PlayerModel)
        {
            carController = PlayerModel.GetComponent<CarController>();
            pickUpManager = PlayerModel.GetComponent<PickUpManager>();
            freelookCam = PlayerModel.GetComponentInChildren<CameraInputHandler>();
        }
    }

    private void Update()
    {
        if(isOnline)
        {
            if(SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 3 || SceneManager.GetActiveScene().buildIndex == 4){
                // when in game scene enable all player visuals and scripts
                /*if(PlayerModel.activeSelf == false)
                {
                    player.Enable();
                }*/
                
            }
        }
    }
    

    // Helper Functions
    private bool IsInputValid()
    {
        // if in online mode check if this object belongs to the client
        if(isOnline)
        {
            return isOwned;
        } 
        else 
        {
            // else return true
            return true;
        }
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

    public void SetVehicleUIController(VehicleUIController ui){
        // set camera input handler
        vehicleUIController = ui;
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
       if(gameObject.GetComponentInChildren<CarRespawn>() && IsInputValid()){
            if(context.performed){
                gameObject.GetComponentInChildren<CarRespawn>().Respawn();
            }
       }
    }

    public void OnJump(CallbackContext context){
        if(canJump && carController && IsInputValid()){
            //Debug.Log("Jump");

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
        Debug.Log("Player: " + transform.gameObject);
        
        //Debug.Log("Is Owned: " + IsInputValid());
        if (carController)
        {
            if (isOnline)
            {
                if (isOwned)
                {
                    Debug.Log("Moving Forward");
                    Forward_Online(carController.gameObject, context.ReadValueAsButton());
                }
            }
            else
            {
                carController.isMovingForward = context.ReadValueAsButton();
            }
        }
    }
    
    [Command]
    private void Forward_Online(GameObject carController, bool isMovingForward){
        carController.GetComponent<CarController>().isMovingForward = isMovingForward;
    }

    public void OnReverse(CallbackContext context)
    {
        if (carController && IsInputValid())
        {
            carController.isReversing = context.ReadValueAsButton();
        }
    }

    public void OnTurn(CallbackContext context)
    {
        Vector2 turn = context.ReadValue<Vector2>();
        //Debug.Log("Turn: " + turn);
        if(carController && IsInputValid())
        {
            carController.SetSteeringAngle(turn);
        }
        
    }

    public void OnBrake(CallbackContext context)
    {

        if(carController && IsInputValid()){
            carController.isBraking =  context.ReadValueAsButton();
            if(context.canceled){
                //Debug.Log("RecoverTraction");
                carController.RecoverTraction();
            }
        }

    }

    public void OnUseItem(CallbackContext context)
    {
        if(pickUpManager && IsInputValid()){
            pickUpManager.useItem = context.ReadValueAsButton();
        }        
    }

    public void OnUseAbility(CallbackContext context) {
        if (abilityManager && IsInputValid()) {
            abilityManager.UseAbility();
        }
    }

    public void OnLook(CallbackContext context)
    {
        //Debug.Log("Look");
        // get on look value and pass it to the free look camera
        var read = context.ReadValue<Vector2>();
        if(freelookCam && IsInputValid()){
            freelookCam.horizontal = read;
        }
    }

    public void OnPause(CallbackContext context){
        if(context.performed && IsInputValid()){
            // invoke a pause event
            Pause?.Invoke();
        }
    }

    public void OnToggleUI(CallbackContext context){
        if(context.performed && IsInputValid()){
            //Debug.Log("Toggle UI"); 
            vehicleUIController.ToggleControlsPnl();
        }
    }


    public void OnHonk(CallbackContext context) {
        if (carController && IsInputValid()) {    
            //Debug.Log("Honk");

            carController.HonkHorn(); 
        }
    }
    
}
