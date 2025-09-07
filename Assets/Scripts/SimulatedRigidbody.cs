using System;
using StarterAssets;
using UnityEngine;
using UnityEngine.Serialization;

public class SimulatedRigidbody : MonoBehaviour {

    [SerializeField, Range(0, 0.1f)] private float _damping;
    private Vector3 _worldVelocity;
    private Transform _player;
    public Vector3 Velocity => _worldVelocity;

    private ThirdPersonController _controller;

    void Start() {
        _player = transform.Find("PlayerArmature");
        _controller = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonController>();
    }

    private void FixedUpdate() {
        _worldVelocity *= 1 - _damping;
    }

    private void LateUpdate() {
        if (_controller.Grounded && (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0))
            _worldVelocity *= 1 - Time.deltaTime * 5;
    }

    public void AddForce(Vector3 force) => _worldVelocity += force;

}
