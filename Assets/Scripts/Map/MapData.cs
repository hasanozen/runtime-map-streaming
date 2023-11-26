using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Map
{
    [CreateAssetMenu(fileName = "New Map Data", menuName = "Map/Map Data"), Serializable]
    public class MapData : ScriptableObject
    {
        [SerializeField] private List<MapObject> mapObjects = new List<MapObject>();
        
        public List<MapObject> MapObjects => mapObjects;

        private void Awake()
        {
            mapObjects ??= new List<MapObject>();
        }
        
        public void AddObject(MapObject mapObject) => mapObjects.Add(mapObject);
        
        public GameObject InstantiateObject(MapObject mapObject)
        {
            var prefab = (GameObject)AssetDatabase.LoadAssetAtPath(mapObject.Path, typeof(GameObject));
            
            if (prefab == null) return null;
            
            var instantiatedObject = Instantiate(prefab);
            SetObjectTransform(instantiatedObject, mapObject);
            
            return instantiatedObject;
        }

        private static void SetObjectTransform(GameObject obj, MapObject mapObject)
        {
            obj.transform.position = mapObject.Position;
            obj.transform.localScale = mapObject.Scale;
            obj.transform.rotation = mapObject.Rotation;
            obj.name = mapObject.Name;
        }

        public static void RecordObjects(MapData mapData, GameObject parentMap)
        {
            mapData.MapObjects.Clear();
            
            foreach (Transform current in parentMap.transform)
            {
                if (current.name == parentMap.name) continue;
                mapData.AddObject(new MapObject(current));
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}