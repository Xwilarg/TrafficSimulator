using System;

namespace TrafficSimulator.SO
{
    [Serializable]
    public class Vision
    {
        public float AngleBase, AngleStep;
        public float OffsetBase, OffsetStep;
        public int NbIteration;
        public float Size;
        public VisionType Type;
    }
}
