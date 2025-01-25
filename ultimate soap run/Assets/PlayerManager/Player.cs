using System;
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
}
