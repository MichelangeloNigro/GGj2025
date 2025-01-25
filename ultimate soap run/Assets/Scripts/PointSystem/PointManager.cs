using System;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public List<Point> pointsClaimed = new List<Point>();
    public List<PlayerPointComponent> playerList = new List<PlayerPointComponent>();
    
    public int numberOfPoints = 0;

    public void AddPoint(Point point)
    {
        pointsClaimed.Add(point);
        numberOfPoints++;
    }

    public Dictionary<PlayerPointComponent, float> CalculatePlayerPointPercentages()
    {
        // Dictionary to store each player's percentage
        Dictionary<PlayerPointComponent, float> playerPercentages = new Dictionary<PlayerPointComponent, float>();

        // Calculate the total points of all players
        int totalPoints = 0;
        foreach (PlayerPointComponent player in playerList)
        {
            totalPoints += player.playerPoints.Count;
        }

        // Avoid division by zero
        if (totalPoints == 0)
        {
            Debug.LogWarning("No points have been assigned to players yet.");
            return playerPercentages; // Empty dictionary
        }

        // Calculate percentage for each player
        foreach (PlayerPointComponent player in playerList)
        {
            float percentage = (float)player.playerPoints.Count / totalPoints * 100;
            playerPercentages[player] = percentage;
        }

        return playerPercentages;
    }

    public void PrintPlayerPointPercentages()
    {
        Dictionary<PlayerPointComponent, float> percentages = CalculatePlayerPointPercentages();

        foreach (KeyValuePair<PlayerPointComponent, float> entry in percentages)
        {
            Debug.Log($"Player: {entry.Key.name}, Points: {entry.Key.playerPoints.Count}, Percentage: {entry.Value:F2}%");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PrintPlayerPointPercentages();
        }
    }
}