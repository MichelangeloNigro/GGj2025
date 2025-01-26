using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum PlaceState
{
    Pick,
    Place,
    End,
    ScoreBoard
}

public class BuildingPlacer : MonoBehaviour
{
    public GameObject pickingCanvas;
    public Sprite defaultSprite;
    [Header("Point canvas")]
    public GameObject pointCanvas;
    public GameObject[] pointDisplays;
    public TMP_Text[] allNames;
    public TMP_Text[] allPoints;
    
    public static BuildingPlacer instance; // (Singleton pattern)

    public LayerMask groundLayerMask;

    protected GameObject _buildingPrefab;
    protected GameObject _toBuild;

    protected Camera _mainCamera;

    protected Ray _ray;
    protected RaycastHit _hit;

    public PlaceState state;
    private int numberOfPlayers = 4;
    private int playersThatPlaced = 0;
    private bool pointsSet = false;

    private void Awake()
    {
        instance = this; // (Singleton pattern)
        _mainCamera = Camera.main;
        _buildingPrefab = null;
        state = PlaceState.End;
    }

    private void Update()
    {
        Debug.Log(state);
        
        switch (state)
        { 
            case PlaceState.ScoreBoard:
                OpenPointUI();
                SkipPoint();
                break;
            case PlaceState.Pick:
                OpenPickMenu();
                break;
            case PlaceState.Place:
                PlaceObject(); 
                break; 
            case PlaceState.End: 
                //change game phase
                break;
        }
    }

    private void OpenPointUI()
    {
        if(!pointCanvas.activeSelf)
            pointCanvas.SetActive(true);

        if (!pointsSet)
        {
            for (int i = 0; i < PlayerManager.Instance.playerList.Count; i++)
            {
                var currentPlayer = PlayerManager.Instance.playerList[i];
                if (currentPlayer.sprite != null)
                    pointDisplays[i].GetComponent<Image>().sprite = currentPlayer.sprite;
                else
                    pointDisplays[i].GetComponent<Image>().sprite = defaultSprite;
                allNames[i].text = currentPlayer.name;
                allPoints[i].text = currentPlayer.totalPoints.ToString();
            }

            pointsSet = true;
        }
    }

    private void SkipPoint()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pointCanvas.SetActive(false);
            state = PlaceState.Pick;
        }
    }

    private void OpenPickMenu()
    {
        if (!pickingCanvas.activeSelf)
        {
            pickingCanvas.SetActive(true);
            numberOfPlayers = PlayerManager.Instance.playerList.Count;
        }
    }

    public void SetBuildingPrefab(GameObject prefab)
    {
        _buildingPrefab = prefab;
        _PrepareBuilding();
        EventSystem.current.SetSelectedGameObject(null);

        state = PlaceState.Place;
        pickingCanvas.SetActive(false);
    }

    protected virtual void _PrepareBuilding()
    {
        if (_toBuild) Destroy(_toBuild);

        _toBuild = Instantiate(_buildingPrefab, new Vector3(transform.position.x, transform.position.y + 20, transform.position.z), quaternion.identity);
        _toBuild.SetActive(false);

        BuildingManager m = _toBuild.GetComponent<BuildingManager>();
        m.isFixed = false;
        m.SetPlacementMode(PlacementMode.Valid);
    }

    private void PlaceObject()
    {
        if (_buildingPrefab != null)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                if (_toBuild.activeSelf) _toBuild.SetActive(false);
                return;
            }
            else if (!_toBuild.activeSelf) _toBuild.SetActive(true);

            // Handle rotation with the mouse scroll wheel
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.01f) // Small threshold to detect meaningful scroll
            {
                _toBuild.transform.Rotate(Vector3.up, scroll * 100f); // Adjust 100f for rotation speed
            }

            // Handle placement position
            _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit, 1000f, groundLayerMask))
            {
                if (!_toBuild.activeSelf) _toBuild.SetActive(true);
                _toBuild.transform.position = new Vector3(_hit.point.x, _hit.point.y + 3, _hit.point.z);

                // Place the building on left mouse click
                if (Input.GetMouseButtonDown(0))
                {
                    BuildingManager m = _toBuild.GetComponent<BuildingManager>();
                    if (m.hasValidPlacement)
                    {
                        m.SetPlacementMode(PlacementMode.Fixed);
                    
                        _buildingPrefab = null;
                        _toBuild = null;

                        playersThatPlaced++;
                        
                        if (playersThatPlaced >= numberOfPlayers)
                        {
                            state = PlaceState.End;
                            playersThatPlaced = 0;
                        }
                        else
                        {
                            state = PlaceState.Pick;
                        }
                    }
                }
            }
            else if (_toBuild.activeSelf) _toBuild.SetActive(false);
        }
    }

}