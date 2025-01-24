using System;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public int gridWidth = 10;          // Width of the grid
    public int gridHeight = 10;         // Height of the grid
    public int gridDepth = 10;          // Depth of the grid
    public float cellSize = 1f;         // Size of each cell in the grid
    public Color gridColor = Color.green; // Default color for grid lines
    public Color occupiedColor = Color.red; // Color for cells with objects
    public GameObject prefabToSpawn;    // Prefab to spawn in the center of detected cells
    public string targetTag = "Target"; // Tag to check for objects

    private void Start()
    {
        SpawnPrefabInTouchingCells();
    }

    void OnDrawGizmos()
    {
        // Loop through all grid positions
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                for (int z = 0; z < gridDepth; z++)
                {
                    Vector3 cellCenter = GetCellCenter(x, y, z);

                    // Check if the cell is occupied by an object with the specified tag
                    if (IsCellTouchingObjectWithTag(cellCenter))
                    {
                        Gizmos.color = occupiedColor; // Mark as occupied
                    }
                    else
                    {
                        Gizmos.color = gridColor; // Default color
                    }

                    // Draw a wireframe cube for the grid cell
                    Gizmos.DrawWireCube(cellCenter, Vector3.one * cellSize);
                }
            }
        }
    }

    Vector3 GetCellCenter(int x, int y, int z)
    {
        // Calculate the center position of a grid cell based on its coordinates
        return new Vector3(x * cellSize, y * cellSize, z * cellSize);
    }

    bool IsCellTouchingObjectWithTag(Vector3 cellCenter)
    {
        // Check for any colliders overlapping the cell using Physics.OverlapBox
        Collider[] colliders = Physics.OverlapBox(cellCenter, Vector3.one * (cellSize / 2));

        // Check if any of the colliders have the specified tag
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(targetTag))
            {
                return true; // Object has the target tag
            }
        }

        return false; // No objects with the target tag
    }

    public void SpawnPrefabInTouchingCells()
    {
        // Loop through all grid positions
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                for (int z = 0; z < gridDepth; z++)
                {
                    Vector3 cellCenter = GetCellCenter(x, y, z);

                    // If the cell is touching an object with the specified tag, spawn a prefab
                    if (IsCellTouchingObjectWithTag(cellCenter))
                    {
                        SpawnPrefab(cellCenter);
                    }
                }
            }
        }
    }

    void SpawnPrefab(Vector3 position)
    {
        // Check if prefabToSpawn is set
        if (prefabToSpawn == null)
        {
            Debug.LogError("Prefab to spawn is not set!");
            return;
        }

        // Instantiate the prefab at the given position with no rotation
        Instantiate(prefabToSpawn, position, Quaternion.identity);
    }
}
