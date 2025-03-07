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
    [SerializeField] private int MaxPickups = 0;
    private List<Transform> TotalSpawnLocations = new List<Transform>();
    private List<Transform> AvailableSpawnLocations = new List<Transform>();

    //private Dictionary<Transform, GameObject> activePickups; // Tracks active pickups at spawn locations

    private void Start()
    {
        // Get all spawn locations first
        GetAllSpawningLocations();
        AvailableSpawnLocations = TotalSpawnLocations;

        // Spawn pickups
        SpawnAllPickups();
    }


    private void SpawnAllPickups()
    {
        // Spawn pickups at randomly chosen locations
        int pickupsToSpawn = Mathf.Min(MaxPickups, AvailableSpawnLocations.Count);
        for (int i = 0; i < pickupsToSpawn; i++)
        {
            // Select a Random Spawn Point from available locations
            Transform spawn = AvailableSpawnLocations[Random.Range(0, AvailableSpawnLocations.Count - 1)]; 
            SpawnPickup(GetRandomPickup(), spawn);
            // Remove Spawn Point from Available Locations
            AvailableSpawnLocations.Remove(spawn);
        }
    }

    private void SpawnPickup(GameObject pickup, Transform spawn)
    {
        if (!AvailableSpawnLocations.Contains(spawn))
        {
            Debug.LogWarning($"Spawn point {spawn.name} is not available");
            return;
        }
        GameObject newPickup = Instantiate(pickup, spawn.position, spawn.rotation, transform);
        AvailableSpawnLocations.Remove(spawn);


        BasePickUp pickupBehavior = newPickup.GetComponent<BasePickUp>();
        if (pickupBehavior)
        {
            pickupBehavior.OnPickupCollected += () => HandlePickupCollected(spawn);
        }
        else
        {
            Debug.LogWarning($"Prefab {newPickup.name} is missing the PickUpBehavior script.");
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
        //Debug.Log($"Pickup collected at {spawnPoint.name}");
        AvailableSpawnLocations.Add(spawnPoint);
        
        StartCoroutine(SpawnPickupWithDelay());
    }

    private IEnumerator SpawnPickupWithDelay()
    {
        //Debug.Log("Respawn Pickup");
        float randomDelay = Random.Range(3f, 18f);
        yield return new WaitForSeconds(randomDelay);

        Transform spawn = AvailableSpawnLocations[Random.Range(0, AvailableSpawnLocations.Count - 1)]; 
        SpawnPickup(GetRandomPickup(), spawn);
    }


    private void GetAllSpawningLocations()
    {
        TotalSpawnLocations.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            TotalSpawnLocations.Add(transform.GetChild(i));
        }
        
    }

}
