using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip[] splashSFX;
    private void Awake(){
        source = GetComponent<AudioSource>();
    
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Length" + splashSFX.Length);
        source.clip = splashSFX[Random.Range(0, splashSFX.Length - 1)];
        source.Play();
    }
}
