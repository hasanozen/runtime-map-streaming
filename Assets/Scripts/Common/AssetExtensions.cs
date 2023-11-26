using UnityEditor;
using UnityEngine;

namespace Common
{
    public static class AssetExtensions
    {
        public static string GetPrefabAssetPath(this GameObject source) => source.GetAssetPath();
        public static string GetPrefabAssetPath(this Transform source) => source.gameObject.GetAssetPath();
        
        private static string GetAssetPath(this GameObject source) => PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(source.gameObject);
    }
}