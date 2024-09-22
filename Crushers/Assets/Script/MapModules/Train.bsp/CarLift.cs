using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CarLift : MonoBehaviour
{
    [SerializeField] private Transform lift;
    private bool playerOnLift = false;
    private Vector3 startingPosition;
    private float timer = 0; // for lift reset OnTriggerExit()
    [Range(0, 10.0f)] [SerializeField] float liftSpeed;
    [Range(0, 100.0f)] [SerializeField] private float newPosition_y; // adjust where it goes!
    
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = lift.position;
    }

    private void Update()
    {
        // Debugging where the elevator will go
        Debug.DrawLine(startingPosition,
            new Vector3(startingPosition.x, startingPosition.y + newPosition_y, startingPosition.z),
            Color.red,
            0.2f
        );
        if (playerOnLift && lift.position.y < startingPosition.y + newPosition_y){
            timer += Time.deltaTime;
            if (timer >= 1.5) {
                lift.Translate(Vector3.up * Time.deltaTime * liftSpeed);
            }
        }
        if (!playerOnLift) {
            timer += Time.deltaTime;
            if (timer >= 3) lift.position = startingPosition;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timer = 0;
            playerOnLift = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        timer = 0;
        playerOnLift = false;
    }
}
