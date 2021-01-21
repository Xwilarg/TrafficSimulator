using UnityEngine;

namespace TrafficSimulator.Vehicle.Sample
{
    public class SampleVehicle : MonoBehaviour, IVehicle
    {
        private Rigidbody _rb; // Rigidbody of the car

        private float _lastSpeed = 0f; // Last speed the car went by

        private bool _isBroken = false;

        private const float _acceleration = 0.05f;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (_isBroken)
            {
                _rb.velocity = _rb.velocity.normalized * _lastSpeed; // Car is broken, we go forward
                return;
            }
        }

        public void SetObjectiveRotation(Quaternion rotation)
        {
            if (!_isBroken)
            {
                var oldRot = _rb.rotation;
                _rb.MoveRotation(rotation);
                _rb.rotation = Quaternion.Euler(new Vector3(oldRot.x, _rb.rotation.eulerAngles.y, oldRot.z));
            }
        }

        public void SetObjectiveSpeed(float speed)
        {
            var currSpeed = Mathf.Lerp(_rb.velocity.magnitude, speed, _acceleration);
            _rb.velocity = transform.forward * currSpeed;
            _lastSpeed = currSpeed;
        }

        public void Break()
        {
            _isBroken = true;
        }

        public float GetCurrentSpeed()
        {
            return _rb.velocity.magnitude;
        }
    }
}
