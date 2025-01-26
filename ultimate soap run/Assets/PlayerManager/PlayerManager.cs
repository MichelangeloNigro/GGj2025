using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Riutilizzabile.SingletonDDOL<PlayerManager>
{
    public List<Player> playerList;
    private int turnNumbers;
    public int maxTurns;
    public PointManager pointManager;
    public BuildingPlacer placer;
    private Dictionary<Color, PlayerColor> colorToPlayerColorMap;

    public DynamicButtonManager buttonManager;
    private void InitializeColorDictionary()
    {
        colorToPlayerColorMap = new Dictionary<Color, PlayerColor>
        {
            { Color.red, PlayerColor.Red },
            { Color.blue, PlayerColor.Blue },
            { Color.green, PlayerColor.Green },
            {new Color(1, 0.92f, 0.016f, 1), PlayerColor.Yellow },
            { new Color(0.43f, 0, 0.404f, 1), PlayerColor.Purple },
            { Color.black, PlayerColor.Black },
            { new Color(1, 0.7f, 0.976f, 1), PlayerColor.Pink }
        };
    }
    public Color GetColorFromPlayerColor(PlayerColor playerColor)
    {
        // Reverse lookup in the dictionary
        foreach (var pair in colorToPlayerColorMap)
        {
            if (pair.Value == playerColor)
            {
                return pair.Key;
            }
        }

        Debug.LogWarning("PlayerColor not found in the dictionary.");
        return Color.black; // Default or fallback
    }
    public PlayerColor GetPlayerColorFromColor(Color color)
    {
        if (colorToPlayerColorMap.TryGetValue(color, out PlayerColor playerColor))
        {
            return playerColor;
        }

        Debug.LogWarning("Color not found in the dictionary.");
        return PlayerColor.Black; // Default or fallback
    }
    
    private void Start()
    {
        InitializeColorDictionary();
        StartCoroutine(StartGame());

    }
    public IEnumerator StartGame()
    {
        pointManager = FindObjectOfType<PointManager>();
        while (turnNumbers < maxTurns)
        {
            yield return StartCoroutine(InstantiatePlayers()); // Wait for InstantiatePlayers to complete
            turnNumbers++; // Increment turnNumbers after InstantiatePlayers is done
            if(turnNumbers >= maxTurns)
                break;
            
            placer.state = PlaceState.Pick;
            buttonManager.StartPlacing();
            
            while (placer.state != PlaceState.End)
            {
                yield return null;
            }
        }
        
        pointManager.CalculateFinalScore();

        Debug.Log("All turns are completed!");
    }

    public void Begin()
    {
        StartCoroutine(StartGame());
    }
    IEnumerator InstantiatePlayers()
    {
        foreach (Player player in playerList)
        {
            var soap = Instantiate(player.prefabSoap);
            player.soapIntance = soap;
            player.soapIntance.GetComponentInChildren<TrailGenerator>(true).playerColor = player.color;
            var controller = player.soapIntance.GetComponent<SoapController>();
            soap.GetComponent<MeshRenderer>().material.color =GetColorFromPlayerColor( player.color);
            controller.enabled = true;
            player.soapController = controller;
            // Wait until soap.state equals state.wait
            while (controller.state != state.Waiting)
            {
                Debug.Log(controller.state);
                yield return null;
                // Wait for the next frame
            }

            // Instantiate the player's soap
            Debug.Log($"Instantiated soap for {player.name}");
        }

        foreach (Player player in playerList)
        {
            player.soapIntance.GetComponent<SoapController>().state = state.Moving;
        }

        while (!AllSoapsAreInEndState())
        {
            Debug.Log("Waiting for all soaps to reach state.End...");
            yield return null; // Wait for the next frame
        }

        Debug.Log("All soaps have reached state.End! Continuing...");
        pointManager.PrintPlayerPointPercentages();
        
        Restart();
    }

    private void Restart()
    {
        foreach (Player player in playerList)
        {
            player.totalPoints += player.soapIntance.GetComponentInChildren<PlayerPointComponent>().playerPoints.Count;
            Debug.Log(player.name + " " + player.totalPoints);
            pointManager.ClearPlayerList();
            Destroy(player.soapIntance);
            foreach (var particleSystem in FindObjectsOfType<ParticleSystem>())
            {
                Destroy(particleSystem.gameObject);
            }
        }
    }

    private bool AllSoapsAreInEndState()
    {
        foreach (Player player in playerList)
        {
            var controller = player.soapIntance.GetComponent<SoapController>();
            if (controller == null || controller.state != state.End)
            {
                return false; // If any soap is not in the "End" state, return false
            }
        }

        return true; // All soaps are in the "End" state
    }
}