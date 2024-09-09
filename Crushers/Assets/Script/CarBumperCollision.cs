using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBumperCollision : MonoBehaviour
{   

    public CarStats carStats;
    [SerializeField] private Collider bumperCollider;
    [SerializeField] private GameObject LastCollidedPlayer;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
       if(collision.collider != bumperCollider)
        {
            Debug.Log("Collision");
            if(collision.gameObject.CompareTag("Vehicle")){
                

                //crash into a player

                LastCollidedPlayer = collision.gameObject;
                Debug.Log("collided with " + LastCollidedPlayer.name);
                CarStats collidedPlayerStats = LastCollidedPlayer.GetComponent<CarStats>();
                
                //if car has stats, do some dmg when collided into
                if(collidedPlayerStats != null){
                    collidedPlayerStats.increaseDamage(10);
                    Debug.Log("Collided vehicle damage: " + collidedPlayerStats.getDamage());
                }
            }



            else{
                Debug.Log("Not a vehicle");
            }
        }
    }
}
    