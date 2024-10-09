using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkSpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private GameObject junkPrefab;
    [SerializeField] private float forceMagnitude = 10f;
    [SerializeField] private float cooldown = 0.5f;

    void Start()
    {
        forceMagnitude *= 100;
        StartCoroutine(SpawnJunk());
    }

    private IEnumerator SpawnJunk()
    {
        while (true)
        {
            yield return new WaitForSeconds(cooldown);
            
            var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            GameObject junk = Instantiate(junkPrefab, spawnPoint.position, spawnPoint.rotation);
            Rigidbody rb = junk.GetComponent<Rigidbody>();
            
            if (rb != null)
            {
                rb.AddForce(spawnPoint.up.normalized * forceMagnitude, ForceMode.Impulse);
            }
        }
    }
}