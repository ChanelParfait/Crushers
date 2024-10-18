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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
         PlayCutscene();
        }
    }

    public void PlayCutscene() {

        playableDirector.Play();

    }
}
