using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Linq;

public class PlayerManager : MonoBehaviour
{
    private List<PlayerConfiguration> playerConfigs; 
    private List<PlayerInput> players = new List<PlayerInput>();
    [SerializeField] private List<Transform> startingPoints; 
    [SerializeField] private List<LayerMask> playerLayers; 
    
    private void Awake()
    {
        playerConfigs = new List<PlayerConfiguration>();
    }
   
   void OnEnable()
    {
        SetupMenuController.vehicleSelected += SetPlayerVehicle; 
        SetupMenuController.playerReady += ReadyPlayer; 

    }

    void OnDisable()
    {
        SetupMenuController.vehicleSelected -= SetPlayerVehicle; 
        SetupMenuController.playerReady -= ReadyPlayer; 

        
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
            StartLevel();
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

    private void StartLevel(){
        Debug.Log("Level Start");
        // disable joining once level starts
        GetComponent<PlayerInputManager>().DisableJoining();


        foreach(PlayerConfiguration playerConfig in playerConfigs){
            SetupVehicle(playerConfig);
        }
    }

    


    public void SetupVehicle(PlayerConfiguration pi)
    {
        Debug.Log("Setup Player Vehicle: " + pi.playerIndex);

        // destroy setup menu
        Destroy(pi.Input.gameObject.GetComponentInChildren<Canvas>().gameObject);

        // spawn vehicle from player config as child of player config
        GameObject vehicle = Instantiate(pi.playerObject, startingPoints[pi.playerIndex].position, startingPoints[pi.playerIndex].rotation, pi.Input.gameObject.transform);
        pi.Input.gameObject.GetComponent<PlayerInputHandler>().SetPlayerIndex(pi.playerIndex);

        // find car controller, pickup manager and camera input handler and hand them to the player input handler
        pi.Input.gameObject.GetComponent<PlayerInputHandler>().SetCarController(pi.Input.gameObject.GetComponentInChildren<PrometeoCarController>(), pi.playerIndex);
        pi.Input.gameObject.GetComponent<PlayerInputHandler>().SetPickupManager(pi.Input.gameObject.GetComponentInChildren<PickUpManager>());
        pi.Input.gameObject.GetComponent<PlayerInputHandler>().SetCameraInputHandler(pi.Input.gameObject.GetComponentInChildren<CameraInputHandler>());
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
    public int playerIndex {get; set;}

    // can store configuration values here 
    public bool isReady { get; set; }
    public GameObject playerObject {get; set;}
    
    public PlayerConfiguration(PlayerInput pi){
        playerIndex = pi.playerIndex;
        Input = pi;
    }

}
