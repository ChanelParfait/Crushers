using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CinematicsManager : MonoBehaviour
{
    private PlayableDirector playableDirector;

    private void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();

        PlayCutscene();
    }

    private void Update()
    {
    
    }

    public void PlayCutscene() {

        playableDirector.Play();

        Debug.Log("Cutscene is playing");

    }
}
