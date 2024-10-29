using UnityEngine;
using UnityEngine.Playables;

public class CutsceneController : MonoBehaviour
{
    private PlayableDirector playableDirector;

    private void Start()
    {
        playableDirector = GetComponentInChildren<PlayableDirector>();
        
        if (playableDirector != null)
        {
            playableDirector.stopped += OnPlayableDirectorStopped;
        }
    }

    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        // Destroy the game object when the timeline finishes
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event when the object is destroyed
        if (playableDirector != null)
        {
            playableDirector.stopped -= OnPlayableDirectorStopped;
        }
    }
}