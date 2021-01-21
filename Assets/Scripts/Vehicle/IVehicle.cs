using UnityEngine;

namespace TrafficSimulator.Vehicle
{
    public interface IVehicle
    {
        /// <summary>
        /// Set the speed the car need to go to in a scale from 0 to 100
        /// </summary>
        void SetObjectiveSpeed(float speed);

        /// <summary>
        /// Set the rotation the car need to look to, from 0 to 360°
        /// </summary>
        void SetObjectiveRotation(Quaternion rotation);

        /// <summary>
        /// Once called, the car is not supposed to be able to do anything and keep a constant speed
        /// </summary>
        void Break();

        /// <summary>
        /// Get the current speed the car goes by (Rigidbody magnitude)
        /// </summary>
        float GetCurrentSpeed();
    }
}
