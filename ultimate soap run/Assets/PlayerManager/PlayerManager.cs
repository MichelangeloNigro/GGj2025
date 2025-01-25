using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Riutilizzabile.SingletonDDOL<PlayerManager> 
{
    public List<Player> playerList;
    public int turnNumbers;

    public void instancePlayerTub(Player player)
    {
        Instantiate(player.prefabSoap);

    }
    public void startGame()
    {

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(InstantiatePlayers());
    }

    IEnumerator InstantiatePlayers()
    {
        foreach (Player player in playerList)
        {
            var soap= Instantiate(player.prefabSoap);
            player.soapIntance = soap;
            player.soapIntance.GetComponentInChildren<TrailGenerator>(true).playerColor = player.color;
            var controller = player.soapIntance.GetComponent<SoapController>();
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
        Restart();
        Debug.Log("All soaps have reached state.End! Continuing...");
    }
    private void Restart()
    {

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
