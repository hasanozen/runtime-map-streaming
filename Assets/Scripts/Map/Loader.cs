using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class Loader : MonoBehaviour
    {
        [SerializeField] private float xStart, yStart, xEnd, yEnd, step;
        [SerializeField] private float loadTolerance;

        private List<Vector3> _mapData;
        private List<Vector3> _loadedMapData;
        private Dictionary<Vector3, MapDataStreamer> _mapDataStreamers = new Dictionary<Vector3, MapDataStreamer>();

        private void Start()
        {
            InitializeMapData();
        }

        private void InitializeMapData()
        {
            _mapData = new List<Vector3>();
            _loadedMapData = new List<Vector3>();

            for (var x = xStart; x <= xEnd; x += step)
            {
                for (var y = yStart; y <= yEnd; y += step)
                {
                    _mapData.Add(new Vector3(x, 0, y));
                }
            }
        }

        private void Update()
        {
            foreach (var current in _mapData)
            {
                UpdateMapDataStreamers(current);
            }
        }

        private void UpdateMapDataStreamers(Vector3 current)
        {
            if (IsVector3InArea(transform.position, current, loadTolerance) && !_loadedMapData.Contains(current))
                LoadMapDataStreamer(current);

            if (!IsVector3InArea(transform.position, current, loadTolerance) && _loadedMapData.Contains(current))
                UnloadMapDataStreamer(current);
        }

        private void LoadMapDataStreamer(Vector3 current)
        {
            _loadedMapData.Add(current);
            _mapDataStreamers.Add(current, new MapDataStreamer($"MapChunks/MapX{current.x}Y{current.z}"));
        }

        private void UnloadMapDataStreamer(Vector3 current)
        {
            _mapDataStreamers[current].Destroy();
            _loadedMapData.Remove(current);
            _mapDataStreamers.Remove(current);
        }

        private bool IsVector3InArea(Vector3 reference, Vector3 point, float range)
        {
            return Vector3.Distance(reference, point) < range;
        }
    }
}
