using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
}
