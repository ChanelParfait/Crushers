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
                StartCoroutine(DisablePlayers(5f, other.gameObject.GetComponentInParent<PrometeoCarController>(), other.GetComponentInParent<PickUpManager>()));
            }
        }
        
    }

    IEnumerator DisablePlayers(float delay, PrometeoCarController players, PickUpManager playerdisabledeffect)
    {
        players.enabled = false;
        if (!playerdisabledeffect.DisabledEffect.GetComponent<ParticleSystem>().isPlaying)
        {
            playerdisabledeffect.DisabledEffect.GetComponent<ParticleSystem>().Play();
        }
        yield return new WaitForSeconds(delay);
        playerdisabledeffect.DisabledEffect.GetComponent<ParticleSystem>().Stop();
        players.enabled = true;
       
    }
}
