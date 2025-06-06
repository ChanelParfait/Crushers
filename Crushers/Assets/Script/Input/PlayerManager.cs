using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Linq;
using Unity.VisualScripting;
using System;
using Random = UnityEngine.Random;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;

    private List<PlayerConfiguration> playerConfigs; 
    // Player Initialising Values
    [SerializeField] private Transform[] startingPoints; 
    [SerializeField] private List<LayerMask> playerLayers; 

    // Game / Scene Management
    [SerializeField] private int selectedMapIndex; 
    private int leaderboardScene = 5; 

    // Events
    public static UnityAction<bool> ArenaLevelLoaded; 
    public static UnityAction firstPlayerJoined; 

    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(this);
            playerConfigs = new List<PlayerConfiguration>();
            startingPoints = null;
        }        

        Debug.Log(this);
    }
   
   void OnEnable()
    {
        SetupMenuController.vehicleSelected += SetPlayerVehicle; 
        SetupMenuController.playerReady += ReadyPlayer; 
        LevelManager.ArenaLevelEnded += LoadLeaderboard;
        LevelManager.ArenaLevelStarted += EnableVehicleControls;
        SceneManager.sceneLoaded += OnLevelLoaded;
        MainMenuController.levelSelected += SaveMapSelection; 

    }

    void OnDisable()
    {
        SetupMenuController.vehicleSelected -= SetPlayerVehicle; 
        SetupMenuController.playerReady -= ReadyPlayer;   
        LevelManager.ArenaLevelEnded -= LoadLeaderboard;
        LevelManager.ArenaLevelStarted -= EnableVehicleControls;    
        SceneManager.sceneLoaded -= OnLevelLoaded;
        MainMenuController.levelSelected -= SaveMapSelection; 
    }

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return playerConfigs;
    }

    // UI Functions // 
    public void SetPlayerVehicle(int index, GameObject vehicle)
{
    //Debug.Log("index: " + index + " :" + vehicle);
    playerConfigs[index].vehiclePrefab = vehicle;

    // Set the vehicle type based on the vehicle name
    CarStats carStats = vehicle.GetComponent<CarStats>();
        if (carStats != null){
            VehicleType vehicleType = GetVehicleType(vehicle.name);
            
            if (vehicle.name != null) {
                carStats.SetVehicleType(vehicleType); // Set the vehicle type
            }

            else{
                Debug.LogWarning("Unknown vehicle type: " + vehicle.name);
            }
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
            default:
                return VehicleType.Unknown;
        }
    }




    public void ReadyPlayer(int index)
    {
        playerConfigs[index].isReady = true; 

        if( playerConfigs.All(p => p.isReady == true))
        {
            // load selected level
            SceneManager.LoadScene(selectedMapIndex);
            
        }   
    }
    
    public void SaveMapSelection(int index){
        //Debug.Log("Level Selected: " + index);
        selectedMapIndex = index;
    }

    // Level / Player Management 

    // keep track of what level we are currently in
    public void OnLevelLoaded(Scene scene, LoadSceneMode mode){
        //Debug.Log("Level Loaded: " + scene);
        // ensure joining is initially disabled
        GetComponent<PlayerInputManager>().DisableJoining();
    
        if(scene.buildIndex == 1){
            // allow joining in vehicle selection level
            GetComponent<PlayerInputManager>().EnableJoining();
        }
        
        // if scene index is an arena scene
        if(scene.buildIndex == 2 || scene.buildIndex == 3 || scene.buildIndex == 4){
            // initialise players with vehicles
            SetupArena();
        } else {
            ArenaLevelLoaded?.Invoke(false);
        }
    }

    public void DestroyConfigs(){
        if(playerConfigs != null){
            foreach(PlayerConfiguration player in playerConfigs){
                Destroy(player.Input.gameObject);
            }
        }
        playerConfigs = new List<PlayerConfiguration>();
    }


    public void HandlePlayerJoin(PlayerInput pi)
    {
        
        if(pi.playerIndex == 0){
            startingPoints = GameObject.FindGameObjectWithTag("Spawns").GetComponentsInChildren<Transform>();

            // invoke first player joined event 
            firstPlayerJoined?.Invoke();
        }

        if(!playerConfigs.Any(p => p.playerIndex == pi.playerIndex))
        {
            // Setup Player
            //Debug.Log("Player Joined: " + pi.playerIndex);
            // create configuration object for player 
            PlayerConfiguration playerConfig = new PlayerConfiguration(pi);
            SetupPlayerCamera(playerConfig); 
            playerConfigs.Add(playerConfig);
            pi.transform.SetParent(transform);
            pi.transform.position = new Vector3(0,0,0);
        }
    }

    public void OnPlayerLeft(PlayerInput pi){
        //Debug.Log("Player " + pi.playerIndex + " Left");
    }

    private void SetupArena(){
        //Debug.Log("Arena initialising...");
        // find starting points
        startingPoints = GameObject.FindGameObjectWithTag("Spawns").GetComponentsInChildren<Transform>();

        // add vehicle for each player
        foreach(PlayerConfiguration playerConfig in playerConfigs){
            AddVehicle(playerConfig);
        }
        // invoke arena level loaded event 

        ArenaLevelLoaded?.Invoke(true);

    }

    
    private void AddVehicle(PlayerConfiguration pi)
    {
        Debug.Log("Setup Player Vehicle: " + pi.playerIndex);
        // destroy setup menu
        // later setup menu may be in a separate level
        // move this to set up menu controller? 
        Destroy(pi.Input.gameObject.GetComponentInChildren<Canvas>().gameObject);
        
        // spawn vehicle from player config as child of player config
        pi.vehicleObject = Instantiate(pi.vehiclePrefab, startingPoints[pi.playerIndex + 1].position, startingPoints[pi.playerIndex + 1].rotation, pi.Input.gameObject.transform);
        
        // get UI controller for each vehicle
        pi.UIController = pi.vehicleObject.GetComponentInChildren<VehicleUIController>();

        // find car controller, pickup manager and camera input handler and hand them to the player input handler
        PrometeoCarController car = pi.Input.gameObject.GetComponentInChildren<PrometeoCarController>();
        pi.InputHandler.SetCarController(car);
        // disable vehicle controls initially
        car.enabled = false;
        


        //initialise other input handler components
        pi.InputHandler.SetPickupManager(pi.Input.gameObject.GetComponentInChildren<PickUpManager>());
        pi.InputHandler.SetCameraInputHandler(pi.Input.gameObject.GetComponentInChildren<CameraInputHandler>());
        // disable camera shake
        pi.InputHandler.GetFreelook().GetComponent<CinemachineImpulseListener>().enabled = false;
        
        // set vehicle canvas to apply to player camera 
        pi.vehicleObject.GetComponentInChildren<Canvas>().worldCamera = pi.playerCam;
        SetupPlayerCamera(pi); 
        
    }
    
    private void SetupPlayerCamera(PlayerConfiguration pi){

        int layerToAdd = (int)Mathf.Log(playerLayers[pi.playerIndex], 2);
        if(pi.playerCam == null){
            // get camera component
            pi.playerCam = pi.Input.gameObject.GetComponentInChildren<Camera>();
            pi.playerCam.transform.position = startingPoints[pi.playerIndex + 1].position;
            pi.playerCam.transform.rotation = startingPoints[pi.playerIndex + 1].rotation;

            var bitmask = (1 << layerToAdd) | (1 << 0) | (1 << 1) | (1 << 2) | (1 << 4) | (1 << 5) | (1 << 10);

            // set the layer
            pi.Input.gameObject.layer = layerToAdd;
            pi.playerCam.gameObject.layer = layerToAdd;
            // add the layer
            pi.playerCam.cullingMask = bitmask;
        }
        // put vehicle and free look camera on player layer 
        if(pi.vehicleObject != null){
            pi.vehicleObject.layer = layerToAdd;
            //set the layer
            pi.vehicleObject.GetComponentInChildren<CinemachineFreeLook>().gameObject.layer = layerToAdd;
            // find speed lines and put them on the player layer
            pi.vehicleObject.GetComponentInChildren<SpeedLines>().gameObject.layer = layerToAdd;
        }

    }

    // Level End Event
    private void LoadLeaderboard()
    {
        Debug.Log("Level Complete");
        SceneManager.LoadScene(leaderboardScene);
        SavePlayerScores();
        DestroyVehicles();
        DisableCameras();
        // send player configs to leaderboard controller 
    }

    // Helper Functions
    private void SavePlayerScores(){
        foreach(PlayerConfiguration playerConfig in playerConfigs){
            playerConfig.score = (int) playerConfig.Input.gameObject.GetComponentInChildren<CarStats>().GetScore();
        }
    }
    
    private void EnableVehicleControls(){
        foreach(PlayerConfiguration playerConfig in playerConfigs){
            playerConfig.InputHandler.GetCarController().enabled = true;
            playerConfig.InputHandler.GetFreelook().GetComponent<CinemachineImpulseListener>().enabled = true;
        }
    }

    private void DisableCameras(){
        foreach(PlayerConfiguration playerConfig in playerConfigs){
            playerConfig.playerCam.enabled = false;
        }
    }

    private void SetMaterial(PlayerConfiguration pi){
        //MeshRenderer[] materials =  pi.vehicleObject.
        //foreach(Material material in materials){
            
        //}
    }

    private void DestroyVehicles(){
        foreach(PlayerConfiguration playerConfig in playerConfigs){
            if(playerConfig.vehicleObject){
                Destroy(playerConfig.vehicleObject);
                playerConfig.vehicleObject = null;  
                playerConfig.UIController = null;  
            }       
        }
    }
}


public class PlayerConfiguration
{
    public PlayerInput Input { get; set; }
    public PlayerInputHandler InputHandler { get; set; }


    public int playerIndex {get; set;}
    public int score {get; set;}

    public Camera playerCam {get; set;}

    // can store configuration values here 
    public bool isReady { get; set; }
    public GameObject vehiclePrefab {get; set;}
    public Material material {get; set;}
    public GameObject vehicleObject {get; set;}
    public VehicleUIController UIController { get; set; }

    
    public PlayerConfiguration(PlayerInput pi){
        playerIndex = pi.playerIndex;
        Input = pi;
        InputHandler = Input.GetComponent<PlayerInputHandler>();
    }

}
