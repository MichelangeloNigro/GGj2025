using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PointManager : MonoBehaviour
{
    public List<Point> pointsClaimed = new List<Point>();
    public List<PlayerPointComponent> playerComponentList = new List<PlayerPointComponent>();
    private PlayerManager playerManager;
    
    public int numberOfPoints = 0;

    private void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
    }

    public void AddPoint(Point point)
    {
        pointsClaimed.Add(point);
        numberOfPoints++;
    }

    public void ClearPlayerList()
    {
        playerComponentList.Clear();
    }

    public Dictionary<PlayerPointComponent, float> CalculatePlayerPointPercentages()
    {
        // Dictionary to store each player's percentage
        Dictionary<PlayerPointComponent, float> playerPercentages = new Dictionary<PlayerPointComponent, float>();

        // Calculate the total points of all players
        int totalPoints = 0;
        foreach (PlayerPointComponent player in playerComponentList)
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
        foreach (PlayerPointComponent player in playerComponentList)
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

    public void CalculateFinalScore()
    {
        List<float> highScore = new List<float>();

        foreach (var player in playerManager.playerList)
        {
            highScore.Add(player.totalPoints);
        }
        
        highScore.Sort();
        
        Console.WriteLine("Sorted list (smaller to higher):");
        foreach (float num in highScore)
        {
            Debug.Log(num);
        }
    }
}