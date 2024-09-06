using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarStats : MonoBehaviour
{
    [Header("----------Stats-----------")]
    [SerializeField]public float score;
    [SerializeField]private float health;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
    void takeDamage(float damage){
        health = health + damage;
        //can respawn if flipped after period of time
    }
}
