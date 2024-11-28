using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    
    public void TestInvoke(GameObject ScoringPlayer, GameObject DefeatedPlayer, TypeOfDeath Death)
    {
        //Debug.Log(ScoringPlayer.name + " Has Scored A Point Against " + DefeatedPlayer.name + " By " + Death.ToString());
        string scoringPlayerLayerName = LayerMask.LayerToName(ScoringPlayer.layer);
        string defeatedPlayerLayerName = LayerMask.LayerToName(DefeatedPlayer.layer);
        
        
        Debug.Log(scoringPlayerLayerName + " Has Scored A Point Against " + defeatedPlayerLayerName + " By " + Death.ToString());
    }
}
