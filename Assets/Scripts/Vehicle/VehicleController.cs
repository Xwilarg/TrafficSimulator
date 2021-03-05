using System.Linq;
using TrafficSimulator.Debug;
using UnityEngine;
using UnityEngine.Assertions;

namespace TrafficSimulator.Vehicle
{
    [RequireComponent(typeof(Rigidbody))]
    public class VehicleController : MonoBehaviour
    {
        private Node _objective; // Where the car need to go

        [SerializeField]
        private SO.VehicleInfo _info; // Information about the vehicle behavior

        [SerializeField]
        private SO.BehaviorInfo _behavior; // Define the driver behavior and how often he will apply rules

        private string _infoText = ""; // Debug information about the car

        private VehicleBehavior _currBehavior = VehicleBehavior.NONE;
        private float _ignoreNextStopTimer = 0f; // TMP

        private int _raycastId = 0; // Used to differenciate raycasts called from FixedUpdate

        // Command interface for this vehicle
        private IVehicle _vehicle;

        private void Start()
        {
            _vehicle = GetComponent<IVehicle>();
            Assert.IsNotNull(_vehicle, "No IVehicle implementation found");

            // Get closest objective node
            _objective = GameObject.FindGameObjectsWithTag("Node").OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).FirstOrDefault()?.GetComponent<Node>();
            Assert.IsNotNull(_objective, "No node found");
        }

        private void FixedUpdate()
        {
            if (_objective == null) // No objective
                return;

            _raycastId = 0;

            // Base velocity
            var objVelocity = _info.Speed;

            // Rotate towards objective
            var rot = Quaternion.LookRotation(_objective.transform.position - transform.position);
            rot = Quaternion.Slerp(transform.rotation, rot, _info.Torque);
            _vehicle.SetObjectiveRotation(rot);

            float? mult = null; // Speed multiplicator to slow down the car
            SO.VisionType? type = null;

            #region OBSTACLE_DETECTION

            // We check for obstacles, if we detect something we lower the vehicle speed by having a multiplicator less than 1
            var objDistance = Vector3.Distance(_objective.transform.position, transform.position);
            if (_currBehavior == VehicleBehavior.STOP && objDistance < 10f)
            {
                mult = CalculateSpeedFromObstacle(objDistance, 10f);
                if (_vehicle.GetCurrentSpeed() < .2f)
                {
                    _currBehavior = VehicleBehavior.NONE;
                    mult = null;
                }
            }
            else if (objDistance < 1f) // Setting next objective is we reached the current one
            {
                _objective = _objective.NextNodes[Random.Range(0, _objective.NextNodes.Length)];
            }

            RaycastHit? closestObstable = null;
            if (mult == null)
            {
                foreach (var r in _info.RangeCheck) // Object detection in front
                {
                    float angle = r.AngleBase;
                    float offset = r.OffsetBase;
                    for (int i = 0; i < r.NbIteration; i++)
                    {
                        var res = DetectObstacle(offset, angle, r.Size, r.Type == SO.VisionType.FRONT ? Color.red : Color.blue, out closestObstable);
                        if (res != null)
                        {
                            mult = res;
                            type = r.Type;
                            break;
                        }
                        angle += r.AngleStep;
                        offset += r.OffsetStep;
                    }
                }
            }

            #endregion OBSTACLE_DETECTION

            AnimationCurve curve = null;
            if (type != null)
            {
                if (type == SO.VisionType.FRONT) curve = _info.FrontSpeedCurve;
                else if (type == SO.VisionType.SIDE) curve = _info.SideSpeedCurve;
            }

            if (mult != null)
                objVelocity *= curve == null ? mult.Value : curve.Evaluate(mult.Value);

            _vehicle.SetObjectiveSpeed(objVelocity);
            _infoText = "Current Speed: " + _vehicle.GetCurrentSpeed().ToString("0.00");
            _infoText += "\nObjective Speed: " + objVelocity;
            _infoText += "\nMultiplicator value: " + (mult == null ? "1" : (mult.Value.ToString("0.00") + ": " + curve.Evaluate(mult.Value).ToString("0.00")));
            _infoText += "\nDetection type: " + type;
            _infoText += "\nBehavior: " + _currBehavior.ToString();
            _infoText += "\nClosest obstacle: "
                + (closestObstable == null ? "None" : closestObstable.Value.collider.name + " (" + closestObstable.Value.distance.ToString("0.00") + ")");
        }

        private void Update()
        {
            if (_ignoreNextStopTimer > 0f)
                _ignoreNextStopTimer -= Time.deltaTime;
        }

        private float? DetectObstacle(float offsetPos, float dirOffset, float visionDist, Color color, out RaycastHit? hitInfo)
        {
            _raycastId++;
            if (DebugManager.S.RaycastWithDebug(GetInstanceID() + "" + _raycastId, (transform.position + transform.forward * 1.1f) + (transform.right * offsetPos), transform.forward * 3f + transform.right * dirOffset, visionDist, color, out RaycastHit hit))
            {
                hitInfo = hit;
                if (hit.collider.CompareTag("Sign") && _ignoreNextStopTimer <= 0f) // The vehicle see a sign
                {
                    switch (hit.collider.GetComponent<Sign>().SignType)
                    {
                        case SignType.STOP:
                            if (Random.Range(0, 101) < _behavior.ChanceRespectStop)
                                _currBehavior = VehicleBehavior.STOP;
                            _ignoreNextStopTimer = 2f;
                            break;
                    }
                }
                else // Various other obstacle
                    return CalculateSpeedFromObstacle(hit.distance, visionDist);
            }
            hitInfo = null;
            return null;
        }

        private float CalculateSpeedFromObstacle(float hitDistance, float visionDistance)
        {
            var val = hitDistance * _info.Speed / visionDistance; // Slow down depending of obstacle distance
            var mult = val / _info.Speed;
            return mult;
        }

        public string GetDebugInformation()
            => _infoText;

        public void Break()
            => _vehicle.Break();
    }
}