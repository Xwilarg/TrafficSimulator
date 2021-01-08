using UnityEngine;

namespace TrafficSimulator.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/BehaviorInfo", fileName = "BehaviorInfo")]
    public class BehaviorInfo : ScriptableObject
    {
        [Tooltip("% of change that a car will mark a stop sign")]
        [Range(0, 100)]
        public int ChanceRespectStop;
    }
}