using UnityEngine;

public class Ramp : MonoBehaviour
{
    [SerializeField] private float rampBoost = 5f;
    public AudioSource jumpsound;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jumpsound.Play();
            Vector3 boostDirection = gameObject.transform.forward * rampBoost;
            other.GetComponent<Rigidbody>().AddTorque(0, 0, 500);
            other.GetComponent<Rigidbody>().AddForce(boostDirection, ForceMode.Acceleration);
        }
    }
}
