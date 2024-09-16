using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class RespawnPickUpManager : MonoBehaviour
{

    [SerializeField] private GameObject[] listofPickupTypes;
    
    [SerializeField] private GameObject[] ListofPickUpLocations;

    private void Start()
    {
        foreach (GameObject PickUp in ListofPickUpLocations)
        {
            SpawnPickUp(PickUp);
        }
    }

    private void Update()
    {
        if (ListofPickUpLocations.Length != 0)
        {
            foreach (var PickUp in ListofPickUpLocations)
            {
                if (PickUp.transform.childCount < 1)
                {
                    
                }
            }
        }
    }

    private void SpawnPickUp(GameObject gameObjectpos)
    {
        Instantiate(RandomPickUp(), gameObjectpos.transform.position,transform.rotation).transform.parent = gameObjectpos.transform;
    }

    private GameObject RandomPickUp()
    {
        int Random = UnityEngine.Random.Range(0, listofPickupTypes.Length);

        return (listofPickupTypes[Random]);
    }
}
