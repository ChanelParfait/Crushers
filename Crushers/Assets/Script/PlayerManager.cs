using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{

    private List<PlayerInput> players = new List<PlayerInput>();
    [SerializeField] private List<Transform> startingPoints; 
    [SerializeField] private List<LayerMask> playerLayers; 
    private PlayerInputManager playerInputManager; 
    
    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
    }


    public void AddPlayer(PlayerInput player){
        players.Add(player);

        player.transform.position = startingPoints[players.Count - 1].position;

        int layerToAdd = (int)Mathf.Log(playerLayers[players.Count-1].value, 2);
        var bitmask = (1 << layerToAdd) | (1 << 0) | (1 << 1) | (1 << 2) | (1 << 4) | (1 << 5);

        //set the layer
        player.GetComponentInChildren<CinemachineFreeLook>().gameObject.layer = layerToAdd;
        player.GetComponentInChildren<Camera>().gameObject.layer = layerToAdd;

        // add the layer
        player.GetComponentInChildren<Camera>().cullingMask = bitmask;
        // set the action in the custom cinemachine input handler
        player.GetComponentInChildren<CameraInputHandler>().horizontal = player.actions.FindAction("Look");
    }

   
}
