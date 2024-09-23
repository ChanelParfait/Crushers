using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Train : MonoBehaviour
{
    private Transform train;
    [SerializeField] private float trainSpeed = 20f;
    // Start is called before the first frame update
    void Start()
    {
        train = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        train.Translate(Vector3.back * (Time.deltaTime * trainSpeed)); // blue to red
    }
}
