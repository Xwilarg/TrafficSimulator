using System;
using UnityEngine;

namespace Debug
{
    public struct HitInfo
    {
        public HitInfo(Vector3 position, Color color)
        {
            Position = position;
            Color = color;
            ExpireTime = DateTime.Now.AddMilliseconds(500);
        }

        public Vector3 Position;
        public Color Color;
        public DateTime ExpireTime;
    }
}
