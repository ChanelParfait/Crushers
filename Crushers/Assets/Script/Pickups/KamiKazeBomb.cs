using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class KamiKazeBomb : MonoBehaviour
{
    [SerializeField] private float growShrinkDuration = 10f;   // Duration before the bomb explodes
    [SerializeField] private Vector3 minScale = new Vector3(1f, 1f, 1f);  // Minimum scale
    [SerializeField] private Vector3 maxScale = new Vector3(2f, 2f, 2f);  // Maximum scale
    [SerializeField] private CarStats Player;
    [SerializeField] private GameObject ExplosionVFX;
    [SerializeField] private AudioClip[] explosionSFX;
    
    public void SetPlayer(CarStats player)
    {
        Player = player;
    }
    
    void Update()
    {
        // Grow and shrink the bomb's scale
        float scaleFactor = Mathf.PingPong(Time.time, growShrinkDuration / 2f) / (growShrinkDuration / 2f);
        transform.localScale = Vector3.Lerp(minScale, maxScale, scaleFactor);
    }

    private void OnDestroy()
    {
        GameObject Explosion =  Instantiate(ExplosionVFX, this.gameObject.transform.position, this.gameObject.transform.rotation);
        Explosion.GetComponent<Explosion>().SetTimeBeforeDestruction(1);

        // select random explosion SFX
        AudioClip activeClip = explosionSFX[Random.Range(0,explosionSFX.Length - 1)];
        AudioSource.PlayClipAtPoint(activeClip, transform.position);
    }
}
