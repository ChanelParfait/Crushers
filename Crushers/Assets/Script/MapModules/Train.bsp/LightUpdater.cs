using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightUpdater : MonoBehaviour
{
    // IMat -> IndicatorMaterial
    [SerializeField] Material greenIMat, amberIMat, redIMat;

    [SerializeField] private TrainSpawnEvent _trainSpawnEvent; // this probably could be better lol
    private float trainSpawnTime;
    private float timer, timer2;
    
    // Start is called before the first frame update
    void Start()
    {
        trainSpawnTime = _trainSpawnEvent.GetTrainTimer();
        if (trainSpawnTime == 0) trainSpawnTime = 1; // avoid div by 0
        timer = trainSpawnTime; timer2 = trainSpawnTime;
        Light_Green();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        timer2 -= Time.deltaTime;
        
        if (timer2 <= 0)
        {
            // sync with trainspawner corountine
            timer2 = _trainSpawnEvent.GetTrainTimer();
        }
        
        // this should be trainTimetoLive var
        if (timer <= -trainSpawnTime / 3) // 30sec -> -10sec
        {
            Light_Green();
            timer = timer2; // update first timer to current coroutine time
        }
        else if (timer <= trainSpawnTime / 10) // 30sec -> 3sec
        {
            Light_Red();
        }
        else if (timer <= trainSpawnTime / 3) // 30sec -> 10sec
        {
            Light_Amber();
        }
    }

    void Light_Green()
    {
        redIMat.DisableKeyword("_EMISSION");
        greenIMat.EnableKeyword("_EMISSION");
    }

    void Light_Amber()
    {
        greenIMat.DisableKeyword("_EMISSION");
        amberIMat.EnableKeyword("_EMISSION");
    }

    void Light_Red()
    {
        amberIMat.DisableKeyword("_EMISSION");
        redIMat.EnableKeyword("_EMISSION");
    }
}
