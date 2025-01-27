using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicButtonManager : MonoBehaviour
{
    [Header("Button References (Max 4)")]
    public Button[] buttons;

    [Header("Available Items")]
    public List<GameObject> items;

    private int currentIndex = 0; 
    private List<GameObject> shuffledItems;

    public void StartPlacing()
    {
        if (buttons.Length > 4)
        {
            Debug.LogError("Maximum of 4 buttons are allowed.");
            return;
        }

        ShuffleItems();

        UpdateButtons();
    }

    public void UpdateButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i + currentIndex < shuffledItems.Count)
            {
                GameObject item = shuffledItems[i + currentIndex];
                Button button = buttons[i];

                // Access the Image component and update the sprite
                Image buttonImage = button.GetComponentInChildren<Image>();
                Sprite itemSprite = item.GetComponentInChildren<SpriteRenderer>()?.sprite; // Assumes the item has a SpriteRenderer
                if (buttonImage != null && itemSprite != null)
                {
                    buttonImage.sprite = itemSprite;
                }
                else if (buttonImage == null)
                {
                    Debug.LogError($"Button {i} is missing an Image component.");
                }
                else
                {
                    Debug.LogError($"Item {item.name} is missing a SpriteRenderer or Sprite.");
                }

                // Set up the button click event
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => AssignToBuildingPlacer(item, button));
                button.gameObject.SetActive(true);
            }
            else
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
    }

    private void AssignToBuildingPlacer(GameObject item, Button button)
    {
        if (BuildingPlacer.instance != null)
        {
            BuildingPlacer.instance.SetBuildingPrefab(item);
        }

       // button.gameObject.SetActive(false);
    }

    private void ShuffleItems()
    {
        shuffledItems = new List<GameObject>(items);

        for (int i = shuffledItems.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (shuffledItems[i], shuffledItems[j]) = (shuffledItems[j], shuffledItems[i]);
        }
    }
}
