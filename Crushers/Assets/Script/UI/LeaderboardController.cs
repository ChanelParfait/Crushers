using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderboardController : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> scoreSlots;
    private PlayerManager pm; 
    private List<PlayerConfiguration> playerConfigs; 
    // list of player configs sorted by scores in descending order
    private List<PlayerConfiguration> leaderboardConfigs; 



    // Start is called before the first frame update
    void Start()
    {  
        leaderboardConfigs = new List<PlayerConfiguration>();
        // look for the player manager
        pm = FindObjectOfType<PlayerManager>();
        // get player configs
        if(pm){
            leaderboardConfigs = pm.GetPlayerConfigs();
            // temporary code to destroy pm since leaderboard is the end of the game
            // can remove later if we make the leaderboard load in between maps
            if(pm){
                Destroy(pm);
                pm = null;
            }
            
        }
        // set up leaderboard
        SortScores();
        DisplayScores();
    }

    private void SortScores(){
        int n = leaderboardConfigs.Count;

        // One by one move boundary of unsorted subarray
        for (int i = 0; i < n; i++)
        {
            // Find the maximum element in unsorted array
            int maxIndex = i;
            for (int j = i + 1; j < n; j++)

                if (leaderboardConfigs[j].score > leaderboardConfigs[maxIndex].score)
                    maxIndex = j;

            // Swap the found minimum element with the first
            // element
            PlayerConfiguration temp = leaderboardConfigs[maxIndex];
            leaderboardConfigs[maxIndex] = leaderboardConfigs[i];
            leaderboardConfigs[i] = temp;
        }
    }


    private void DisplayScores(){
        // loop through player configs
        for(int i = 0; i < leaderboardConfigs.Count; i++){
            string player = (leaderboardConfigs[i].playerIndex + 1).ToString(); 
            string score = leaderboardConfigs[i].score.ToString(); 

            scoreSlots[i].SetText("Player " + player + "            " + score);

            Debug.Log("Player " + player + "            " + score);
        }
    }

    // Create a UI Controller base class with a generic click function 
    // could change this to find audio source in parent instead
    public void Click(AudioSource buttonAudio){
        //Debug.Log("Click: " + st);
        buttonAudio.Play();
    }

    public void RestartGame(){
        // destroy player manager
        if(pm){
            Destroy(pm);
        }
        // load main menu
        SceneManager.LoadScene(0);
    }
}
