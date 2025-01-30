using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Mirror;
using Mirror.BouncyCastle.Security;
using Mirror.Examples.Basic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;



public class PlayerObjectController : NetworkBehaviour
{
    public GameObject PlayerVehicle {get; set;}
    public GameObject PlayerVehiclePrefab;
    public PlayerInput Input { get; set; }
    public PlayerInputHandler InputHandler { get; set; }
    public int PlayerIndex {get; set;}
    public int Score {get; set;}

    public Camera PlayerCam {get; set;}

    public bool IsReady { get; set; }
    public VehicleUIController UIController { get; set; }
    public GameObject VehicleSelectCanvas;

    private CustomNetworkManager manager;

    private CustomNetworkManager Manager{
        get { 
                if(manager != null){
                    return manager; 
                }
                return manager = CustomNetworkManager.singleton as CustomNetworkManager;
            }
    }


    // Is Online Flag 
    public bool isOnline = false; 
    // is Testing Flag
    public bool isTesting = false; 

    // Player Setup Values
    [SerializeField] private List<LayerMask> playerLayers; 
    private Transform Spawn;

       void OnEnable()
    {
        SetupMenuController.vehicleSelected += SelectVehicle; 
        LevelManager.ArenaLevelEnded += OnLevelEnded;
        LevelManager.ArenaLevelStarted += EnableVehicleControls;
        SceneManager.sceneLoaded += OnLevelLoaded;

    }

    void OnDisable()
    {
        SetupMenuController.vehicleSelected -= SelectVehicle;  
        LevelManager.ArenaLevelEnded -= OnLevelEnded;
        LevelManager.ArenaLevelStarted -= EnableVehicleControls;    
        SceneManager.sceneLoaded -= OnLevelLoaded;

    }

