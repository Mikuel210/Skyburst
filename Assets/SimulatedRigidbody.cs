using System;
using UnityEngine;

public class SimulatedRigidbody : MonoBehaviour {

    [SerializeField, Range(0, 0.1f)] private float _damping;
    private Vector3 _velocity;

    private void FixedUpdate() {
        _velocity *= 1 - _damping;
        transform.Translate(_velocity, Space.World);
    }

    public void AddForce(Vector3 force) => _velocity += force;

}
