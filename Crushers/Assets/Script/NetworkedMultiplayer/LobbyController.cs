using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class LobbyController : MonoBehaviour
{
    public static LobbyController instance;

    // UI 
    public TextMeshProUGUI LobbyNameText; 

    // Player Data
    public GameObject PlayerListViewContent;
    public GameObject PlayerListItemPrefab;
    public GameObject LocalPlayerObject;

    // Other Data
    public ulong CurrentLobbyID;
    public bool PlayerItemCreated = false; 
    private List<PlayerListItem> PlayerListItems = new List<PlayerListItem>();
    public NetworkPlayerController LocalPlayerController;

    // Ready Up
     public Button StartGameButton;
     public TextMeshProUGUI ReadyButtonText;

    // Manager
    private CustomNetworkManager manager;

    private CustomNetworkManager Manager{
        get { 
                if(manager != null){
                    return manager; 
                }
                return manager = CustomNetworkManager.singleton as CustomNetworkManager;
            }
    }

    private void Awake()
    {
        if(instance == null) { instance = this;}
    }

    public void ReadyPlayer()
    {
        Debug.Log("Ready Player");
        LocalPlayerController.ChangeReady();
    }

    public void UpdateButton()
    {
        if(LocalPlayerController.Ready){
            ReadyButtonText.text = "Unready";
        } 
        else 
        {
            ReadyButtonText.text = "Ready";
        }
    }

    public void CheckIfAllReady(){
        Debug.Log("Lobby Controller Check If All Ready");
        
        bool AllReady = false; 
        foreach(NetworkPlayerController player in Manager.GamePlayers)
        {
            if(player.Ready){
                AllReady = true;
            } 
            else 
            {
                AllReady = false;
                break; 
            }
        }
        if(AllReady)
        {
            if(LocalPlayerController.playerIDNumber == 1){
                StartGameButton.interactable = true;
            }
            else 
            {
                StartGameButton.interactable = false;
            }
        }
        else 
        {
            StartGameButton.interactable = false;
        }
    }

    public void UpdateLobbyName()
    {
        CurrentLobbyID = Manager.GetComponent<SteamLobby>().CurrentLobbyID;
        LobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(CurrentLobbyID), "name");
    }

    public void UpdatePlayerList()
    {
        if(!PlayerItemCreated ) {  CreateHostPlayerItem(); }
        if(PlayerListItems.Count < Manager.GamePlayers.Count) { CreateClientPlayerItem(); }
        if(PlayerListItems.Count > Manager.GamePlayers.Count) { RemovePlayerItem(); }
        if(PlayerListItems.Count == Manager.GamePlayers.Count) { UpdatePlayerItem(); }
    }

    public void FindLocalPlayer(){
        LocalPlayerObject = GameObject.Find("LocalGamePlayer");
        LocalPlayerController = LocalPlayerObject.GetComponent<NetworkPlayerController>();
    }

    public void CreateHostPlayerItem()
    {
        foreach (NetworkPlayerController player in Manager.GamePlayers) {
            GameObject NewPlayerItem = Instantiate(PlayerListItemPrefab) as GameObject;
            PlayerListItem NewPlayerItemScript = NewPlayerItem.GetComponent<PlayerListItem>();

            NewPlayerItemScript.PlayerName = player.PlayerName;
            NewPlayerItemScript.ConnectionID = player.ConnectionID;
            Debug.Log("Player Steam ID: " + player.PlayerSteamID);
            NewPlayerItemScript.PlayerSteamID = player.PlayerSteamID;
            NewPlayerItemScript.Ready = player.Ready;
            NewPlayerItemScript.SetPlayerValues();


            NewPlayerItem.transform.SetParent(PlayerListViewContent.transform);
            NewPlayerItem.transform.localScale = Vector3.one;

            PlayerListItems.Add(NewPlayerItemScript);

        }

        PlayerItemCreated = true; 
    }

    public void CreateClientPlayerItem()
    {
        foreach (NetworkPlayerController player in Manager.GamePlayers) {
            // check that this player isn't already in the list
            if(!PlayerListItems.Any(b => b.ConnectionID == player.ConnectionID)) {
                GameObject NewPlayerItem = Instantiate(PlayerListItemPrefab) as GameObject;
                PlayerListItem NewPlayerItemScript = NewPlayerItem.GetComponent<PlayerListItem>();

                NewPlayerItemScript.PlayerName = player.PlayerName;
                NewPlayerItemScript.ConnectionID = player.ConnectionID;
                NewPlayerItemScript.PlayerSteamID = (ulong)player.PlayerSteamID;
                NewPlayerItemScript.Ready = player.Ready;
                NewPlayerItemScript.SetPlayerValues();


                NewPlayerItem.transform.SetParent(PlayerListViewContent.transform);
                NewPlayerItem.transform.localScale = Vector3.one;

                PlayerListItems.Add(NewPlayerItemScript);
            }

        }
    }

    public void UpdatePlayerItem()
    {
        foreach (NetworkPlayerController player in Manager.GamePlayers) 
        {
            foreach(PlayerListItem PlayerListItemScript in PlayerListItems){
                if(PlayerListItemScript.ConnectionID == player.ConnectionID){
                    PlayerListItemScript.PlayerName = player.PlayerName;
                    PlayerListItemScript.Ready = player.Ready;
                    PlayerListItemScript.SetPlayerValues();

                    if(player == LocalPlayerController)
                    {
                        UpdateButton();
                    }
                }
            }
        } 
        CheckIfAllReady();
    }

    public void RemovePlayerItem()
    {
        List<PlayerListItem> PlayerListItemsToRemove = new List<PlayerListItem>();

        foreach (PlayerListItem playerListItem in PlayerListItems)
        {
            if(!Manager.GamePlayers.Any(b => b.ConnectionID == playerListItem.ConnectionID))
            {
                PlayerListItemsToRemove.Add(playerListItem);
            }
        }
        if(PlayerListItemsToRemove.Count > 0){
            foreach( PlayerListItem playerListItemToRemove in PlayerListItemsToRemove)
            {
                GameObject ObjectToRemove = playerListItemToRemove.gameObject;
                PlayerListItems.Remove(playerListItemToRemove);
                Destroy(ObjectToRemove);
                ObjectToRemove = null;
            }
        }
    }


    public void StartGame(string SceneName){
        LocalPlayerController.CanLoadScene(SceneName);
    }
}