    // Start is called before the first frame update
    void Start()
    {
        Input = GetComponent<PlayerInput>();
        InputHandler = GetComponent<PlayerInputHandler>();

        if(isOnline)
        {
            // Set Player ID
            PlayerIndex = GetComponent<NetworkPlayerController>().playerIDNumber - 1;
            // Enable local players input 
            Input.enabled = isOwned;
        } 
        else 
        {
            PlayerIndex = GetComponent<PlayerInput>().playerIndex;       
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(isOnline)
        {  
            if(SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 3 || SceneManager.GetActiveScene().buildIndex == 4){
                // when in game scene enable all player visuals and scripts
                if(PlayerVehicle.activeSelf == false)
                {
                    Debug.Log("Set Position");
                    PlayerVehicle.SetActive(true);
                    SetPosition();
                    SetPlayerLayers();
                }
            }
        }*/
    }
    private void OnLevelLoaded(Scene scene, LoadSceneMode mode){
        if(scene.name == "TestingScene")
        {
            SpawnVehicle();
        }

        // when loading into the selection menu
        if(scene.name == "VehicleSelection")
        {
            if(isOnline)
            {
                // Enable Selection Menu
                VehicleSelectCanvas.SetActive(true);
            }
            SetPosition();
            SetPlayerLayers();
        }
        // When loading into an arena scene
        if(scene.buildIndex == 2 || scene.buildIndex == 3 || scene.buildIndex == 4)
        {
            // spawn player vehicle
            SpawnVehicle();
        } 
    }

    private void OnLevelEnded()
    {
        Score = (int)GetComponentInChildren<ScoreKeeper>().GetScore();
        Destroy(PlayerVehicle);
        PlayerVehicle = null;
        PlayerCam.enabled = false;

    }

    private void SpawnVehicle()
    {
       // Destroy Unneeded components
        Destroy(GetComponentInChildren<Canvas>().gameObject);

        // spawn vehicle from player config as child of player config
        LevelManager lvlManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        Spawn =  lvlManager.GetSpawnPos();
        if(isOnline) 
        {
            PlayerVehicle = Manager.SpawnPlayerVehicle(PlayerVehiclePrefab, Spawn, transform, connectionToClient);
        }
        else
        {
            PlayerVehicle = Instantiate(PlayerVehiclePrefab, Spawn.position, Spawn.rotation, transform);
        }
        
        // get UI controller for each vehicle
        UIController = PlayerVehicle.GetComponentInChildren<VehicleUIController>();
        // disable vehicle controls initially if not testing
        UIController.startTimer.SetActive(!isTesting);

        // find car controller, pickup manager and camera input handler and hand them to the player input handler
        CarController car = PlayerVehicle.GetComponent<CarController>();
        car.enabled = isTesting;

        //initialise other input handler components
        InputHandler.SetCarController(car);
        InputHandler.PlayerModel = PlayerVehicle;
        InputHandler.SetPickupManager(PlayerVehicle.GetComponent<PickUpManager>());
        InputHandler.SetAbilityManager(PlayerVehicle.GetComponent<AbilityManager>());
        InputHandler.SetCameraInputHandler(PlayerVehicle.GetComponentInChildren<CameraInputHandler>());
        // disable camera shake
        //InputHandler.GetFreelook().GetComponent<CinemachineImpulseListener>().enabled = false;
        
        // set vehicle canvas to apply to player camera 
        Debug.Log("Canvas: " + PlayerVehicle.GetComponentInChildren<Canvas>());
        Debug.Log("Camera: " + PlayerCam);

        PlayerVehicle.GetComponentInChildren<Canvas>().worldCamera = PlayerCam;
        SetPlayerLayers(); 
    }

    private void SetPosition(){
        LevelManager lvlManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        Spawn =  lvlManager.GetSpawnPos();
        Debug.Log("Spawn: " + Spawn);
        transform.SetPositionAndRotation(Spawn.position, Spawn.rotation);
    }

    public void SelectVehicle(int index, GameObject vehicle)
    {
        if(index != PlayerIndex) { return; }
        PlayerVehiclePrefab = vehicle;
        // Set the vehicle type based on the vehicle name
        CarController carController = PlayerVehiclePrefab.GetComponent<CarController>();
        if (carController != null){
            VehicleType vehicleType = GetVehicleType(vehicle.name);
            
            if (vehicle.name != null) {
                carController.SetVehicleType(vehicleType); // Set the vehicle type
            }

            else{
                Debug.LogWarning("Unknown vehicle type: " + vehicle.name);
            }
        }
    }

    private void EnableVehicleControls(){
       PlayerVehicle.GetComponent<CarController>().enabled = true;
    }


    private void SetPlayerLayers(){
        // Set the Layer and Culling Mask on this Players Camera 
        // Consider Moving This : Where should these player values be initialised? 
        int layerToAdd = (int)Mathf.Log(playerLayers[PlayerIndex], 2);
        var cullingMask = (1 << layerToAdd) | (1 << 0) | (1 << 1) | (1 << 2) | (1 << 4) | (1 << 5) | (1 << 10);
        var impulseMask = 1 << PlayerIndex;
        var colliderMask = 1 << layerToAdd;
        // Run if player camera hasn't been initialised yet
        if(!PlayerCam)
        {
            // Add Camera to Player Layer
            PlayerCam = GetComponentInChildren<Camera>();
            if(isOwned){
                PlayerCam.enabled = true;
            }
            PlayerCam.gameObject.layer = layerToAdd;
            PlayerCam.transform.SetPositionAndRotation(transform.position, transform.rotation);
            PlayerCam.cullingMask = cullingMask;
            
        }
        // run if player vehicle is set
        if(PlayerVehicle)
        {
            PlayerVehicle.layer = layerToAdd;
            // Add all vehicle colliders to player layer and have them exclude that layer from collisions
            BoxCollider[] colliders = PlayerVehicle.GetComponentsInChildren<BoxCollider>();
            foreach(BoxCollider collider in colliders){
                if(collider.gameObject.tag == "Player"){
                    collider.gameObject.layer = layerToAdd;
                    collider.excludeLayers = colliderMask;
                }
            }

           
            // Add Freelook to player layer
            CinemachineFreeLook freeLook = PlayerVehicle.GetComponentInChildren<CinemachineFreeLook>();
            freeLook.gameObject.layer = layerToAdd;
            // Add Impulse Source and Listener to Player Impulse Mask
            PlayerVehicle.GetComponent<CinemachineImpulseSource>().m_ImpulseDefinition.m_ImpulseChannel = impulseMask;
            freeLook.gameObject.GetComponent<CinemachineImpulseListener>().m_ChannelMask = impulseMask;

            // Add speed lines to player layer
            PlayerVehicle.GetComponentInChildren<SpeedLines>().gameObject.layer = layerToAdd;
        }
        
    }

    private VehicleType GetVehicleType(string vehicleName)
    {
        switch (vehicleName){
            case "VehicleStandard":
                return VehicleType.Standard;
            case "VehicleSmall":
                return VehicleType.Small;
            case "VehicleBig":
                return VehicleType.Big;
            case "VehiclePolice":
                return VehicleType.Police;
            case "VehicleLightning":
                return VehicleType.Lightning;
            case "VehicleHook":
                return VehicleType.Hook;
            case "VehicleVempire":
                return VehicleType.Vampire;
            case "VehicleSurf":
                return VehicleType.Surf;
            default:
                return VehicleType.Unknown;
        }
    }

}
