using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Map
{
    /// <summary>
    /// Streams the map data dynamically.
    /// </summary>
    public class MapDataStreamer
    {
        private readonly string _mapDataPath;
        private readonly GameObject _environment;
        private readonly List<GameObject> _loadedObjects;
        
        public MapDataStreamer(string mapDataPath)
        {
            _mapDataPath = mapDataPath;
            _environment = new GameObject("Environment");
            _loadedObjects = new List<GameObject>();

            InstantiateAsync();
        }

        /// <summary>
        /// Loads the map data based on the path and instantiates the map objects.
        /// </summary>
        private void InstantiateAsync()
        {
            Addressables.LoadAssetAsync<MapData>(_mapDataPath).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    foreach (var current in handle.Result.MapObjects)
                    {
                        if (AddressableResourceExist(current.Path))
                        {
                            Addressables.InstantiateAsync(current.Path).Completed += inst =>
                            {
                                if (inst.Status == AsyncOperationStatus.Succeeded)
                                    ConfigureItem(inst, current);
                            };
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Configures instantiated map object's transform values.
        /// </summary>
        /// <param name="inst">Located asset handle</param>
        /// <param name="current"><see cref="MapObject"/> data</param>
        private void ConfigureItem(AsyncOperationHandle<GameObject> inst, MapObject current)
        {
            inst.Result.transform.position = current.Position;
            inst.Result.transform.localScale = current.Scale;
            inst.Result.transform.rotation = current.Rotation;
            inst.Result.name = current.Name;
            inst.Result.isStatic = true;

            if (_environment != null)
            {
                inst.Result.transform.SetParent(_environment.transform);
                _loadedObjects.Add(inst.Result);
            }
            else
            {
                Addressables.ReleaseInstance(inst.Result);
            }
        }

        /// <summary>
        /// Checks if the addressable resource exists with given key.
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if exist, otherwise false</returns>
        private static bool AddressableResourceExist(string key)
        {
            return Addressables.ResourceLocators.Any(resourceLocator => resourceLocator.Locate(key, typeof(GameObject), out _));
        }
        
        /// <summary>
        /// Destroys the environment and releases the loaded objects.
        /// </summary>
        public void Destroy()
        {
            foreach (var current in _loadedObjects.Where(current => current != _environment)) Addressables.ReleaseInstance(current);
            Object.Destroy(_environment);
        }
    }
}