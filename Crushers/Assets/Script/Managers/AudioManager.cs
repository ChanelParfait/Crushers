using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;

    [Header("Music")]
    [SerializeField] AudioClip mainMusic;
    [SerializeField] [Range(0f,1f)] float mainMusicVolume = 1.0f;
    [Header("Crowd")]
    [SerializeField] AudioClip crowdSounds;
    [SerializeField][Range(0f, 1f)] float crowdSoundsVolume = 1.0f;

    
    void OnEnable()
    {
        PlayerManager.ArenaLevelLoaded +=  SetMusic;
        //PlayerManager.ArenaLevelLoaded +=  DisableSetupComponents;

    }

    void OnDisable()
    {
        PlayerManager.ArenaLevelLoaded -=  SetMusic;
        //PlayerManager.ArenaLevelLoaded -=  DisableSetupComponents;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
            audioSource = GetComponent<AudioSource>();
            PlayClip(mainMusic, mainMusicVolume);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SetMusic(bool isArena){
        if(isArena){
            PlayClip(crowdSounds, crowdSoundsVolume);
        } else {
            PlayClip(mainMusic, mainMusicVolume);
        }
    }
    private void PlayClip(AudioClip clip, float volume)
    {
        if (clip != null)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.Play();
        }

    }

}
