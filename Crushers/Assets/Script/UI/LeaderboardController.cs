using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderboardController : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> scoreSlots;
    private PlayerManager pm; 
    private List<PlayerObjectController> playerConfigs; 
    // list of player configs sorted by scores in descending order
    private List<PlayerObjectController> leaderboardConfigs; 



    // Start is called before the first frame update
    void Start()
    {  
        leaderboardConfigs = new List<PlayerObjectController>();
        // look for the player manager
        pm = FindObjectOfType<PlayerManager>();

        // get player configs
        if(pm){
            leaderboardConfigs = pm.GetPlayerObjects();
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

                if (leaderboardConfigs[j].Score > leaderboardConfigs[maxIndex].Score)
                    maxIndex = j;

            // Swap the found minimum element with the first
            // element
            PlayerObjectController temp = leaderboardConfigs[maxIndex];
            leaderboardConfigs[maxIndex] = leaderboardConfigs[i];
            leaderboardConfigs[i] = temp;
        }
    }


    private void DisplayScores(){
        // loop through player configs
        for(int i = 0; i < leaderboardConfigs.Count; i++){
            string player = (leaderboardConfigs[i].PlayerIndex + 1).ToString(); 
            string score = leaderboardConfigs[i].Score.ToString(); 

            scoreSlots[i].SetText("Player " + player + "            " + score);

            //Debug.Log("Player " + player + "            " + score);
        }
    }

    // Create a UI Controller base class with a generic click function 
    // could change this to find audio source in parent instead
    public void Click(AudioSource buttonAudio){
        //Debug.Log("Click: " + st);
        buttonAudio.Play();
    }

    public void RestartGame(){
        if(pm){
            // remove player objects
            pm.DestroyConfigs();
            // destroy player input manager
            DestroyImmediate(pm.gameObject);
        }
        // load main menu
        SceneManager.LoadScene(0);
    }
}
