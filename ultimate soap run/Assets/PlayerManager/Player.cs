using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player
{
    public string name;
    public PlayerColor color;
    public GameObject prefabSoap;
    public GameObject soapIntance;
    public SoapController soapController;
    public PlayerPointComponent points;
    public float soapLeft;
    public int totalPoints;
    public Sprite sprite;

    public void AssignSprite(Player player, Dictionary<(PlayerColor, string), Sprite> spriteDictionary)
    {
        if (player.prefabSoap != null)
        {
            string soapName = player.prefabSoap.name;

            // Use the tuple (color, soapName) to get the sprite
            var key = (player.color, soapName);

            if (spriteDictionary.ContainsKey(key))
            {
                player.sprite = spriteDictionary[key];
            }
            else
            {
                Debug.LogWarning(
                    $"No sprite found for player: {player.name} with color: {player.color} and prefabSoap: {soapName}");
            }
        }
        else
        {
            Debug.LogWarning($"Player: {player.name} does not have a valid prefabSoap.");
        }
    }
}