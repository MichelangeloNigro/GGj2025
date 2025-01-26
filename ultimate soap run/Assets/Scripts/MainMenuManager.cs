using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenuManager : MonoBehaviour
{
    public GameObject currPanel;
    public List<GameObject> soaps;
    private int currSoap=0;
    public GameObject currSoapModel;
    public TMP_Text namesoap;
    public int choosingPlayer=0;
    public TMP_Text namefield;
    private Dictionary<Color, PlayerColor> colorToPlayerColorMap;
    public GameObject spawnObj;
    public TMP_Text description;
    public List <GameObject> aromafill;
    public List <GameObject> flavofill;
    public List <GameObject> gaithfill;
    public List <GameObject> shapefill;
    public List<GameObject> colorfill;
    public ParticleSystem bubblestart;
    public LayerMask uiSoap;
    public AudioSource clickButton;
    public AudioClip ok;
    public AudioClip next;
    public AudioClip bubble;

    
    public void changePanel(GameObject nextPanel)
    {
        clickButton.PlayOneShot(ok);
        currPanel.SetActive(false);
        currPanel = nextPanel;
        currPanel.SetActive(true);
    }
    public void setNumberPlayer(int z)
    {
        clickButton.PlayOneShot(next);

        //if (PlayerManager.Instance.playerList[z] != null)
        //{
        //    PlayerManager.Instance.playerList.RemoveAt(z);
        //}
        //else
        //{
        //    PlayerManager.Instance.playerList[z] = new Player();
        //}
        if (PlayerManager.Instance.playerList.Count==z) {
            PlayerManager.Instance.playerList.Clear();
            for (int i = 0; i < z-1; i++)
            {
                PlayerManager.Instance.playerList.Add(new Player());

            }


        }
        else
        {
            PlayerManager.Instance.playerList.Clear();

            for (int i = 0; i < z; i++)
            {
                PlayerManager.Instance.playerList.Add(new Player());

            }
        }
       

    }

    public void changeColor()
    {
        if(currSoapModel != null)
        {
            GameObject pressedButton = EventSystem.current.currentSelectedGameObject;
            PlayerManager.Instance.playerList[choosingPlayer].color = GetPlayerColorFromColor(pressedButton.GetComponent<Button>().colors.disabledColor);
            currSoapModel.GetComponent<MeshRenderer>().material.color = pressedButton.GetComponent<Button>().colors.disabledColor;
        }
    }
    public void NextPlayer()
    {
        clickButton.PlayOneShot(ok);
        if (PlayerManager.Instance.playerList[choosingPlayer].prefabSoap!=null && PlayerManager.Instance.playerList[choosingPlayer].color!=null)
        {
            choosingPlayer++;
            if (choosingPlayer==PlayerManager.Instance.playerList.Count)
            {
                Debug.Log("startGame");
                Destroy(currSoapModel);
                currPanel.SetActive(false);
                StartCoroutine(setbubbleandStart());
            }
            else
            {
                Destroy(currSoapModel);

            }
            namefield.text = (choosingPlayer + 1).ToString();
        }
    }
    public IEnumerator setbubbleandStart()
    {
        clickButton.PlayOneShot(bubble);
        bubblestart.Play();
        yield return new WaitForSeconds(bubblestart.startLifetime);
        PlayerManager.Instance.Begin();
    }
    public void ChangeSoap(int i)
    {
        clickButton.PlayOneShot(next);
        Destroy(currSoapModel);
        currSoap += i;
        spawnObj.GetComponent<Animator>().SetTrigger("selected");

        if (currSoap>=soaps.Count)
        {
            currSoap = 0;
        }
        if(currSoap<0) {
            currSoap = soaps.Count-1;

        }
        currSoapModel= Instantiate(soaps[currSoap], spawnObj.transform.position, soaps[currSoap].transform.rotation,spawnObj.transform);
        currSoapModel.layer =7;
        namesoap.text=currSoapModel.name.Replace("(Clone)","");
        PlayerManager.Instance.playerList[choosingPlayer].prefabSoap = soaps[currSoap];
        description.text = soaps[currSoap].GetComponent<SoapController>().description;
        setStats();
    }
    private void Start()
    {
        InitializeColorDictionary();
        SoundEngine.Instance.PlayOST("Player");

    }
    public void SetName(string text)
    {
        PlayerManager.Instance.playerList[choosingPlayer].name =text;
    }
    private void InitializeColorDictionary()
    {
        colorToPlayerColorMap = new Dictionary<Color, PlayerColor>
        {
            { new Color(0.93f, 0.22f, 0.19f, 1), PlayerColor.Red },
            { new Color(0.46f, 0.71f, 0.87f, 1), PlayerColor.Blue },
            { new Color(0.50f, 0.78f, 0.48f, 1), PlayerColor.Green },
            {new Color(0.87f, 0.70f, 0.45f, 1), PlayerColor.Yellow },
            { new Color(0.70f, 0.49f, 0.78f, 1), PlayerColor.Purple },
            {  new Color(0.40f, 0.94f, 0.92f, 1), PlayerColor.celeste },
            { new Color(0.87f, 0.36f, 0.67f, 1), PlayerColor.Pink },
            { Color.black,PlayerColor.Black}
        };
    }
    public void setStats() {
        foreach (var item in aromafill)
        {
            item.SetActive(false);
        }
        foreach (var item in flavofill)
        {
            item.SetActive(false);
        }
        foreach (var item in gaithfill)
        {
            item.SetActive(false);
        }
        foreach (var item in shapefill)
        {
            item.SetActive(false);
        }
        foreach (var item in colorfill)
        {
            item.SetActive(false);
        }
        var currcontroller = soaps[currSoap].GetComponent<SoapController>();
        for (int i = 0; i < currcontroller.aroma; i++)
        {
            aromafill[i].SetActive(enabled);
        }
        for (int j = 0; j < currcontroller.flavor; j++)
        {
            flavofill[j].SetActive(enabled);
        }
        for (int k = 0;k  < currcontroller.faith; k++)
        {
            gaithfill[k].SetActive(enabled);
        }
        for (int i = 0; i < currcontroller.shape; i++)
        {
            shapefill[i].SetActive(enabled);
        }
        for (int i = 0; i < currcontroller.colourstat; i++)
        {
            colorfill[i].SetActive(enabled);
        }
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
