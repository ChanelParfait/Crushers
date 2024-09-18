using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPickUp : MonoBehaviour
{
    public float Timer;

    [SerializeField] private float TimeUntilRespawnPickups = 10;

    [SerializeField] private GameObject[] listofPickupTypes;
    public void Update()
    {
        if (this.gameObject.transform.childCount == 0)
        {
            Timer = Timer + Time.deltaTime;
        }

        if (Timer > TimeUntilRespawnPickups)
        {
            SpawnPickUp();
            Timer = 0;
        }
        
    }

    private void Start()
    {
        SpawnPickUp();
    }

    private void SpawnPickUp()
    {
        Instantiate(GetRandomPickUp(),transform.position,transform.rotation).transform.parent = this.gameObject.transform;
    }

    private GameObject GetRandomPickUp()
    {
        int Random = UnityEngine.Random.Range(0, listofPickupTypes.Length);

        return (listofPickupTypes[Random]);
    }
}
