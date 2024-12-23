using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawnManager : MonoBehaviour
{   
    [Header("Spawn Settings")]
    [SerializeField] private bool useSceneSpecificList = false;
    [SerializeField] private GameObject[] globalPickupList;
    [SerializeField] private GameObject[] sceneSpecificPickupList;

    [Header("Pickup Settings")]
    [SerializeField] private GameObject highValuePickup;
    [SerializeField] private float percentageOfHighPickup = 10f;
    [SerializeField] private int maxPickupsPerCycle = 3;

    [Header("Spawn Locations")]
    [SerializeField] private List<Transform> spawnLocations;

    private Dictionary<Transform, GameObject> activePickups; // Tracks active pickups at spawn locations

    private void Start()
    {
        activePickups = new Dictionary<Transform, GameObject>();
        foreach (var spawnPoint in spawnLocations)
        {
            activePickups[spawnPoint] = null;
        }

        if (GetComponent<MeshRenderer>())
        {
            GetComponent<MeshRenderer>().enabled = false;
        }

        SpawnPickups();
    }

    private void SpawnPickups()
    {
        // Shuffle the spawn locations for randomness
        List<Transform> availableLocations = new List<Transform>(spawnLocations);
        ShuffleList(availableLocations);

        // Clear previously used pickups
        ClearInactivePickups();

        // Spawn pickups at randomly chosen locations
        int pickupsToSpawn = Mathf.Min(maxPickupsPerCycle, availableLocations.Count);
        for (int i = 0; i < pickupsToSpawn; i++)
        {
            Transform spawnPoint = availableLocations[i];
            SpawnPickupAtLocation(spawnPoint);
        }
    }

    private void SpawnPickupAtLocation(Transform spawnPoint)
    {
        // Check if the location is valid for spawning
        if (activePickups[spawnPoint] == null)
        {
            GameObject newPickup = Instantiate(GetRandomPickup(), spawnPoint.position, spawnPoint.rotation, transform);
            activePickups[spawnPoint] = newPickup;

            // Add a callback for when the pickup is collected
            var pickupBehavior = newPickup.GetComponent<PickUpBehavior>();
            if (pickupBehavior != null)
            {
                pickupBehavior.OnPickupCollected += () => HandlePickupCollected(spawnPoint);
            }
            else
            {
                Debug.LogWarning("Pickup prefab is missing the PickUpBehavior script.");
            }
        }
        else
        {
            Debug.LogWarning($"Spawn point {spawnPoint.name} is already occupied.");
        }
    }

    private GameObject GetRandomPickup()
    {
        // Determine which pickup list to use
        GameObject[] selectedList = useSceneSpecificList ? sceneSpecificPickupList : globalPickupList;

        // Check for high-value pickup chance
        if (highValuePickup)
        {
            float randomValue = Random.Range(0f, 100f);
            if (randomValue <= percentageOfHighPickup)
            {
                return highValuePickup;
            }
        }

        // Select random pickup from the chosen list
        int randomIndex = Random.Range(0, selectedList.Length);
        return selectedList[randomIndex];
    }

    private void HandlePickupCollected(Transform spawnPoint)
    {
        Debug.Log($"Pickup collected at {spawnPoint.name}");
        // Remove the collected pickup
        activePickups[spawnPoint] = null;

        // Respawn pickups with new random spots after a delay
        StartCoroutine(RespawnAllPickupsWithDelay());
    }

    private IEnumerator RespawnAllPickupsWithDelay()
    {
        float randomDelay = Random.Range(5f, 10f);
        yield return new WaitForSeconds(randomDelay);

        // Respawn pickups at new random locations
        SpawnPickups();
    }

    private void ClearInactivePickups()
    {
        // Destroy inactive pickups and clear their references
        foreach (var spawnPoint in activePickups.Keys)
        {
            if (activePickups[spawnPoint] != null)
            {
                Destroy(activePickups[spawnPoint]);
                activePickups[spawnPoint] = null;
            }
        }
    }

    private void ShuffleList(List<Transform> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }
}
