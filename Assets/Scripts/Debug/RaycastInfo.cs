using System;
using UnityEngine;

namespace TrafficSimulator.Debug
{
    public struct RaycastInfo
    {
        public RaycastInfo(Vector3 origin, Vector3 destination, Color color)
        {
            Origin = origin;
            Destination = destination;
            Color = color;
            ExpireTime = DateTime.Now.AddMilliseconds(200);
        }

        public Vector3 Origin;
        public Vector3 Destination;
        public Color Color;
        public DateTime ExpireTime;
    }
}
