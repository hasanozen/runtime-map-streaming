using System.Collections.Generic;
using Map;
using UnityEditor;
using UnityEngine;

namespace Editor.Map
{
    public class MapChunk : EditorWindow
    {
        private float _xStart, _yStart, _step, _xEnd, _yEnd;
        private GameObject _environment;

        [MenuItem("Tools/Map/Map Chunk")]
        public static void ShowWindow()
        {
            GetWindow<MapChunk>("Map Chunk");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Bottom Left Corner Position: ");
            EditorGUILayout.Space(2);

            _xStart = EditorGUILayout.FloatField("X Start: ", _xStart);
            _yStart = EditorGUILayout.FloatField("Y Start: ", _yStart);

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Top Right Corner Position: ");
            EditorGUILayout.Space(2);

            _xEnd = EditorGUILayout.FloatField("X End: ", _xEnd);
            _yEnd = EditorGUILayout.FloatField("Y End: ", _yEnd);

            EditorGUILayout.Space(10);

            _step = EditorGUILayout.FloatField("Step: ", _step);

            EditorGUILayout.Space(10);

            _environment = (GameObject) EditorGUILayout.ObjectField(_environment, typeof(GameObject), true);

            if (GUILayout.Button("Chunk Map"))
            {
                PlaceObjectsIntoChunks();
            }

            if (GUILayout.Button("Generate Map Data"))
            {
                GenerateMapData();
            }
        }

        private void PlaceObjectsIntoChunks()
        {
            for (var x = _xStart; x <= _xEnd; x += _step)
            {
                for (var y = _yStart; y <= _yEnd; y += _step)
                {
                    List<Transform> objectsToChangeParent = new List<Transform>();

                    foreach (Transform t in _environment.transform)
                    {
                        if (t.gameObject != _environment)
                        {
                            if (IsVector3InArea(t.position, new Vector3(x - _step / 2f, 0, y - _step / 2f),
                                    new Vector3(x + _step / 2f, 0, y + _step / 2f)))
                            {
                                objectsToChangeParent.Add(t);
                            }
                        }
                    }

                    if (objectsToChangeParent.Count > 0)
                    {
                        var parent = new GameObject($"MapX{x}Y{y}")
                        {
                            transform =
                            {
                                position = new Vector3(x, 0, y)
                            }
                        };
                        
                        foreach (var current in objectsToChangeParent)
                        {
                            current.SetParent(parent.transform);
                        }
                    }
                }
            }
        }

        private bool IsVector3InArea(Vector3 reference, Vector3 lower, Vector3 upper)
        {
            return reference.x >= lower.x &&
                   reference.x <= upper.x &&
                   reference.z >= lower.z &&
                   reference.z <= upper.z;
        }

        private void GenerateMapData()
        {
            foreach (var current in Selection.gameObjects)
            {
                var mapData = CreateInstance<MapData>();

                var fileName = current.name;
                MapData.RecordObjects(mapData, current);

                AssetDatabase.CreateAsset(mapData, $"Assets/Map/MapChunks/{fileName}.asset");
                AssetDatabase.SaveAssets();
            }

            AssetDatabase.Refresh();
        }
    }
}