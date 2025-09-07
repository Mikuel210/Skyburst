using UnityEngine;

public class LavaController : MonoBehaviour {

    [SerializeField] private float _damage;

    void OnTriggerStay(Collider collider) {
        if (!collider.gameObject.TryGetComponent<Health>(out var health)) return;
        health.TakeDamage(_damage * Time.fixedDeltaTime);
    }
    
    void OnCollisionStay(Collision collision) {
        if (!collision.gameObject.TryGetComponent<Health>(out var health)) return;
        health.TakeDamage(_damage * Time.fixedDeltaTime);
    }
}
