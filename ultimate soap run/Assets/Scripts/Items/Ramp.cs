using System;
using UnityEngine;

public class Ramp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Rigidbody>().AddTorque(0,0,100);
    }
}
