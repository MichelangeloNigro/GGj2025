using UnityEngine;

public class MUSIC : MonoBehaviour
{
    private void Start()
    {
        SoundEngine.Instance.PlayOST("Menu");
    }
}
