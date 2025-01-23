using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{ 
    [SerializeField] private bool isTesting = false; 

    // level timing // 
    [SerializeField] private int levelDuration; 
    private int startCountdownTimer = 3; 
    private int levelCountdownTimer;
    private float prevTime = 0; 
    private float totalTime = 0; 
    private bool startCountdownEnded = false; 
    private bool isTimerRunning = false; 
    private AudioSource source; 
    // Spawning
    [SerializeField] private List<Transform> spawnPositions = new List<Transform>();
    private List<Transform> availableSpawnPositions = new List<Transform>();


    // event when players should gain control 
    public static UnityAction ArenaLevelStarted;

    // event when timer runs out
    public static UnityAction ArenaLevelEnded;
    // event for countdown timers
    public static UnityAction<int> startTimeChanged;
    public static UnityAction<int> levelTimeChanged;
     


    void OnEnable()
    {
        PlayerManager.ArenaLevelLoaded +=  LoadLevel;
        //PlayerManager.ArenaLevelLoaded +=  DisableSetupComponents;

    }

    void OnDisable()
    {
        PlayerManager.ArenaLevelLoaded -=  LoadLevel;
        //PlayerManager.ArenaLevelLoaded -=  DisableSetupComponents;
    }

    // Start is called before the first frame update
    void Awake()
    {   
        source = GetComponent<AudioSource>();
        levelCountdownTimer = levelDuration;
        availableSpawnPositions = spawnPositions;
        startCountdownTimer = 3;
        if(isTesting){
             startCountdownTimer = 0;

            startTimeChanged?.Invoke(0);
            startCountdownEnded = true;

        }

    }

    // Update is called once per frame
    void Update()
    {     
        if(isTimerRunning){
            totalTime += Time.deltaTime;
        }

        // one second has passed
        if(prevTime + 1 <= totalTime){
            // If start countdown hasn't ended
            // decriment start countdown to 0 
            if(!startCountdownEnded){
                startCountdownTimer--;
                startTimeChanged?.Invoke(startCountdownTimer);

                if(startCountdownTimer == 0){
                    startCountdownEnded = true;
                    ArenaLevelStarted?.Invoke();
                }
            } 
            // if level has started, decriment round timer to 0 
            else {
                levelCountdownTimer--;
                
                if(levelTimeChanged != null)
                    levelTimeChanged?.Invoke(levelCountdownTimer);

                if(levelCountdownTimer == 0){
                    // invoke level ended event
                    isTimerRunning = false;
                    ArenaLevelEnded?.Invoke();
                    
                }
            }
            prevTime = totalTime;
        }

    }

    public Transform GetSpawnPos()
    {
        Transform spawn = null;
        // get last available spawn position in list
        Debug.Log(availableSpawnPositions);
        if(availableSpawnPositions.Count > 0){
            spawn = availableSpawnPositions[availableSpawnPositions.Count - 1];
            // and remove it 
            availableSpawnPositions.Remove(spawn);
        }
        return spawn;
    }

    private void LoadLevel(bool isArena){

        isTimerRunning = isArena;
        //source.Play();
    }

    
}
