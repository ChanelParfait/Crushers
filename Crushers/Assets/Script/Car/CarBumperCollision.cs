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
        //check at least 1 contact point, check the first contact point, 
        //check collider reference, check collider triggered contact is the bumperCollider 
       if(collision.contacts.Length > 0 && collision.contacts[0].thisCollider == bumperCollider){
            Debug.Log("Collision");
            if(collision.gameObject.CompareTag("Player")){
                

                //crash into a player

                LastCollidedPlayer = collision.gameObject;
                Debug.Log("collided with " + LastCollidedPlayer.name);
                CarStats collidedPlayerStats = LastCollidedPlayer.GetComponent<CarStats>();
                
                //if car has stats, do some dmg when collided into
                if(collidedPlayerStats != null){
                    collidedPlayerStats.increaseDamage(10);
                    Debug.Log("Collided vehicle damage: " + collidedPlayerStats.getDamage());
                }
                StartCoroutine(ClearLastCollidedPlayerAfterDelay(5f));
            }



            else{
                Debug.Log("Not a vehicle");
            }
        }
    }

    private IEnumerator ClearLastCollidedPlayerAfterDelay(float delay){
        yield return new WaitForSeconds(delay);
        LastCollidedPlayer = null;
        Debug.Log("Cleared last collided player");
    }


}
    