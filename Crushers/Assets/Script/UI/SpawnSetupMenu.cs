using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class SpawnPlayerSetupMenu : MonoBehaviour
{
    
    public GameObject playerSetupMenuPrefab;
    public PlayerInput input;

    private void Awake()
    { 
            var menu = Instantiate(playerSetupMenuPrefab, transform);
            input.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
            //menu.GetComponent<SetupMenuController>().SetPlayerIndex(input.playerIndex);
    }
}
