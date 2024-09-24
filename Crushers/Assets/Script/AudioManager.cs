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


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlayClip(AudioClip clip, float volume)
    {
        if (clip != null)
        {
            Vector3 camerPos = Camera.main.transform.position;
            AudioSource.PlayClipAtPoint(clip, camerPos, volume);
        }

    }

    public void StopCurrentClip()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void PlayMainMusic()
    {
        StopCurrentClip(); 
        audioSource.PlayOneShot(mainMusic, mainMusicVolume);
    }

    public void PlayCrowdSounds()
    {
        StopCurrentClip(); 
        audioSource.PlayOneShot(crowdSounds, crowdSoundsVolume);
    }


}
