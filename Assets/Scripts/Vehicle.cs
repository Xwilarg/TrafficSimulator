using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class Vehicle : MonoBehaviour
{
    private Node _objective;
    private Rigidbody _rb;

    private const float _speed = 20f;
    private const float _torque = 0.1f;

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
        var rot = Quaternion.LookRotation(_objective.transform.position - transform.position);
        rot = Quaternion.Slerp(transform.rotation, rot, _torque);
        _rb.MoveRotation(rot);

        if (Vector3.Distance(_objective.transform.position, transform.position) < 1f)
            _objective = _objective.NextNode;
    }
}
