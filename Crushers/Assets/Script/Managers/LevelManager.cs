using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{ 
    [SerializeField] private int levelDuration; 

    private int startCountdownTimer; 
    private int levelCountdownTimer;
    private float prevTime = 0; 
    private float totalTime = 0; 
    private bool startCountdownEnded = false; 
    private bool isTimerRunning = false; 
    private AudioSource source; 


    // event when players should gain control 
    public static UnityAction ArenaLevelStarted;

    public static UnityAction ArenaLevelEnded;

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
    void Start()
    {   
        source = GetComponent<AudioSource>();
        levelCountdownTimer = levelDuration;
        startCountdownTimer = 3;
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
                    ArenaLevelStarted.Invoke();
                }
            } 
            // if level has started, decriment round timer to 0 
            else {
                levelCountdownTimer--;
                
                if(levelTimeChanged != null)
                    levelTimeChanged.Invoke(levelCountdownTimer);

                if(levelCountdownTimer == 0){
                    // invoke level ended event
                    isTimerRunning = false;
                    ArenaLevelEnded?.Invoke();
                    
                }
            }
            prevTime = totalTime;
        }

    }

    private void LoadLevel(bool isArena){

        isTimerRunning = isArena;
        //source.Play();
    }

    private void DisableSetupComponents(bool isArena){
        // disable join canvas 
        if(isArena){
            GameObject canvas = GameObject.FindGameObjectWithTag("JoinCanvas");
            if(canvas){ canvas.SetActive(false); }
        }
    }

    
}
