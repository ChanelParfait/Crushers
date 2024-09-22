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
    [SerializeField] private int leaderboardScene; 
    private List<PlayerConfiguration> playerConfigs; 
    private List<PlayerInput> players = new List<PlayerInput>();
    [SerializeField] private List<Transform> startingPoints; 
    [SerializeField] private List<LayerMask> playerLayers; 

    // Events
    public static UnityAction LevelLoaded; 
    
    private void Awake()
    {
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
        playerConfigs[index].playerObject = vehicle;
    }

    public void ReadyPlayer(int index)
    {
        playerConfigs[index].isReady = true; 

        if( playerConfigs.All(p => p.isReady == true))
        {
            // start level
            InitialiseLevel();
        }   
    }

    public void HandlePlayerJoin(PlayerInput pi)
    {
        if(pi.playerIndex == 0){
            // disable main camera
            // invoke first player joined event 
            //if(firstPlayerJoined != null) 
            //    firstPlayerJoined.Invoke();
        }
        pi.gameObject.GetComponentInChildren<SetupMenuController>().SetPlayerIndex(pi.playerIndex);
            

        if(!playerConfigs.Any(p => p.playerIndex == pi.playerIndex))
        {
            Debug.Log("Player Joined: " + pi.playerIndex);
            
            playerConfigs.Add(new PlayerConfiguration(pi));
            pi.transform.SetParent(transform);
        }
    }

    private void InitialiseLevel(){
        Debug.Log("Level initialising...");
        // disable joining once level loads / opens
        GetComponent<PlayerInputManager>().DisableJoining();

        foreach(PlayerConfiguration playerConfig in playerConfigs){
            SetupVehicle(playerConfig);
        }

        if(LevelLoaded != null){
            LevelLoaded.Invoke();
        }
    }

    private void LoadLeaderboard()
    {
        Debug.Log("Level Complete");
        //SceneManager.LoadScene(leaderboardScene);
        // send player configs to leaderboard controller 
    }
    
    private void EnablePlayerControls(){
        foreach(PlayerConfiguration playerConfig in playerConfigs){
            playerConfig.InputHandler.GetCarController().enabled = true;
        }
    }
    


    public void SetupVehicle(PlayerConfiguration pi)
    {
        Debug.Log("Setup Player Vehicle: " + pi.playerIndex);

        // destroy setup menu
        Destroy(pi.Input.gameObject.GetComponentInChildren<Canvas>().gameObject);

        // spawn vehicle from player config as child of player config
        GameObject vehicle = Instantiate(pi.playerObject, startingPoints[pi.playerIndex].position, startingPoints[pi.playerIndex].rotation, pi.Input.gameObject.transform);
        pi.InputHandler.SetPlayerIndex(pi.playerIndex);

        // find car controller, pickup manager and camera input handler and hand them to the player input handler
        PrometeoCarController car = pi.Input.gameObject.GetComponentInChildren<PrometeoCarController>();
        pi.InputHandler.SetCarController(car);
        // disable vehicle controls initially
        car.enabled = false;

        pi.InputHandler.SetPickupManager(pi.Input.gameObject.GetComponentInChildren<PickUpManager>());
        pi.InputHandler.SetCameraInputHandler(pi.Input.gameObject.GetComponentInChildren<CameraInputHandler>());
        // get camera component
        Camera camera = pi.Input.gameObject.GetComponentInChildren<Camera>(); 
        // set vehicle canvas to apply to this camera 
        vehicle.gameObject.GetComponentInChildren<Canvas>().worldCamera = camera;

        int layerToAdd = (int)Mathf.Log(playerLayers[pi.playerIndex], 2);
        var bitmask = (1 << layerToAdd) | (1 << 0) | (1 << 1) | (1 << 2) | (1 << 4) | (1 << 5);

        vehicle.layer = layerToAdd;

        //set the layer
        vehicle.GetComponentInChildren<CinemachineFreeLook>().gameObject.layer = layerToAdd;
        camera.gameObject.layer = layerToAdd;

        // add the layer
        camera.cullingMask = bitmask;
        
        
    }

   
}

public class PlayerConfiguration
{
    public PlayerInput Input { get; set; }
    public PlayerInputHandler InputHandler { get; set; }

    public int playerIndex {get; set;}

    // can store configuration values here 
    public bool isReady { get; set; }
    public GameObject playerObject {get; set;}
    
    public PlayerConfiguration(PlayerInput pi){
        playerIndex = pi.playerIndex;
        Input = pi;
        InputHandler = Input.GetComponent<PlayerInputHandler>();
    }

}
