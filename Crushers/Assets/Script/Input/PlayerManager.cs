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
    PlayerControls playerControls; 
    private List<PlayerConfiguration> playerConfigs; 
    private List<PlayerInput> players = new List<PlayerInput>();
    [SerializeField] private List<Transform> startingPoints; 
    [SerializeField] private List<LayerMask> playerLayers; 
    //private PlayerInputManager playerInputManager; 
    
    private void Awake()
    {
        //playerInputManager = GetComponent<PlayerInputManager>();
        playerConfigs = new List<PlayerConfiguration>();
        playerControls = new PlayerControls();
    }
   
   void OnEnable()
    {
        playerControls.UI.Enable();
        SetupMenuController.vehicleSelected += SetPlayerVehicle; 
        SetupMenuController.playerReady += ReadyPlayer; 

    }

    void OnDisable()
    {
        playerControls.UI.Disable();

        SetupMenuController.vehicleSelected -= SetPlayerVehicle; 
        SetupMenuController.playerReady -= ReadyPlayer; 

        
    }

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return playerConfigs;
    }

    public void SetPlayerVehicle(int index, GameObject vehicle)
    {
        //Debug.Log("Player: " + index + " Colour: " + colour.name);
        //Debug.Log("Configs: " + playerConfigs.Count());

        playerConfigs[index].playerObject = vehicle;
    }

    public void ReadyPlayer(int index)
    {
        playerConfigs[index].isReady = true; 

        if( playerConfigs.All(p => p.isReady == true))
        {
            // start game
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


        if(!playerConfigs.Any(p => p.playerIndex == pi.playerIndex))
        {
            Debug.Log("Player Joined" + pi.playerIndex);

            playerConfigs.Add(new PlayerConfiguration(pi));
            pi.transform.SetParent(transform);
        }
    }

    private void StartLevel(){
        Debug.Log("Level Start");


        playerControls.UI.Disable();
        playerControls.Player.Enable();


        foreach(PlayerConfiguration playerConfig in playerConfigs){
            SetupVehicle(playerConfig);
        }
    }

    


    public void SetupVehicle(PlayerConfiguration pi)
    {
        Debug.Log("Setup Player Vehicle: " + pi.playerIndex);
         //pi.Input.uiInputModule = null;

        // disable setup menu and camera
        Destroy(pi.Input.gameObject.GetComponentInChildren<Camera>().gameObject);
        //pi.Input.gameObject.GetComponentInChildren<Canvas>().gameObject.SetActive(false);

        // spawn vehicle from player config as child of player config
        GameObject vehicle = Instantiate(pi.playerObject, startingPoints[pi.playerIndex].position, startingPoints[pi.playerIndex].rotation, pi.Input.gameObject.transform);
        pi.Input.camera = vehicle.GetComponentInChildren<Camera>();
        // find car controller and hand it to input handler
        pi.Input.gameObject.GetComponent<PlayerInputHandler>().SetCarController(vehicle.GetComponent<PrometeoCarController>());
        pi.Input.gameObject.GetComponent<PlayerInputHandler>().SetPickupManager(vehicle.GetComponent<PickUpManager>());



        /*players.Add(player);

        player.transform.position = startingPoints[players.Count - 1].position;
        player.transform.rotation = startingPoints[players.Count - 1].rotation;


        int layerToAdd = (int)Mathf.Log(playerLayers[players.Count-1].value, 2);
        var bitmask = (1 << layerToAdd) | (1 << 0) | (1 << 1) | (1 << 2) | (1 << 4) | (1 << 5);

        //set the layer
        player.GetComponentInChildren<CinemachineFreeLook>().gameObject.layer = layerToAdd;
        player.GetComponentInChildren<Camera>().gameObject.layer = layerToAdd;

        // add the layer
        player.GetComponentInChildren<Camera>().cullingMask = bitmask;
        // set the action in the custom cinemachine input handler
        player.GetComponentInChildren<CameraInputHandler>().horizontal = player.actions.FindAction("Look");*/
        
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
