using System;
using UnityEngine;

public class Duckie : MonoBehaviour
{
    [SerializeField] private float impulseStrength = 10f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector3 impulse = Vector3.up * impulseStrength;
            rb.AddForce(impulse, ForceMode.Acceleration);
        }
    }

    private void BounceByNormals(Collision other)
    {
        ContactPoint[] contacts = new ContactPoint[other.contactCount];

        int contactCount = other.GetContacts(contacts);

        Vector3 averageNormal = Vector3.zero;

        for (int i = 0; i < contactCount; i++)
        {
            averageNormal += contacts[i].normal;
        }

        if (contactCount > 0)
        {
            averageNormal /= contactCount;
        }

        averageNormal = averageNormal.normalized;

        Vector3 impulse = averageNormal * impulseStrength;

        rb.AddForce(impulse, ForceMode.Impulse);
    }
}