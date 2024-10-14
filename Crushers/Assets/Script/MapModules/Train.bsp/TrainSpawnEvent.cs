using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSpawnEvent : MonoBehaviour
{
    [SerializeField] GameObject trainPrefab;
    [SerializeField] GameObject spawnPoint;
    [SerializeField] private AudioSource trainWarn;
    [SerializeField] float spawnTimer = 30f;
    void Start()
    {
        StartCoroutine(SpawnTrain());
    }

    IEnumerator SpawnTrain()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTimer);
            trainWarn.Play();
            Instantiate(trainPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        }
    }
}
