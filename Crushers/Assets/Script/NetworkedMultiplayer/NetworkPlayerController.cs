using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using Edgegap;
using UnityEditor.SearchService;

public class NetworkPlayerController : NetworkBehaviour
{
    // Player Data
    [SyncVar] public int ConnectionID;
    [SyncVar] public int playerIDNumber;
    [SyncVar] public ulong PlayerSteamID;
    [SyncVar (hook = nameof(PlayerNameUpdate))] public string PlayerName;
    [SyncVar (hook = nameof(PlayerReadyUpdate))] public bool Ready; 
    [SyncVar (hook = nameof(VehicleConfirmedUpdate))] public bool VehicleConfirmed; 
    [SyncVar (hook = nameof(SendPlayerColour))] public int PlayerColour; 

    private CustomNetworkManager manager;

    private CustomNetworkManager Manager{
        get { 
                if(manager != null){
                    return manager; 
                }
                return manager = CustomNetworkManager.singleton as CustomNetworkManager;
            }
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnStartAuthority()
    {
        CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
        gameObject.name = "LocalGamePlayer";
        LobbyController.instance.FindLocalPlayer();
        PlayerManager.instance.FindLocalPlayer();
        LobbyController.instance.UpdateLobbyName();

    }

    public override void OnStartClient()
    {
        Manager.GamePlayers.Add(this);
        LobbyController.instance.UpdateLobbyName();
        LobbyController.instance.UpdatePlayerList();
    }

    public override void OnStopClient()
    {
        Manager.GamePlayers.Remove(this);
        LobbyController.instance.UpdatePlayerList();
    }

    private void PlayerReadyUpdate(bool oldValue, bool newValue){
        if(isServer)
        {
            this.Ready = newValue;
        }
        if(isClient)
        {
            LobbyController.instance.UpdatePlayerList();
        }
    }

    [Command]
    private void CMDSetPlayerReady()
    {
        Debug.Log("Change Ready: " + this.Ready);

        this.PlayerReadyUpdate(this.Ready, !this.Ready);
    }

    public void ChangeReady(){
        //Debug.Log("Check Auth: " + authority);

        if(isOwned){
            CMDSetPlayerReady();
        }
    }

    private void VehicleConfirmedUpdate(bool oldValue, bool newValue){
        if(isServer)
        {
            this.VehicleConfirmed = newValue;
        
        }
        if(isClient)
        {
            UpdateVehicleConfirmed(newValue);
            PlayerManager.instance.CheckIfAllConfimed();
        }
    }

    [Command]
    private void CMDSetVehicleConfirmed()
    {
        Debug.Log("Change Vehicle Confirmed: " + this.VehicleConfirmed);

        this.VehicleConfirmedUpdate(this.VehicleConfirmed, !this.VehicleConfirmed);
    }

    public void ChangeVehicleConfirmed(){
        //Debug.Log("Check Auth: " + authority);
        if(isOwned){
            CMDSetVehicleConfirmed();
        }
    }
    void UpdateVehicleConfirmed(bool message)
    {
        VehicleConfirmed = message;
    }

    [Command]
    private void CmdSetPlayerName(string PlayerName)
    {
        this.PlayerNameUpdate(this.PlayerName, PlayerName);
    }

    public void PlayerNameUpdate(string OldValue, string newValue)
    {
        if(isServer)
        {
            this.PlayerName = newValue;
        }
        if(isClient)
        {
            LobbyController.instance.UpdatePlayerList();
        }
    }

    // Start Game
    public void CanLoadScene(string SceneName)
    {
        if(isOwned){
            CmdCanLoadScene(SceneName);
        }
    }

    [Command]
    public void CmdCanLoadScene(string SceneName)
    {
        Manager.ChangeScene(SceneName);
    }

    // Cosmetics
    [Command]
    public void CmdUpdatePlayerColour(int newValue)
    {
        SendPlayerColour(PlayerColour, newValue);
    }

    public void SendPlayerColour(int oldValue, int newValue)
    {
        if(isServer) // host
        {
            PlayerColour = newValue;
        }
        if(isClient && oldValue != newValue)  // client
        {
            UpdateColour(newValue);
        }
    }

    void UpdateColour(int message)
    {
        PlayerColour = message;
    }

}   
