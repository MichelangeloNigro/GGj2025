using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.linearVelocity = Vector3.zero;
            other.GetComponent<SoapController>().state=state.End;
        }
        else
        {
            Destroy(other);
        }
    }
}
