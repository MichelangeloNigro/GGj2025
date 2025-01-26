using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class saponestart : MonoBehaviour
{
    private void Start()
    {
        SoundEngine.Instance.PlayOST("Menu");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("loading"))
        {
            SceneManager.LoadScene(1);
        }
    }
   
}
