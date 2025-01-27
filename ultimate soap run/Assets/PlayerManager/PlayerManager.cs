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
            { new Color(0.93f, 0.22f, 0.19f, 1), PlayerColor.Red },
            { new Color(0.46f, 0.71f, 0.87f, 1), PlayerColor.Blue },
            { new Color(0.50f, 0.78f, 0.48f, 1), PlayerColor.Green },
            {new Color(0.87f, 0.70f, 0.45f, 1), PlayerColor.Yellow },
            { new Color(0.70f, 0.49f, 0.78f, 1), PlayerColor.Purple },
            {  new Color(0.47f, 0.95f, 0.93f, 1), PlayerColor.celeste },
            { new Color(0.87f, 0.36f, 0.67f, 1), PlayerColor.Pink },
            { Color.black,PlayerColor.Black}
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
        //StartCoroutine(StartGame());

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
            
            placer.state = PlaceState.ScoreBoard;
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
        SoundEngine.Instance.PlayOST("Setup");
        foreach (Player player in playerList)
        {
            var soap = Instantiate(player.prefabSoap);
            player.soapIntance = soap;
            player.soapIntance.GetComponentInChildren<TrailGenerator>(true).playerColor = player.color;
            var controller = player.soapIntance.GetComponent<SoapController>();
            soap.GetComponent<MeshRenderer>().material.color = GetColorFromPlayerColor( player.color);
            controller.setColorTrail(GetColorFromPlayerColor(player.color));
            controller.color = GetColorFromPlayerColor(player.color);
            controller.enabled = true;
            player.soapController = controller;
            controller.GetComponentInChildren<TrailGenerator>(true).playerColor = player.color;
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
            if (maxTurns - turnNumbers == 1)
            {
                SoundEngine.Instance.PlayOST("Play2");


            }
            else
            {
                SoundEngine.Instance.PlayOST("Play1");
            }
            player.soapIntance.GetComponent<SoapController>().state = state.Moving;
        }

        while (!AllSoapsAreInEndState())
        {
            Debug.Log("Waiting for all soaps to reach state.End...");
            yield return null; // Wait for the next frame
        }

        Debug.Log("All soaps have reached state.End! Continuing...");
        SoundEngine.Instance.PlayOST("Setup");
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