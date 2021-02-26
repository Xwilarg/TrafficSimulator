using UnityEngine;

namespace TrafficSimulator.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/VehicleInfo", fileName = "VehicleInfo")]
    public class VehicleInfo : ScriptableObject
    {
        [Tooltip("Base speed of the car")]
        public float Speed;

        [Tooltip("Rotation speed of the car")]
        public float Torque;

        [Tooltip("Information check")]
        public Vision[] RangeCheck;

        public AnimationCurve SpeedCurve;
    }
}