using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class Vehicle : MonoBehaviour
{
    private Node _objective;
    private Rigidbody _rb;

    private const float _speed = 15f;
    private const float _acceleration = .05f;
    private const float _torque = 0.1f;
    private const float _vision = 10f;
    private const float _sideVision = 8f;
    private float _lastSpeed = 0f; // Last speed the car went by

    private readonly float[] _rangeCheck = new[] { -.3f, 0f, .3f };
    private readonly float[] _sideRangeCheck = new[] { -.9f, -.6f, .6f, .9f };

    private TextMesh _infoText;

    private VehicleBehavior _currBehavior = VehicleBehavior.NONE;
    private float _ignoreNextStopTimer = 0f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _infoText = GetComponentInChildren<TextMesh>();

        _objective = GameObject.FindGameObjectsWithTag("Node").OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).FirstOrDefault()?.GetComponent<Node>();
        Assert.IsNotNull(_objective, "No node found");
    }

    private void FixedUpdate()
    {
        if (_objective == null) // No objective
            return;

        var objVelocity = transform.forward * _speed; 
        var rot = Quaternion.LookRotation(_objective.transform.position - transform.position);
        rot = Quaternion.Slerp(transform.rotation, rot, _torque);
        _rb.MoveRotation(rot);

        float? mult = null; // Speed multiplicator to slow down the car

        var objDistance = Vector3.Distance(_objective.transform.position, transform.position);
        if (_currBehavior == VehicleBehavior.STOP && objDistance < _vision)
        {
            mult = CalculateSpeedFromObstacle(objDistance, _vision);
            if (_lastSpeed < .2f)
            {
                _currBehavior = VehicleBehavior.NONE;
                mult = null;
            }
        }
        else if (objDistance < 1f) // Setting next objective is we reached the current one
        {
            _objective = _objective.NextNodes[Random.Range(0, _objective.NextNodes.Length)];
        }

        if (mult == null)
        {
            foreach (var r in _rangeCheck) // Object detection
            {
                var res = DetectObstacle(r, _vision);
                if (res != null)
                {
                    mult = res;
                    break;
                }
            }
        }
        if (mult == null)
        {
            foreach (var r in _sideRangeCheck) // Object detection
            {
                var res = DetectObstacle(r, _sideVision);
                if (res != null)
                {
                    mult = res;
                    break;
                }
            }
        }
        if (mult != null)
            objVelocity *= mult.Value;
        var currSpeed = Mathf.Lerp(_rb.velocity.magnitude, objVelocity.magnitude, _acceleration);
        _rb.velocity = objVelocity.normalized * currSpeed;
        _infoText.text = "Current Speed: " + currSpeed + "\nBehavior: " + _currBehavior.ToString();
        _lastSpeed = currSpeed;
    }

    private void Update()
    {
        if (_ignoreNextStopTimer > 0f)
            _ignoreNextStopTimer -= Time.deltaTime;
    }

    private float? DetectObstacle(float dirOffset, float visionDist)
    {
        if (Physics.Raycast(transform.position + transform.forward * 1.1f, transform.forward * 3f + transform.right * dirOffset, out RaycastHit hit, visionDist))
        {
            if (hit.collider.CompareTag("Sign") && _ignoreNextStopTimer <= 0f) // The vehicle see a sign
            {
                switch (hit.collider.GetComponent<Sign>().SignType)
                {
                    case SignType.STOP:
                        _currBehavior = VehicleBehavior.STOP;
                        _ignoreNextStopTimer = 2f;
                        break;
                }
            }
            else // Various other obstacle
                return CalculateSpeedFromObstacle(hit.distance, visionDist);
        }
        return null;
    }

    private float CalculateSpeedFromObstacle(float hitDistance, float visionDistance)
    {
        var val = hitDistance * _speed / visionDistance; // Slow down depending of obstacle distance
        var mult = val / _speed;
        return mult;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var r in _rangeCheck)
        {
            Gizmos.DrawLine(transform.position + transform.forward * 1.1f, transform.position + (transform.forward * 3f + transform.right * r).normalized * _vision);
        }
        Gizmos.color = Color.blue;
        foreach (var r in _sideRangeCheck)
        {
            Gizmos.DrawLine(transform.position + transform.forward * 1.1f, transform.position + (transform.forward * 3f + transform.right * r).normalized * _sideVision);
        }
    }
}
