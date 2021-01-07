using UnityEngine;

namespace Debug
{
    public struct RaycastInfo
    {
        public RaycastInfo(Vector3 origin, Vector3 destination, Color color)
        {
            Origin = origin;
            Destination = destination;
            Color = color;
        }

        public Vector3 Origin;
        public Vector3 Destination;
        public Color Color;
    }
}
