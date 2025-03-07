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


// This class controls the player initialisation and determines behaviour between online and offline gameplay
public class PlayerObjectController : NetworkBehaviour
{
    public GameObject PlayerVehicle {get; set;}
    public GameObject PlayerVehiclePrefab;
    public PlayerInput Input { get; set; }
    public PlayerInputHandler InputHandler { get; set; }
    public int PlayerIndex {get; set;}
    public int Score {get; set;}

    public Camera PlayerCam {get; set;}

    public bool VehicleConfirmed { get; set; }
    public VehicleUIController UIController { get; set; }
    // Vehicle Canvas Should be Disabled on Start
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
    // Events 
    public static UnityAction vehicleConfirmed;

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
            //Debug.Log("Player Index: " +  PlayerIndex);
            VehicleSelectCanvas.SetActive(true);
        }
    }

    void Update()
    {
        //Debug.Log("Client Ready: " + connectionToClient.isReady);
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode){
        //Debug.Log("Level Loaded");
        if(isOnline)
        {
            connectionToClient.isReady = true;
            NetworkClient.Ready();
        }
        
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
            //Debug.Log("Arena Level Loaded");

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

    /*
    private IEnumerator WaitTillReady()
    {
        while (!connectionToClient.isReady) {
            yield return new WaitForSeconds(0.25f);
        }
        CmdSpawnVehicle(PlayerVehiclePrefab, Spawn, transform, connectionToClient);
    }
    */

    private void SpawnVehicle()
    {
        // spawn vehicle from player config as child of player config
        LevelManager lvlManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        Spawn =  lvlManager.GetSpawnPos();
        if(isOnline) 
        {
            /*Debug.Log("Spawn Vehicle Online: " + gameObject.name);
            if(connectionToClient.isReady)
            {
                CmdSpawnVehicle(PlayerVehiclePrefab, Spawn, transform, connectionToClient);
            } else {
                StartCoroutine(WaitTillReady());
            }*/
        }
        else
        {
            //Debug.Log("Spawn Vehicle Offline: " + gameObject.name);
            Destroy(GetComponentInChildren<Canvas>().gameObject);

            PlayerVehicle = Instantiate(PlayerVehiclePrefab, Spawn.position, Spawn.rotation, transform);
        
            //PlayerVehicle = GetComponentInChildren<CarController>().gameObject;
            
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
            InputHandler.SetVehicleUIController(PlayerVehicle.GetComponentInChildren<VehicleUIController>());
            // disable camera shake
            //InputHandler.GetFreelook().GetComponent<CinemachineImpulseListener>().enabled = false;
            
            // set vehicle canvas to apply to player camera 
            //Debug.Log("Canvas: " + PlayerVehicle.GetComponentInChildren<Canvas>());
            //Debug.Log("Camera: " + PlayerCam);

            PlayerVehicle.GetComponentInChildren<Canvas>().worldCamera = GetComponentInChildren<Camera>();
            SetPlayerLayers(); 
        }
    }
    /*
    [Command]
    private void CmdSpawnVehicle(GameObject prefab, Transform spawn, Transform parent, NetworkConnectionToClient conn)
    {
        GameObject oldPlayer = conn.identity.gameObject;
        
        // Instantiate the new player object and broadcast to clients
        // Include true for keepAuthority paramater to prevent ownership change
        GameObject player = Instantiate(prefab, spawn.position, spawn.rotation, parent);
        NetworkServer.ReplacePlayerForConnection(conn, player, true);
        player.GetComponent<PlayerObjectController>().PlayerVehicle = player;
        // Remove the previous player object that's now been replaced
        // Delay is required to allow replacement to complete.
        Destroy(oldPlayer, 0.1f);
    }*/
    

    public void SetPosition(){
        LevelManager lvlManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        Spawn =  lvlManager.GetSpawnPos();
        //Debug.Log("Spawn: " + Spawn);
        transform.SetPositionAndRotation(Spawn.position, Spawn.rotation);
    }

    public void SelectVehicle(int index, GameObject vehicle)
    {
        if(index != PlayerIndex) { return; }
        //Debug.Log("Select Vehicle");

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

    public void SetVehicleConfirmed()
    {
        //Debug.Log("Set Vehicle Confirmed ");

        if(isOnline){
            GetComponent<NetworkPlayerController>().ChangeVehicleConfirmed();
        } 
        else 
        {
            VehicleConfirmed = true;
            vehicleConfirmed?.Invoke();

        }
    }

    private void EnableVehicleControls(){
        if(isOnline){ return; }
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
        if(PlayerCam == null)
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
