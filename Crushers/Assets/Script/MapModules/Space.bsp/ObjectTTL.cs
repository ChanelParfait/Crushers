using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTTL : MonoBehaviour
{
    [SerializeField] private float timeToLive;
    private void Awake()
    {
        StartCoroutine(SelfDestruct());
    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(timeToLive);
        
        Destroy(gameObject);
    }
}
