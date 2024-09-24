using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    AudioSource musicPlayer; 
    [SerializeField] private int levelDuration; 

    private int startCountdownTimer; 
    private int levelCountdownTimer;
    private float prevTime = 0; 
    private float totalTime = 0; 
    private bool startCountdownEnded = false; 
    private bool levelLoaded = false; 
    // event when players should gain control 
    public static UnityAction LevelStarted;

    public static UnityAction LevelEnded;

    public static UnityAction<int> startTimeChanged;
    public static UnityAction<int> levelTimeChanged;
     


       void OnEnable()
    {
        PlayerManager.LevelLoaded +=  LoadLevel;
        PlayerManager.LevelLoaded +=  DisableSetupComponents;

    }

    void OnDisable()
    {
        PlayerManager.LevelLoaded -=  LoadLevel;
        PlayerManager.LevelLoaded -=  DisableSetupComponents;

    }

    // Start is called before the first frame update
    void Start()
    {   
        musicPlayer = GetComponentInChildren<AudioSource>();
        musicPlayer.Play();
        levelCountdownTimer = levelDuration;
        startCountdownTimer = 3; 

    }

    // Update is called once per frame
    void Update()
    {     
        if(levelLoaded){
            // stop playing menu music when level starts
            if(musicPlayer.isPlaying){
                musicPlayer.Stop();
            }
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
                    LevelStarted.Invoke();
                }
            } 
            // if level has started, decriment round timer to 0 
            else {
                levelCountdownTimer--;
                
                if(levelTimeChanged != null)
                    levelTimeChanged.Invoke(levelCountdownTimer);

                if(levelCountdownTimer == 0){
                    // invoke level ended event
                    if(LevelEnded != null)
                    levelLoaded = false;
                    LevelEnded.Invoke();
                }
            }
            prevTime = totalTime;
        }

    }

    private void LoadLevel(){
        levelLoaded = true;
    }

    private void DisableSetupComponents(){
        // get main camera and join canvas and disable objects
        //GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        //if(camera){ camera.SetActive(false); }
        GameObject canvas = GameObject.FindGameObjectWithTag("JoinCanvas");
        if(canvas){ canvas.SetActive(false); }

    }

    
}
