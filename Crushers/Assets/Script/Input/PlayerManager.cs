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
using UnityEditor;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;
    private List<PlayerObjectController> PlayerObjects;

    // Player Initialising Values
    [SerializeField] private GameObject defaultVehiclePrefab; 
    // Game State
    public bool isTesting = false;
    // Game / Scene Management
    [SerializeField] private int selectedMapIndex = 2; 
    private int leaderboardScene = 5; 
    // Menus
    private LoadingScreen loadingScreen;
    private PauseMenuController pauseMenu;


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
            PlayerObjects = new List<PlayerObjectController>();
        }        
    }
   
   void OnEnable()
    {
        SetupMenuController.playerReady += CheckIfAllReady; 
        LevelManager.ArenaLevelEnded += LoadLeaderboard;
        SceneManager.sceneLoaded += OnLevelLoaded;
        MainMenuController.levelSelected += SaveMapSelection; 
        PlayerInputHandler.Pause += OnPause;

    }

    void OnDisable()
    {
        SetupMenuController.playerReady -= CheckIfAllReady;   
        LevelManager.ArenaLevelEnded -= LoadLeaderboard; 
        SceneManager.sceneLoaded -= OnLevelLoaded;
        MainMenuController.levelSelected -= SaveMapSelection; 
        PlayerInputHandler.Pause -= OnPause;

    }

    public List<PlayerObjectController> GetPlayerObjects()
    {
        return PlayerObjects;
    }
        
    // Event for a Player Joining 
    // Needs checks between online and offline functionality
    public void HandlePlayerJoin(PlayerInput pi)
    {
        if(isTesting){
            // create configuration object for player 
            PlayerObjectController playerConfig = pi.GetComponent<PlayerObjectController>();
            PlayerObjects.Add(playerConfig);
            GetComponent<PlayerInputManager>().DisableJoining();
            SetupArena();
        }
        
        if(pi.playerIndex == 0){
            //startingPoints = GameObject.FindGameObjectWithTag("Spawns").GetComponentsInChildren<Transform>();
            // invoke first player joined event 
            firstPlayerJoined?.Invoke();
        }

        if(!PlayerObjects.Any(p => p.PlayerIndex == pi.playerIndex))
        {
            // Add Player to List
            PlayerObjectController playerConfig = pi.GetComponent<PlayerObjectController>();
            //playerConfig.SetPosition();
            PlayerObjects.Add(playerConfig);
            DontDestroyOnLoad(playerConfig.gameObject);
        }
    }

    public void OnPlayerLeft(PlayerInput pi){
        //Debug.Log("Player " + pi.playerIndex + " Left");
    }


    // Create Check if All Ready Function here
    public void CheckIfAllReady(int index)
    {
        PlayerObjects[index].IsReady = true; 

        if( PlayerObjects.All(p => p.IsReady == true))
        {
            // load selected level
            StartCoroutine(DelayScreen(2));
        }   
    }

    // Loading Screen Functionality //
    private IEnumerator DelayScreen(float delay){
        GetComponent<PlayerInputManager>().DisableJoining();
        loadingScreen.DisplayScreen();
        
        yield return new WaitForSeconds(delay);

        StartCoroutine(LoadSceneAsync(selectedMapIndex));
    }

    private IEnumerator LoadSceneAsync(int buildIndex){
        AsyncOperation operation = SceneManager.LoadSceneAsync(buildIndex);

        while(!operation.isDone){
            float progress = Mathf.Clamp01(operation.progress / 0.9f );
            loadingScreen.UpdateProgress(progress);
            yield return null;
        }
    }
    
    // Pausing Funcctionality // 
    // Will need adjustment between online and offline functionality
    private void OnPause(){
        // display a pause menu
        if(SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 3 || SceneManager.GetActiveScene().buildIndex == 4){
            if(pauseMenu){
                if(Time.timeScale != 0){
                    //Debug.Log("Pause: " + playerIndex);
                    Time.timeScale = 0;
                    pauseMenu.SetActive(true);
                } else {
                    //Debug.Log("Unpause: " + playerIndex);
                    Time.timeScale = 1; 
                    pauseMenu.SetActive(false);
                }
            }
        }
    }

    // Needs altering to suit new class structure
    private void SetupArena()
    {
        //Debug.Log("Arena initialising...");
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu").GetComponent<PauseMenuController>();

        // invoke arena level loaded event 
        // Check if all Players are Loaded here
        ArenaLevelLoaded?.Invoke(true);
    }

    // Level / Player Management 

    // keep track of what level we are currently in
    private void OnLevelLoaded(Scene scene, LoadSceneMode mode){
        
        GetComponent<PlayerInputManager>().DisableJoining();

        if(scene.name == "TestingScene"){
            GetComponent<PlayerInputManager>().EnableJoining();
            SetupArena();
        }
    
        if(scene.buildIndex == 1){
            // Enable joining in vehicle selection level
            GetComponent<PlayerInputManager>().EnableJoining();
            // find the loading screen game object
            loadingScreen = GameObject.FindGameObjectWithTag("LoadingScreen").GetComponent<LoadingScreen>();
        }
        
        // if scene index is an arena scene
        if(scene.buildIndex == 2 || scene.buildIndex == 3 || scene.buildIndex == 4){
            // initialise players with vehicles
            SetupArena();
        } else {
            ArenaLevelLoaded?.Invoke(false);
        }
    }

    // Level End Event // 
    private void LoadLeaderboard()
    {
        Debug.Log("Level Complete");
        SceneManager.LoadScene(leaderboardScene);
    }

    public void DestroyConfigs()
    {
        if(PlayerObjects != null){
            foreach(PlayerObjectController player in PlayerObjects){
                Destroy(player.gameObject);
            }
        }
        PlayerObjects = new List<PlayerObjectController>();
    }

    private void SaveMapSelection(int index)
    {
        //Debug.Log("Level Selected: " + index);
        selectedMapIndex = index;
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

