using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class Vehicle : MonoBehaviour
{
    private Node _objective;
    private Rigidbody _rb;

    private const float _speed = 10f;
    private const float _torque = 1f;

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

        _rb.velocity = transform.forward * _speed;
        // transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.FromToRotation(transform.position, _objective.transform.position), _torque);
    }
}
