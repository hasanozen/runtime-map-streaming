using Map;
using UnityEditor;
using UnityEngine;

namespace Editor.Map
{
    [CustomEditor(typeof(MapData))]
    public class MapDataCustomInspector : UnityEditor.Editor
    {
        [SerializeField] private GameObject parentMap;
        
        private MapData _mapData;

        private void OnEnable()
        {
            _mapData = (MapData) target;
        }

        public override void OnInspectorGUI()
        {
            parentMap = (GameObject) EditorGUILayout.ObjectField(parentMap, typeof(GameObject), true);
            
            EditorUtility.SetDirty(target);
            EditorGUILayout.LabelField($"Number of objects: {_mapData.MapObjects.Count}");

            if (GUILayout.Button("Clear"))
            {
                _mapData.MapObjects.Clear();
            }

            if (GUILayout.Button("Record Map Objects"))
            {
                var objects = parentMap.GetComponentsInChildren<Transform>();
                _mapData.MapObjects.Clear();
                
                foreach (var current in objects)
                {
                    if (current.name == parentMap.name) continue;
                    _mapData.AddObject(new MapObject(current));
                }
                
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("Instantiate Map Objects"))
            {
                foreach (var current in _mapData.MapObjects)
                {
                    _mapData.InstantiateObject(current);
                }
            }
        }
    }
}