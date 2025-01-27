using Unity.VisualScripting;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    public int force;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            var contact=   collision.GetContact(0);
            collision.rigidbody.AddForce(-contact.normal * force, ForceMode.Impulse);
            Debug.Log("bounce");
        }
    }


}
