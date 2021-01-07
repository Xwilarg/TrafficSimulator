using UnityEngine;

namespace SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/VehicleInfo", fileName = "VehicleInfo")]
    public class VehicleInfo : ScriptableObject
    {
        [Tooltip("Base speed of the car")]
        public float Speed;

        [Tooltip("Acceleration of the car (speed in which it goes from 0 to _speed)")]
        public float Acceleration;

        [Tooltip("Rotation speed of the car")]
        public float Torque;

        [Tooltip("Distance in which the car see things ahead of it")]
        public float Vision;

        [Tooltip("Same as _vision but on the sides of the car")]
        public float SideVision;

        [Tooltip("Range check for information in front of the car")]
        public float[] RangeCheck;

        [Tooltip("Range check for information on the side of the car")]
        public float[] SideRangeCheck;
    }
}