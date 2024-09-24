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

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;
    [SerializeField] private int leaderboardScene; 
    private List<PlayerConfiguration> playerConfigs; 
    //private List<PlayerInput> players = new List<PlayerInput>();
    [SerializeField] private List<Transform> startingPoints; 
    [SerializeField] private List<LayerMask> playerLayers; 

    // Events
    public static UnityAction LevelLoaded; 
    public static UnityAction firstPlayerJoined; 
    
    private void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }

        DontDestroyOnLoad(this);
        playerConfigs = new List<PlayerConfiguration>();
    }
   
   void OnEnable()
    {
        SetupMenuController.vehicleSelected += SetPlayerVehicle; 
        SetupMenuController.playerReady += ReadyPlayer; 
        LevelManager.LevelEnded += LoadLeaderboard;
        LevelManager.LevelStarted += EnablePlayerControls;

    }

    void OnDisable()
    {
        SetupMenuController.vehicleSelected -= SetPlayerVehicle; 
        SetupMenuController.playerReady -= ReadyPlayer;   
        LevelManager.LevelEnded -= LoadLeaderboard;
        LevelManager.LevelStarted -= EnablePlayerControls;      
    }

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return playerConfigs;
    }

    public void SetPlayerVehicle(int index, GameObject vehicle)
    {
        //Debug.Log("index: " + index + " :" + vehicle);
        //Debug.Log("player configs: " + playerConfigs.Count);

        playerConfigs[index].vehiclePrefab = vehicle;
    }

    public void ReadyPlayer(int index)
    {
        playerConfigs[index].isReady = true; 

        if( playerConfigs.All(p => p.isReady == true))
        {
            // start level
            InitialisePlayers();
        }   
    }

    public void HandlePlayerJoin(PlayerInput pi)
    {
        if(pi.playerIndex == 0){
            // invoke first player joined event 
            if(firstPlayerJoined != null) 
                firstPlayerJoined.Invoke();
        }


        if(!playerConfigs.Any(p => p.playerIndex == pi.playerIndex))
        {
            Debug.Log("Player Joined: " + pi.playerIndex);
            pi.gameObject.GetComponentInChildren<SetupMenuController>().SetPlayerIndex(pi.playerIndex);
            
            playerConfigs.Add(new PlayerConfiguration(pi));
            pi.transform.SetParent(transform);
        }
    }

    // may alter this to be more generic later
    private void InitialisePlayers(){

        int testscore = 0; 
        Debug.Log("Level initialising...");
        // disable joining once level loads / opens
        GetComponent<PlayerInputManager>().DisableJoining();

        foreach(PlayerConfiguration playerConfig in playerConfigs){
            // generic setup
            // should only run on join level
            SetupPlayer(playerConfig);

            playerConfig.score = testscore;
            testscore += 5;
            // add vehicle for each player
            // need to implement a check for if this is an arena level
            AddVehicle(playerConfig);
        }

        if(LevelLoaded != null){
            LevelLoaded.Invoke();
        }
    }

    private void LoadLeaderboard()
    {
        Debug.Log("Level Complete");
        SceneManager.LoadScene(leaderboardScene);
        SavePlayerScores();
        DestroyVehicles();
        DisableCameras();
        
        // send player configs to leaderboard controller 
    }

    private void SavePlayerScores(){
        foreach(PlayerConfiguration playerConfig in playerConfigs){
            playerConfig.score = (int) playerConfig.Input.gameObject.GetComponentInChildren<CarStats>().getScore();
        }
    }
    
    private void EnablePlayerControls(){
        foreach(PlayerConfiguration playerConfig in playerConfigs){
            playerConfig.InputHandler.GetCarController().enabled = true;
        }
    }

    private void DisableCameras(){
        foreach(PlayerConfiguration playerConfig in playerConfigs){
            playerConfig.playerCam.enabled = false;
        }
    }

    
    
    // run this code in the join menu
    private void SetupPlayer(PlayerConfiguration pi){
        pi.InputHandler.SetPlayerIndex(pi.playerIndex);
        // get camera component
        pi.playerCam = pi.Input.gameObject.GetComponentInChildren<Camera>();

        int layerToAdd = (int)Mathf.Log(playerLayers[pi.playerIndex], 2);
        var bitmask = (1 << layerToAdd) | (1 << 0) | (1 << 1) | (1 << 2) | (1 << 4) | (1 << 5);

        //set the layer
        pi.Input.gameObject.layer = layerToAdd;
        pi.playerCam.gameObject.layer = layerToAdd;
        // add the layer
        pi.playerCam.cullingMask = bitmask;

    }

    // run this code when loading into any arena level
    private void AddVehicle(PlayerConfiguration pi)
    {
        Debug.Log("Setup Player Vehicle: " + pi.playerIndex);
        // destroy setup menu
        // later setup menu may be in a separate level
        Destroy(pi.Input.gameObject.GetComponentInChildren<Canvas>().gameObject);

        // spawn vehicle from player config as child of player config
        pi.vehicleObject = Instantiate(pi.vehiclePrefab, startingPoints[pi.playerIndex].position, startingPoints[pi.playerIndex].rotation, pi.Input.gameObject.transform);
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
        
        
        // set vehicle canvas to apply to player camera 
        pi.vehicleObject.GetComponentInChildren<Canvas>().worldCamera = pi.playerCam;

        int layerToAdd = (int)Mathf.Log(playerLayers[pi.playerIndex], 2);
        pi.vehicleObject.layer = layerToAdd;
        //set the layer
        pi.vehicleObject.GetComponentInChildren<CinemachineFreeLook>().gameObject.layer = layerToAdd;
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
    public GameObject vehicleObject {get; set;}
    public VehicleUIController UIController { get; set; }

    
    public PlayerConfiguration(PlayerInput pi){
        playerIndex = pi.playerIndex;
        Input = pi;
        InputHandler = Input.GetComponent<PlayerInputHandler>();
    }

}
