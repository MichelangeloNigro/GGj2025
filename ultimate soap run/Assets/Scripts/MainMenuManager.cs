using System.Collections.Generic;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject currPanel;
    public Dropdown dropdown;
    public List<GameObject> soaps;
    private int currSoap=0;
    public GameObject currSoapModel;
    public TMP_Text namesoap;
    public int choosingPlayer=0;
    public TMP_InputField namefield;
    private Dictionary<Color, PlayerColor> colorToPlayerColorMap;

    public void changePanel(GameObject nextPanel)
    {
        currPanel.SetActive(false);
        currPanel = nextPanel;
        currPanel.SetActive(true);
    }
    public void setNumberPlayer(int z)
    {
        PlayerManager.Instance.playerList.Clear();
        for (int i = 0; i < z + 2; i++)
        {
            PlayerManager.Instance.playerList.Add(new Player());

        }

    }

    public void changeColor()
    {
        if(currSoapModel != null)
        {
            GameObject pressedButton = EventSystem.current.currentSelectedGameObject;
            PlayerManager.Instance.playerList[choosingPlayer].color = GetPlayerColorFromColor(pressedButton.GetComponent<Image>().color);
            currSoapModel.GetComponent<MeshRenderer>().material.color =pressedButton.GetComponent<Image>().color ;
        }
    }
    public void NextPlayer()
    {
        if (PlayerManager.Instance.playerList[choosingPlayer].prefabSoap!=null && PlayerManager.Instance.playerList[choosingPlayer].color!=null && PlayerManager.Instance.playerList[choosingPlayer].name != null)
        {
            choosingPlayer++;
            if (choosingPlayer==PlayerManager.Instance.playerList.Count)
            {
                Debug.Log("startGame");
            }
            else
            {
                Destroy(currSoapModel);
                namefield.text = "Enter your Name...";

            }

        }
    }
    public void ChangeSoap(int i)
    {
        Destroy(currSoapModel);
        currSoap += i;
        if(currSoap>=soaps.Count)
        {
            currSoap = 0;
        }
        if(currSoap<0) {
            currSoap = soaps.Count-1;

        }
       currSoapModel= Instantiate(soaps[currSoap],Vector3.zero,Quaternion.identity);
        namesoap.text=currSoapModel.name.Replace("(Clone)","");
        PlayerManager.Instance.playerList[choosingPlayer].prefabSoap = soaps[currSoap];
    }
    private void Start()
    {
        InitializeColorDictionary();

    }
    public void SetName(string text)
    {
        PlayerManager.Instance.playerList[choosingPlayer].name =text;
    }
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

    public PlayerColor GetPlayerColorFromColor(Color color)
    {
        if (colorToPlayerColorMap.TryGetValue(color, out PlayerColor playerColor))
        {
            return playerColor;
        }

        Debug.LogWarning("Color not found in the dictionary.");
        return PlayerColor.Black; // Default or fallback
    }
}
