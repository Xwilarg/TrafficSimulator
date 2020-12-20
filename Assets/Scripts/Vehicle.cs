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
    private const float _vision = 20f;

    private readonly float[] _rangeCheck = new[] { -.9f, -.6f, -.3f, 0f, .3f, .6f, .9f };

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

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

        if (Vector3.Distance(_objective.transform.position, transform.position) < 1f) // Setting next objective is we reached the current one
            _objective = _objective.NextNodes[Random.Range(0, _objective.NextNodes.Length)];

        foreach (var r in _rangeCheck) // Object detection
        {
            if (Physics.Raycast(transform.position + transform.forward * 1.1f, transform.forward * 3f + transform.right * r, out RaycastHit hit, 10f))
            {
                var val = hit.distance * _speed / _vision; // Slow down depending of obstacle distance
                var mult = (val - 1f) / _speed;
                if (mult < 0f) mult = 0f;
                objVelocity *= mult;
                break;
            }
        }
        var currSpeed = Mathf.Lerp(_rb.velocity.magnitude, objVelocity.magnitude, _acceleration);
        _rb.velocity = objVelocity.normalized * currSpeed;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var r in _rangeCheck)
        {
            Gizmos.DrawLine(transform.position + transform.forward * 1.1f, transform.position + (transform.forward * 3f + transform.right * r).normalized * _vision);
        }
    }
}
