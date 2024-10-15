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
            //Debug.Log("Collision");
           
            if (collision.gameObject.CompareTag("Player"))
            {

                CarStats hitCarStats = collision.gameObject.GetComponent<CarStats>();

                
                if (hitCarStats != null)
                {
                    // Set this car (the one doing the hitting) as the last collided car in the hit car's stats
                    hitCarStats.SetLastCollidedVehicle(hitCarStats);

                    hitCarStats.IncreaseDamageFromSpeed(carStats.GetSpeed());
                    Debug.Log("Collided vehicle damage: " + hitCarStats.GetDamage());
                    
                    StartCoroutine(ClearLastCollidedPlayerAfterDelay(hitCarStats, 5f));
                }

            }
            else
            {
                Debug.Log("Not a vehicle");
            }
        }
    }

    
    private IEnumerator ClearLastCollidedPlayerAfterDelay(CarStats hitCarStats, float delay)
    {
        yield return new WaitForSeconds(delay);
        hitCarStats.SetLastCollidedVehicle(null);  
        Debug.Log("Cleared last collided player");
    }
}