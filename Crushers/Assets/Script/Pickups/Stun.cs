using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : MonoBehaviour
{
    [SerializeField] private float _Speed = 3000f;
    [SerializeField] private CarStats FiredBy;
    [SerializeField] private GameObject ExplosionVFX;
    [SerializeField] private float Timer;
    [SerializeField] private float Counter;

    
    private void Start()
    {
        this.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * _Speed, ForceMode.Force);
    }

    public void SetFiredBy(CarStats FiredFrom)
    {
        FiredBy = FiredFrom;
    }
    
    public CarStats GetFiredBy()
    {
        return FiredBy;
    }

    private void Update()
    {
        Counter = Counter + Time.deltaTime;
        if (Counter > Timer)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.GetComponentInParent<PrometeoCarController>().isActiveAndEnabled)
            {
                StartCoroutine(DisablePlayers(5f, other.gameObject.GetComponentInParent<PrometeoCarController>()));
            }
        }
        
    }

    IEnumerator DisablePlayers(float delay, PrometeoCarController Players)
    {
        Players.enabled = false;
        yield return new WaitForSeconds(delay);
        Players.enabled = true;
    }
}
