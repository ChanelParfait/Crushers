using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnRandomPickUp : MonoBehaviour
{
    public float Timer;

    [SerializeField] private float TimeUntilRespawnPickups = 10;

    [SerializeField] private GameObject[] listofPickupTypes;

    [SerializeField] private GameObject HighValuePickUp;

    [SerializeField] private float percentageofHighPickup;
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
        if (GetComponent<MeshRenderer>())
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
        
    }

    private void SpawnPickUp()
    {
        Instantiate(GetRandomPickUp(),transform.position,transform.rotation).transform.parent = this.gameObject.transform;
    }

    private GameObject GetRandomPickUp()
    {
        if (HighValuePickUp)
        {
            float randomvalue = UnityEngine.Random.Range(0f, 100f);
            if (randomvalue <= percentageofHighPickup)
            {
                
                return HighValuePickUp;
            }
        }
        int Random = UnityEngine.Random.Range(0, listofPickupTypes.Length);

        return (listofPickupTypes[Random]);
        
        
    }
}
