using UnityEditor;
using UnityEngine;

public class AssetPathGetter : MonoBehaviour
{
    [ContextMenu("GetPrefabAssetPath")]
    private void GetPrefabAssetPath()
    {
        GameObject selectedObject = gameObject; // Replace with your GameObject reference

        string prefabAssetPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(selectedObject);

        if (!string.IsNullOrEmpty(prefabAssetPath))
        {
            Debug.Log("Prefab Asset Path: " + prefabAssetPath);
        }
        else
        {
            Debug.Log("Selected object is not a prefab instance.");
        }
    }
}