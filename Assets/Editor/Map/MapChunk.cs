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
        public static void ShowWindow() => GetWindow<MapChunk>("Map Chunk");

        private void OnGUI()
        {
            DrawPositionFields("Bottom Left Corner Position: ", ref _xStart, ref _yStart);
            DrawPositionFields("Top Right Corner Position: ", ref _xEnd, ref _yEnd);

            _step = EditorGUILayout.FloatField("Step: ", _step);
            _environment = (GameObject)EditorGUILayout.ObjectField(_environment, typeof(GameObject), true);

            if (GUILayout.Button("Chunk Map"))
                PlaceObjectsIntoChunks();

            if (GUILayout.Button("Generate Map Data"))
                GenerateMapData();
        }

        private void DrawPositionFields(string label, ref float x, ref float y)
        {
            EditorGUILayout.LabelField(label);
            EditorGUILayout.Space(2);
            x = EditorGUILayout.FloatField("X Start: ", x);
            y = EditorGUILayout.FloatField("Y Start: ", y);
            EditorGUILayout.Space(10);
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
                        if (t.gameObject != _environment && IsVector3InArea(t.position, new Vector3(x - _step / 2f, 0, y - _step / 2f),
                            new Vector3(x + _step / 2f, 0, y + _step / 2f)))
                            objectsToChangeParent.Add(t);
                    }

                    if (objectsToChangeParent.Count > 0)
                        CreateParentAndSetTransform(x, y, objectsToChangeParent);
                }
            }
        }

        private void CreateParentAndSetTransform(float x, float y, List<Transform> objectsToChangeParent)
        {
            var parent = new GameObject($"MapX{x}Y{y}")
            {
                transform =
                {
                    position = new Vector3(x, 0, y)
                }
            };

            foreach (var current in objectsToChangeParent)
                current.SetParent(parent.transform);
        }

        private bool IsVector3InArea(Vector3 reference, Vector3 lower, Vector3 upper) =>
            reference.x >= lower.x &&
            reference.x <= upper.x &&
            reference.z >= lower.z &&
            reference.z <= upper.z;

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
