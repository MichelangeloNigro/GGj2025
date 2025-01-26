using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPointComponent : MonoBehaviour
{
    private PointManager pointManager;
    
    public List<Point> playerPoints = new List<Point>();

    private void Start()
    {
        pointManager = FindObjectOfType<PointManager>();
        pointManager.playerComponentList.Add(this);
    }

    public void AddPoint(Point point)
    {
        playerPoints.Add(point);
    }

    public void RemovePoint(Point point)
    {
        playerPoints.Remove(point);
    }

    private void OnDisable()
    {
        pointManager.playerComponentList.Remove(this);
    }
}
