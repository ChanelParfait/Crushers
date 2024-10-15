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
                CarStats collidedVehicle = collision.gameObject.GetComponentInParent<CarStats>();
                Debug.Log("Collision with Player" + collidedVehicle.name);
                
                if (collidedVehicle)
                {
                    // Set this vehicles last collided to the collided player
                    carStats.SetLastCollidedVehicle(collidedVehicle);
                    // Set the collided vehicles last collided to this vehicle
                    collidedVehicle.SetLastCollidedVehicle(carStats);
                    
                    // apply damage
                    float newDamage = Mathf.Round(carStats.GetSpeed() / 3);
                    collidedVehicle.IncreaseDamage(newDamage);
                    collidedVehicle.AddCentreOfMass(newDamage);

                    //Debug.Log("Collided vehicle damage: " + collidedVehicle.GetDamage());
                }

                
            }
            else
            {
                Debug.Log("Not a vehicle");
            }
        }
    }

}