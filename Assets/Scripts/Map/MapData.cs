using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Map
{
    [CreateAssetMenu(fileName = "New Map Data", menuName = "Map/Map Data"), Serializable]
    public class MapData : ScriptableObject
    {
        [SerializeField] private List<MapObject> mapObjects;
        
        public List<MapObject> MapObjects => mapObjects;

        private void Awake()
        {
            mapObjects ??= new List<MapObject>();
        }
        
        public void AddObject(MapObject mapObject)
        {
            mapObjects.Add(mapObject);
        }
        
        public GameObject InstantiateObject(MapObject mapObject)
        {
            var temp = (GameObject) AssetDatabase.LoadAssetAtPath(mapObject.Path, typeof(GameObject));
            
            if (temp == null) return null;
            
            temp = Instantiate(temp);
            temp.transform.position = mapObject.Position;
            temp.transform.localScale = mapObject.Scale;
            temp.transform.rotation = mapObject.Rotation;
            temp.name = mapObject.Name;
            
            return temp;
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