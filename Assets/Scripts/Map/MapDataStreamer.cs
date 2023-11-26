using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Map
{
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
                            };
                        }
                    }
                }
            };
        }

        private static bool AddressableResourceExist(string key)
        {
            return Addressables.ResourceLocators.Any(resourceLocator => resourceLocator.Locate(key, typeof(GameObject), out _));
        }
        
        public void Destroy()
        {
            foreach (var current in _loadedObjects.Where(current => current != _environment)) Addressables.ReleaseInstance(current);
            Object.Destroy(_environment);
        }
    }
}