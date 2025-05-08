using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Cinemachine;
using Mirror;
using Mirror.BouncyCastle.Security;
using Mirror.Examples.Basic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


// This class controls the player initialisation and determines behaviour between online and offline gameplay
public class PlayerObjectController : NetworkBehaviour
{
    public GameObject PlayerVehicle;
    public GameObject PlayerVehiclePrefab;
    [SyncVar (hook = nameof(UpdateSelectedVehicleIndex))] public int SelectedVehicleIndex;
    public PlayerInput Input { get; set; }
    public PlayerInputHandler InputHandler { get; set; }
    public int PlayerIndex {get; set;}
    public int Score {get; set;}

    public Camera PlayerCam;

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
    // Has the player vehicle been spawned 
    public bool playerInitialised = false;
    public bool playersSpawned = false;
    // is Testing Flag
    public bool isTesting = false; 
    // Events 
    public static UnityAction vehicleConfirmed;

    // Player Setup Values
    [SerializeField] private List<LayerMask> playerLayers; 
    private Transform Spawn;
    private float time = 0;

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
        if(isOnline){
            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            if(buildIndex == 2 || buildIndex == 3 || buildIndex == 4 ){
                if(isClient && !playerInitialised && NetworkClient.ready)
                {   
                    // initialise player 
                    if(isOwned){
                        Canvas canvas  = GetComponentInChildren<Canvas>(); 
                        if(canvas){ Destroy(canvas.gameObject);}
                        LevelManager lvlManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
                        Spawn =  lvlManager.GetSpawnPos();
                        transform.SetPositionAndRotation(Spawn.position, Spawn.rotation);
                    }
                    // set the is loaded flag 
                    SetIsLoaded();
                    playerInitialised = true; 
                }
                else if( isServer)
                {
                    // Once all players have loaded spawn their vehicles 
                    if(CheckIfAllLoaded() && !playersSpawned)
                    {
                        // if all players are ready, spawn a vehicle for each
                        // player on the server
                        //Debug.Log("Spawn Vehicle for " + PlayerIndex);
                        Server_SpawnVehicles();
                        playersSpawned = true; 
                    }
                }
                
                // time += Time.deltaTime;
                // Debug.Log("Time: " + time);
                // Debug.Log("All Ready: " + CheckAllReady());
            }
        }

        

    
    }
    public bool CheckIfAllLoaded()
    {
        bool allLoaded = true; 
        foreach(NetworkPlayerController player in Manager.GamePlayers)
        {
            if(!player.Loaded){
                allLoaded = false;
            } 
        }
        return allLoaded;
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode){
        //Debug.Log("Level Loaded");
        if(isOnline)
        {
            if(!NetworkClient.ready && isServer){
                //NetworkClient.Ready(); 
                connectionToClient.isReady = true;
            }
        }
        
        if(scene.name == "TestingScene")
        {
            SpawnVehicle();
        }

        // when loading into the selection menu
        if(scene.name == "VehicleSelection")
        {
            if(isOnline && isOwned)
            {
                Debug.Log("Enable Vehicle Canvas: " + gameObject.name);
                // Enable Selection Menu
                VehicleSelectCanvas.SetActive(true);
            }
            SetPosition();
            SetPlayerLayers();
        }
        // When loading into an arena scene
        if(scene.buildIndex == 2 || scene.buildIndex == 3 || scene.buildIndex == 4)
        {
            // only use on scene loaded for offline spawning
            if(!isOnline){
                SpawnVehicle();
            }
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
        // spawn vehicle from player config as child of player config
        LevelManager lvlManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        Spawn =  lvlManager.GetSpawnPos();
        transform.SetPositionAndRotation(Spawn.position, Spawn.rotation);
        if(isOnline) 
        {
            SpawnVehicleOnline();
        }
        else
        {
            //Debug.Log("Spawn Vehicle Offline: " + gameObject.name);
            Canvas canvas  = GetComponentInChildren<Canvas>(); 
            Destroy(canvas.gameObject);
            InitialiseVehicle(Instantiate(PlayerVehiclePrefab, Vector3.zero, Quaternion.identity, transform));
        }
    }

    private void InitialiseVehicle(GameObject playerVehicle){
        if(!PlayerVehicle) {    PlayerVehicle = playerVehicle;  }
        
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
        //InputHandler.SetPickupManager(PlayerVehicle.GetComponent<PickUpManager>());
        //InputHandler.SetAbilityManager(PlayerVehicle.GetComponent<AbilityManager>());
        InputHandler.SetCameraInputHandler(PlayerVehicle.GetComponentInChildren<CameraInputHandler>());
        InputHandler.SetVehicleUIController(PlayerVehicle.GetComponentInChildren<VehicleUIController>());
        // disable camera shake
        //InputHandler.GetFreelook().GetComponent<CinemachineImpulseListener>().enabled = false;
        
        // set vehicle canvas to apply to player camera 
        PlayerVehicle.GetComponentInChildren<Canvas>().worldCamera = GetComponentInChildren<Camera>();
        SetPlayerLayers(); 
    }


    private void SpawnVehicleOnline(){
        if(isClient && isOwned)
        {
            Canvas canvas  = GetComponentInChildren<Canvas>(); 
            if(canvas){ Destroy(canvas.gameObject);}
            CmdSpawnVehicle(transform, connectionToClient);
        }
   
    }
    

    // move these commands and rpcs to network player controller
    [Command]
    private void CmdSpawnVehicle(Transform playerTransform, NetworkConnectionToClient conn)
    {
        Debug.Log("Player " + PlayerIndex +  " Selected Vehicle Index: " + SelectedVehicleIndex);
        GameObject playerObject = Instantiate(Manager.spawnPrefabs[SelectedVehicleIndex], playerTransform.position, playerTransform.rotation, playerTransform);
        NetworkServer.Spawn(playerObject, conn);
        Debug.Log("Player Object: " + playerObject);
        RpcSpawnVehicle(playerObject, playerTransform);
    }

    // move these commands and rpcs to network player controller
    [Server]
    private void Server_SpawnVehicles()
    {
        // Spawn a vehicle for each player 
        foreach(NetworkPlayerController player in Manager.GamePlayers)
        {
            int selectedVehicleIndex = player.GetComponent<PlayerObjectController>().SelectedVehicleIndex; 
            Transform playerTransform = player.gameObject.transform; 
            GameObject playerObject = Instantiate(Manager.spawnPrefabs[selectedVehicleIndex], playerTransform.position, playerTransform.rotation, playerTransform);
            NetworkServer.Spawn(playerObject, player.connectionToClient);
            RpcSpawnVehicle(playerObject, playerTransform);
        }

    }

    [ClientRpc]
    private void RpcSpawnVehicle(GameObject playerVehicle, Transform playerTransform){
        Debug.Log("Initialise Vehicle on Client: " + PlayerIndex);
        Debug.Log("Player Vehicle ID: " + playerVehicle.GetComponent<NetworkIdentity>().netId);
        playerVehicle.transform.SetParent(playerTransform);
        //InitialiseVehicle(playerVehicle);
        CarController car = playerVehicle.GetComponent<CarController>();
        InputHandler.SetCarController(car);
        if(isOwned){
            car.enabled = true;
            playerVehicle.GetComponentInChildren<CinemachineFreeLook>().enabled = true;
            playerVehicle.GetComponent<CarRespawn>().enabled = true;
            SetPlayerLayers(); 
        }
        
        
    }
    

    public void SetPosition(){
        LevelManager lvlManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        Spawn =  lvlManager.GetSpawnPos();
        //Debug.Log("Spawn: " + Spawn);
        transform.SetPositionAndRotation(Spawn.position, Spawn.rotation);
    }


    [Command]
    private void CmdSelectVehicle(int index){
        Debug.Log("Command Select Vehicle");
    
        this.UpdateSelectedVehicleIndex(this.SelectedVehicleIndex, index);

    }

    public void UpdateSelectedVehicleIndex(int oldValue, int newValue){
        Debug.Log("Update Selected Vehicle");
        if(isServer)
        {
            this.SelectedVehicleIndex = newValue;
        }
    }

    public void SelectVehicle(int index, GameObject vehicle)
    {
        if(index != PlayerIndex) { return; }
        Debug.Log("Select Vehicle: " + vehicle.name);
        //Debug.Log("Get Vehicle from Manager: " + Manager.spawnPrefabs[1]);
        PlayerVehiclePrefab = vehicle;
        
        // if online pass an index to retrieve the selected vehicle from the 
        // registered spawnable prefabs list
        if(isOnline && isOwned){
            CmdSelectVehicle(vehicle.GetComponent<CarController>().GetCar().GetVehicleIndex());
        }
        else if (!isOnline){
            // Vehicle Type currently isn't used for anything and seems unnecessary 
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
        
        
    }

    public void SetIsLoaded(){
        GetComponent<NetworkPlayerController>().ChangePlayerLoaded();
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
                Debug.Log("Enable Camera: " + gameObject.name);

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
