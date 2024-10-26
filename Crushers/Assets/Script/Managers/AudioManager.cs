using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource bgSource;

    [Header("Music")]
    [SerializeField] AudioClip mainMusic;
    [SerializeField] AudioClip levelMusic;
    [SerializeField] [Range(0f,1f)] float musicVolume = 1.0f;
    [SerializeField] AudioClip crowdSounds;
    [SerializeField] AudioClip crowdCheer;
    [SerializeField][Range(0f, 1f)] float crowdSoundsVolume = 0.3f;

    
    void OnEnable()
    {
        PlayerManager.ArenaLevelLoaded +=  UpdateMusic;
        //PlayerManager.ArenaLevelLoaded +=  DisableSetupComponents;

    }

    void OnDisable()
    {
        PlayerManager.ArenaLevelLoaded -=  UpdateMusic;
        //PlayerManager.ArenaLevelLoaded -=  DisableSetupComponents;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
            PlayMusic(mainMusic, musicVolume);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void UpdateMusic(bool isArena){
        if(isArena){
            PlayMusic(levelMusic, musicVolume);
            PlaySFX(crowdSounds, crowdSoundsVolume);
        } 
        else if(SceneManager.GetActiveScene().buildIndex == 5)
        {
            // if scene is the leaderboard 
            StopMusic();
            PlaySFX(crowdCheer, 1);
        } else {
            StopSFX();
            PlayMusic(mainMusic, musicVolume);
        }
    }
    private void PlayMusic(AudioClip clip, float volume)
    {
        if (clip != null && clip != musicSource.clip)
        {
            musicSource.clip = clip;
            musicSource.volume = volume;
            musicSource.Play();
        }
    }

    private void StopMusic(){
        musicSource.Stop();
    }

    private void PlaySFX(AudioClip clip, float volume)
    {
        if (clip != null)
        {
            bgSource.clip = clip;
            bgSource.volume = volume;
            bgSource.Play();
        }
    }

    private void StopSFX(){
        bgSource.Stop();
    }

}
