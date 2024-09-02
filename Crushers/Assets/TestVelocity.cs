using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVelocity : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            this.gameObject.GetComponent<Rigidbody>().AddForce(this.transform.forward * 500f, ForceMode.Impulse);
            Debug.Log(GetComponent<Rigidbody>().velocity.x);
        }
    }
}
