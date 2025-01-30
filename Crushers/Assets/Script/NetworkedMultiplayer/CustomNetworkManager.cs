using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror; 
using UnityEngine.SceneManagement;
using Steamworks;
using Unity.VisualScripting;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField] private NetworkPlayerController GamePlayerPrefab;
    public List<NetworkPlayerController> GamePlayers { get; }= new List<NetworkPlayerController>();



    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if(SceneManager.GetActiveScene().name == "Lobby"){
            NetworkPlayerController GamePlayerInstance = Instantiate(GamePlayerPrefab);
            GamePlayerInstance.ConnectionID = conn.connectionId;
            GamePlayerInstance.playerIDNumber = GamePlayers.Count + 1;
            GamePlayerInstance.PlayerSteamID = (ulong)SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)SteamLobby.Instance.CurrentLobbyID, GamePlayers.Count);

            NetworkServer.AddPlayerForConnection(conn, GamePlayerInstance.gameObject);
        }
    }

    public void StartGame(string SceneName)
    {
        ServerChangeScene(SceneName);
    }

    public GameObject SpawnPlayerVehicle(GameObject prefab, Transform spawn, Transform parent, NetworkConnectionToClient conn)
    {
        NetworkClient.RegisterPrefab(prefab);
        GameObject PlayerVehicle = Instantiate(prefab, spawn.position, spawn.rotation, parent);
        NetworkServer.Spawn(PlayerVehicle, conn);
        return PlayerVehicle;
    }
}
