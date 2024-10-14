using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamiKazeBomb : MonoBehaviour
{
    
    [SerializeField] private GameObject Player;
    public void SetPlayer(GameObject player)
    {
        Player = player;
    }
}
