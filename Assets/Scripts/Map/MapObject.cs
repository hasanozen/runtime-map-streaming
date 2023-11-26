using System;
using Common;
using UnityEngine;

namespace Map
{
    [Serializable]
    public struct MapObject
    {
        [SerializeField] private string name;
        [SerializeField] private Vector3 position;
        [SerializeField] private Vector3 scale;
        [SerializeField] private Quaternion rotation;
        [SerializeField] private string path;

        public string Name => name;
        public Vector3 Position => position;
        public Vector3 Scale => scale;
        public Quaternion Rotation => rotation;
        public string Path => path;

        public MapObject(Transform @ref)
        {
            name = @ref.name;
            position = @ref.position;
            scale = @ref.localScale;
            rotation = @ref.rotation;
            path = @ref.GetPrefabAssetPath();
        }
    }
}