using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    /// <summary>
    /// Responsible for loading and unloading map data streamers based on the player's position.
    /// </summary>
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
        
        private void Update()
        {
            foreach (var current in _mapData)
                UpdateMapDataStreamers(current);
        }

        /// <summary>
        /// Defines the map data area based on the start and end positions and the step.
        /// </summary>
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

        /// <summary>
        /// Checks if the player is in the area of the map data and loads/unloads the map data streamers accordingly.
        /// </summary>
        /// <param name="current"></param>
        private void UpdateMapDataStreamers(Vector3 current)
        {
            if (IsVector3InArea(transform.position, current, loadTolerance) && !_loadedMapData.Contains(current))
                LoadMapDataStreamer(current);

            if (!IsVector3InArea(transform.position, current, loadTolerance) && _loadedMapData.Contains(current))
                UnloadMapDataStreamer(current);
        }

        /// <summary>
        /// Loads map data streamer based on the position and adds it to the loaded map data list.
        /// </summary>
        /// <param name="current">Position to loading map</param>
        private void LoadMapDataStreamer(Vector3 current)
        {
            _loadedMapData.Add(current);
            _mapDataStreamers.Add(current, new MapDataStreamer($"MapChunks/MapX{current.x}Y{current.z}"));
        }

        /// <summary>
        /// Unloads map data streamer based on the position and adds it to the loaded map data list.
        /// </summary>
        /// <param name="current">Position to unloading map</param>
        private void UnloadMapDataStreamer(Vector3 current)
        {
            _mapDataStreamers[current].Destroy();
            _loadedMapData.Remove(current);
            _mapDataStreamers.Remove(current);
        }

        /// <summary>
        /// Checks if the vector3 is in the area of the reference vector3 with the range.
        /// </summary>
        /// <param name="reference">Reference point to check</param>
        /// <param name="point">Position to check distance between reference</param>
        /// <param name="range">Control parameter</param>
        /// <returns></returns>
        private bool IsVector3InArea(Vector3 reference, Vector3 point, float range)
        {
            return Vector3.Distance(reference, point) < range;
        }
    }
}
