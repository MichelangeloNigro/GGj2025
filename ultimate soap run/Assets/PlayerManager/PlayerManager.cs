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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        pointManager = FindObjectOfType<PointManager>();
        while (turnNumbers < maxTurns)
        {
            yield return StartCoroutine(InstantiatePlayers()); // Wait for InstantiatePlayers to complete
            turnNumbers++; // Increment turnNumbers after InstantiatePlayers is done
        }

        Debug.Log("All turns are completed!");
    }


    IEnumerator InstantiatePlayers()
    {
        foreach (Player player in playerList)
        {
            var soap= Instantiate(player.prefabSoap);
            player.soapIntance = soap;
            player.soapIntance.GetComponentInChildren<TrailGenerator>(true).playerColor = player.color;
            var controller = player.soapIntance.GetComponent<SoapController>();
            player.soapController= controller;
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
        placer.state = PlaceState.Pick;
        Restart();
        while (placer.state != PlaceState.End)
        {
            yield return null;
        }
    }
    private void Restart()
    {
        foreach(Player player in playerList)
        {
            Destroy(player.soapIntance);
foreach (var particleSystem in FindObjectsOfType<ParticleSystem>())
{
    Destroy(particleSystem.gameObject);
}        }

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


    // Update is called once per frame
    void Update()
    {
        
    }
}
