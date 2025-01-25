using System;
using UnityEngine;

public class Point : MonoBehaviour
{
    private PointManager pointManager;
    private bool isAssigned = false;
    private PlayerPointComponent previousPlayer = null;

    private void Start()
    {
        pointManager = FindObjectOfType<PointManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (previousPlayer == null)
            {
                previousPlayer = other.GetComponent<PlayerPointComponent>();
                pointManager.AddPoint(this);
            }
            else
            {
                previousPlayer.RemovePoint(this);
            }
            
            other.GetComponent<PlayerPointComponent>().AddPoint(this);
        }
    }
}
