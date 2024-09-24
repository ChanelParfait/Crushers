using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSpawnEvent : MonoBehaviour
{
    [SerializeField] GameObject trainPrefab;
    [SerializeField] GameObject spawnPoint;
    [SerializeField] float spawnTimer = 30f;
    void Start()
    {
        StartCoroutine(SpawnTrain());
    }

    IEnumerator SpawnTrain()
    {
        yield return new WaitForSeconds(spawnTimer);
        Instantiate(trainPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        StartCoroutine(SpawnTrain());
    }
}
