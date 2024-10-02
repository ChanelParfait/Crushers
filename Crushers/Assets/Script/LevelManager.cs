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
    private bool ArenalevelLoaded = false; 
    // event when players should gain control 
    public static UnityAction ArenaLevelStarted;

    public static UnityAction ArenaLevelEnded;

    public static UnityAction<int> startTimeChanged;
    public static UnityAction<int> levelTimeChanged;
     


       void OnEnable()
    {
        PlayerManager.ArenaLevelLoaded +=  LoadLevel;
        PlayerManager.ArenaLevelLoaded +=  DisableSetupComponents;

    }

    void OnDisable()
    {
        PlayerManager.ArenaLevelLoaded -=  LoadLevel;
        PlayerManager.ArenaLevelLoaded -=  DisableSetupComponents;

    }

    // Start is called before the first frame update
    void Start()
    {   
        
        levelCountdownTimer = levelDuration;
        startCountdownTimer = 3;
        AudioManager.Instance.PlayMainMusic();
    }

    // Update is called once per frame
    void Update()
    {     
        if(ArenalevelLoaded){
            totalTime += Time.deltaTime;
        }

        // one second has passed
        if(prevTime + 1 <= totalTime){
            // If start countdown hasn't ended
            // decriment start countdown to 0 
            if(!startCountdownEnded){
                startCountdownTimer--;
                
                if(startTimeChanged != null)
                    startTimeChanged.Invoke(startCountdownTimer);

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
                    if(ArenaLevelEnded != null)
                    ArenalevelLoaded = false;
                    ArenaLevelEnded.Invoke();
                    AudioManager.Instance.PlayMainMusic();
                }
            }
            prevTime = totalTime;
        }

    }

    private void LoadLevel(){
        ArenalevelLoaded = true;
        AudioManager.Instance.PlayCrowdSounds();
    }

    private void DisableSetupComponents(){
        // get main camera and join canvas and disable objects
        //GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        //if(camera){ camera.SetActive(false); }
        GameObject canvas = GameObject.FindGameObjectWithTag("JoinCanvas");
        if(canvas){ canvas.SetActive(false); }

    }

    
}
