using UnityEngine;

public class MousePos : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(Screen.height);
        Debug.Log(Screen.width);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Input.mousePosition);
    }
}
