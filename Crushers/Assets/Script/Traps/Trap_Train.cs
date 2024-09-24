using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Train : MonoBehaviour
{
    private Transform train;
    [Range(0, 100f)] [SerializeField] private float trainSpeed = 20f;
    [SerializeField] float timeToLive = 5f;
    // Start is called before the first frame update
    void Start()
    {
        train = GetComponent<Transform>();
        StartCoroutine(KillTrainEntity());
    }

    // Update is called once per frame
    void Update()
    {
        train.Translate(Vector3.back * (Time.deltaTime * trainSpeed)); // blue to red
    }

    IEnumerator KillTrainEntity()
    {
        yield return new WaitForSeconds(timeToLive);
        Destroy(gameObject);
    }
}
