using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicButtonManager : MonoBehaviour
{
    [Header("Button References (Max 4)")]
    public Button[] buttons; // Array of up to 4 buttons

    [Header("Available Items")]
    public List<GameObject> items; // List of items to assign to buttons

    private int currentIndex = 0; // Tracks the current index in the items list
    private List<GameObject> shuffledItems; // List of shuffled items

    public void StartPlacing()
    {
        if (buttons.Length > 4)
        {
            Debug.LogError("Maximum of 4 buttons are allowed.");
            return;
        }

        // Initialize the shuffled list of items
        ShuffleItems();

        // Initialize buttons
        UpdateButtons();
    }

    public void UpdateButtons()
    {
        // Loop through each button and assign a random item from shuffled list
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i + currentIndex < shuffledItems.Count)
            {
                GameObject item = shuffledItems[i + currentIndex];
                Button button = buttons[i];

                // Update button text with the correct name from the shuffled list
                Text buttonText = button.GetComponentInChildren<Text>();
                if (buttonText != null)
                {
                    buttonText.text = item.name; // Use the name of the item
                }

                // Assign the prefab to the button's click event
                button.onClick.RemoveAllListeners(); // Clear previous listeners
                button.onClick.AddListener(() => AssignToBuildingPlacer(item, button));
                button.gameObject.SetActive(true); // Ensure button is visible
            }
            else
            {
                // Hide buttons if there are no more items
                buttons[i].gameObject.SetActive(false);
            }
        }
    }

    public void NextPage()
    {
        if (currentIndex + buttons.Length < shuffledItems.Count)
        {
            currentIndex += buttons.Length;
            UpdateButtons();
        }
    }

    public void PreviousPage()
    {
        if (currentIndex - buttons.Length >= 0)
        {
            currentIndex -= buttons.Length;
            UpdateButtons();
        }
    }

    private void AssignToBuildingPlacer(GameObject item, Button button)
    {
        // Assign the object to the BuildingPlacer
        if (BuildingPlacer.instance != null)
        {
            BuildingPlacer.instance.SetBuildingPrefab(item);
        }

        // Deactivate the button after it has been pressed
        button.gameObject.SetActive(false);
    }

    private void ShuffleItems()
    {
        // Create a copy of the items list and shuffle it
        shuffledItems = new List<GameObject>(items);

        // Fisher-Yates shuffle algorithm
        for (int i = shuffledItems.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1); // Random index between 0 and i
            GameObject temp = shuffledItems[i];
            shuffledItems[i] = shuffledItems[j];
            shuffledItems[j] = temp;
        }
    }
}
