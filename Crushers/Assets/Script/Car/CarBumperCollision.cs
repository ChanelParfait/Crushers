using System.Collections;
using UnityEngine;

public class CarBumperCollision : MonoBehaviour
{   
    public CarStats carStats;  
    [SerializeField] private Collider bumperCollider;  
    void Start(){
        if(carStats == null){
            carStats = GetComponent<CarStats>();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision was triggered by the bumperCollider
        if (collision.contacts.Length > 0 && collision.contacts[0].thisCollider == bumperCollider)
        {
            Debug.Log("Collision");
           
            if (collision.gameObject.CompareTag("Player"))
            {
                // Set the last collided player in the car's stats
                carStats.setLastCollided(collision.gameObject);
                GameObject lastCollidedPlayer = carStats.getLastCollided();

                Debug.Log("Collided with " + lastCollidedPlayer.name);

                CarStats collidedPlayerStats = lastCollidedPlayer.GetComponentInParent<CarStats>();
                
                if (collidedPlayerStats != null)
                {
                    float newDamage = Mathf.Round(carStats.getSpeed() / 3);
                    collidedPlayerStats.increaseDamage(newDamage);
                    collidedPlayerStats.addCentreOfMass(newDamage);
                    Debug.Log("Collided vehicle damage: " + collidedPlayerStats.getDamage());
                }

                // Start coroutine to clear the last collided player after 5 seconds
                StartCoroutine(ClearLastCollidedPlayerAfterDelay(5f));
            }
            else
            {
                Debug.Log("Not a vehicle");
            }
        }
    }

    
    private IEnumerator ClearLastCollidedPlayerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        carStats.setLastCollided(null);  
        Debug.Log("Cleared last collided player");
    }
}