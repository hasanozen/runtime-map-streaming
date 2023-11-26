using UnityEditor;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private Transform parent; // The parent of the instantiated objects
    [SerializeField] private GameObject prefab; // The prefab you want to instantiate
    [SerializeField] private int gridSizeX = 5; // Number of grid cells in the X direction
    [SerializeField] private int gridSizeY = 5; // Number of grid cells in the Y direction
    [SerializeField] private float spacing = 2f; // Spacing between grid cells
    [SerializeField] private Vector3 scale = Vector3.one; // Scale of the instantiated objects

    [ContextMenu("Generate")]
    private void Generate()
    {
        InstantiateGrid();
    }

    private void InstantiateGrid()
    {
        // Calculate the starting position to center the grid
        Vector3 startPosition = new Vector3(-gridSizeX / 2f * spacing, 0f, -gridSizeY / 2f * spacing);

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                // Calculate the position based on the grid, spacing, and starting position
                Vector3 position = startPosition + new Vector3(x * spacing, 0f, y * spacing);

                // Instantiate the prefab at the calculated position
                var current = PrefabUtility.InstantiatePrefab(prefab, parent) as GameObject;
                current.transform.position = position;
                current.transform.rotation = Quaternion.identity;
                current.transform.localScale = scale;
                current.transform.SetParent(parent);
            }
        }
    }
    
}
