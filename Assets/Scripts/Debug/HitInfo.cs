using UnityEngine;

namespace Debug
{
    public struct HitInfo
    {
        public HitInfo(Vector3 position, Color color)
        {
            Position = position;
            Color = color;
        }

        public Vector3 Position;
        public Color Color;
    }
}
