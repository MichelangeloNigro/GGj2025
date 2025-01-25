using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject currPanel;
    public Dropdown dropdown;
    public List<GameObject> soaps;
    private int currSoap=0;

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
    public void ChangeSoap(int i)
    {
        currSoap += i;
        if(currSoap>soaps.Count)
        {
            currSoap = 0;
        }
        if(currSoap<0) {
            currSoap = soaps.Count;

        }
        Instantiate(soaps[currSoap]);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
