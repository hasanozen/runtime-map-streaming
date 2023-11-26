using UnityEditor;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject prefab;
    [SerializeField] private int gridSizeX = 5;
    [SerializeField] private int gridSizeY = 5;
    [SerializeField] private float spacing = 2f;
    [SerializeField] private Vector3 scale = Vector3.one;

    [ContextMenu("Generate")]
    private void Generate()
    {
        InstantiateGrid();
    }

    /// <summary>
    /// This method created for the purpose of generating a grid of objects.
    /// Actually, this method not being used for a real project.
    /// It is being used for generating a grid of objects for the purpose of handling the study quickly.
    /// In a nutshell, it is creating grid for placing specified prefab in the grid cells.
    /// </summary>
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
