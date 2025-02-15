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

    private List<Transform> spawnLocations = new List<Transform>();

    private Dictionary<Transform, GameObject> activePickups; // Tracks active pickups at spawn locations

    private void Start()
    {
        // Initialize the dictionary
        activePickups = new Dictionary<Transform, GameObject>();

        // Get all spawn locations first
        GetAllSpawningLocations();

        // Initialize dictionary with spawn locations
        foreach (var spawnPoint in spawnLocations)
        {
            activePickups[spawnPoint] = null;
        }

        if (GetComponent<MeshRenderer>())
        {
            GetComponent<MeshRenderer>().enabled = false;
        }

        // Spawn pickups
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
        if (!activePickups.ContainsKey(spawnPoint))
        {
            Debug.LogWarning($"Spawn point {spawnPoint.name} is not in the activePickups dictionary.");
            return;
        }

        if (activePickups[spawnPoint] == null)
        {
            GameObject newPickup = Instantiate(GetRandomPickup(), spawnPoint.position, spawnPoint.rotation, transform);
            activePickups[spawnPoint] = newPickup;

            var pickupBehavior = newPickup.GetComponent<PickUpBehavior>();
            if (pickupBehavior != null)
            {
                pickupBehavior.OnPickupCollected += () => HandlePickupCollected(spawnPoint);
            }
            else
            {
                Debug.LogWarning($"Pickup prefab {newPickup.name} is missing the PickUpBehavior script.");
            }
        }
    }


    private GameObject GetRandomPickup()
    {
        GameObject[] selectedList = useSceneSpecificList ? sceneSpecificPickupList : globalPickupList;

        if (selectedList == null || selectedList.Length == 0)
        {
            Debug.LogError("No pickups available in the selected list.");
            return null;
        }

        if (highValuePickup)
        {
            float randomValue = Random.Range(0f, 100f);
            if (randomValue <= percentageOfHighPickup)
            {
                return highValuePickup;
            }
        }

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

        ShuffleList(spawnLocations); // Ensure new random locations
        SpawnPickups();
    }


    private void ClearInactivePickups()
    {
        StartCoroutine(DestroyAllPickups());
    }

    private IEnumerator DestroyAllPickups()
    {
        foreach (var spawnPoint in activePickups.Keys)
        {
            if (activePickups[spawnPoint] != null)
            {
                Destroy(activePickups[spawnPoint]);
                yield return null; // Wait a frame to ensure proper destruction
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

    private void GetAllSpawningLocations()
    {
        spawnLocations.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            spawnLocations.Add(transform.GetChild(i));
        }
    }

}
