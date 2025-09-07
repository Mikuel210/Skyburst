using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FollowCamera : MonoBehaviour {

    [SerializeField] private float _movementSmoothing;
    [SerializeField] private float _rotationSmoothing;
    
    private Transform _mainCamera;
    private Vector3 _shake;
    
    void Start() {
        _mainCamera = Camera.main!.transform;
    }
    
    void Update() {
        transform.position = Vector3.Lerp(transform.position, _mainCamera.position, _movementSmoothing * Time.deltaTime) + _shake;
        transform.rotation = Quaternion.Lerp(transform.rotation, _mainCamera.rotation, _rotationSmoothing * Time.deltaTime);
    }

    public void StartShake(float duration, float amplitude) {
        StartCoroutine(Shake(duration, amplitude));
    }

    IEnumerator Shake(float duration, float amplitude) {
        float elapsed = 0;

        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            _shake = Random.insideUnitSphere * (amplitude * ((duration - elapsed) / duration));
            yield return null;
        }

        _shake = new();
    }
}
