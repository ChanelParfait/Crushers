using System.Collections;
using UnityEngine;

public class GrapplingHookProjectile : MonoBehaviour
{
    private Vector3 enemyCarPos;
    private float speed;
    private GameObject controlledCar;
    private CarController enemyCar;
    private float pullSpeed;
    private float pullOffset = 6f;
    private float holdDuration = 5f; 
    private bool hasHitTarget = false;

    public void Initialize(Vector3 enemyCarPos, float hookSpeed, GameObject controlledCar, CarController enemyCar, float pullSpeed)
    {
        this.enemyCarPos = enemyCarPos;
        this.speed = hookSpeed;
        this.controlledCar = controlledCar;
        this.enemyCar = enemyCar;
        this.pullSpeed = pullSpeed;
    }

    private void Update()
    {
        if (!hasHitTarget)
        {
            MoveTowardsTarget();
        }
    }

    private void MoveTowardsTarget()
    {
        // Move the grappling hook toward the target
        transform.position = Vector3.MoveTowards(transform.position, enemyCarPos, speed * Time.deltaTime);

        // Check if the hook reaches the target
        if (Vector3.Distance(transform.position, enemyCarPos) < 0.1f)
        {
            hasHitTarget = true;
            StartCoroutine(PullEnemyCar());
        }
    }

    private IEnumerator PullEnemyCar()
    {
        // Pull the enemy car toward the controlled car until within pullOffset
        while (Vector3.Distance(enemyCar.transform.position, controlledCar.transform.position) > pullOffset)
        {
            enemyCar.transform.position = Vector3.MoveTowards(
                enemyCar.transform.position,
                controlledCar.transform.position,
                pullSpeed * Time.deltaTime
            );

            yield return null;
        }

        // Start the hold timer
        StartCoroutine(HoldEnemyCar());
    }

    private IEnumerator HoldEnemyCar()
    {
        float timer = holdDuration;

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            // Lock the enemy car's position at pullOffset distance
            enemyCar.transform.position = controlledCar.transform.position +
                (enemyCar.transform.position - controlledCar.transform.position).normalized * pullOffset;

            yield return null;
        }

        // Release and destroy the grappling hook
        Destroy(gameObject);
    }
}
