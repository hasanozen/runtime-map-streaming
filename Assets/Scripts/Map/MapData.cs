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
        
        /// <summary>
        /// Adds a map object to the map objects list.
        /// </summary>
        /// <param name="mapObject">Item to add</param>
        public void AddObject(MapObject mapObject) => mapObjects.Add(mapObject);
        
        /// <summary>
        /// Instantiates a map object and configures it's transform values.
        /// </summary>
        /// <param name="mapObject">Object data to instantiate</param>
        /// <returns>Instantiated GameObject</returns>
        public GameObject InstantiateObject(MapObject mapObject)
        {
            var prefab = (GameObject)AssetDatabase.LoadAssetAtPath(mapObject.Path, typeof(GameObject));
            
            if (prefab == null) return null;
            
            var instantiatedObject = Instantiate(prefab);
            SetObjectTransform(instantiatedObject, mapObject);
            
            return instantiatedObject;
        }

        /// <summary>
        /// Configures the transform values of a GameObject based on the MapObject data.
        /// </summary>
        /// <param name="obj">GameObject to configure</param>
        /// <param name="mapObject"><see cref="MapObject"/> data that contains information</param>
        private static void SetObjectTransform(GameObject obj, MapObject mapObject)
        {
            obj.transform.position = mapObject.Position;
            obj.transform.localScale = mapObject.Scale;
            obj.transform.rotation = mapObject.Rotation;
            obj.name = mapObject.Name;
        }

        /// <summary>
        /// Records all the child objects in the parent map to the map data.
        /// </summary>
        /// <param name="mapData"><see cref="MapData"/> to modify</param>
        /// <param name="parentMap">Parent GameObject that contains map items</param>
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