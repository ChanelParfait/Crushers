using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraInputHandler : MonoBehaviour, AxisState.IInputAxisProvider
{
    //[HideInInspector]
    //public InputAction horizontal;
    [HideInInspector]
    public InputAction vertical; 
    [HideInInspector]
    public Vector2 horizontal;


    public float GetAxisValue(int axis)
    {
        /*switch(axis){
            case 0: return horizontal.ReadValue<Vector2>().x;
            case 1: return horizontal.ReadValue<Vector2>().y;
            case 2: return vertical.ReadValue<float>();
        }
        
        return 0;*/

        switch(axis){
            case 0: 
                //Debug.Log("X");
                return horizontal.x;
            case 1: 
                //Debug.Log("Y");
                return horizontal.y;
            //case 2: return vertical.ReadValue<float>();
        }
        
        return 0;
    }

}
