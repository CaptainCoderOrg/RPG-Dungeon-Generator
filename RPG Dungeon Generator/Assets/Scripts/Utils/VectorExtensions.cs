using UnityEngine;

namespace CaptainCoder.Extensions
{

    public static class VectorExtensions
    {
        /// <summary>
        /// Applies the ceiling function to each of the Vector2 coordinates
        /// </summary>
        public static Vector2Int Ceil(this Vector2 toCeil) => new (Mathf.CeilToInt(toCeil.x), Mathf.CeilToInt(toCeil.y));
    }

}