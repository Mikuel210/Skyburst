using UnityEngine;

public class WeaponController : MonoBehaviour {

    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletSpawn;
    [Space, SerializeField] private float _fireRate;
    [SerializeField] private float _shootingForce;
    [SerializeField] private float _recoil;

    private SimulatedRigidbody _playerRigidbody;
    private float _time;
    
    void Start() {
        _playerRigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<SimulatedRigidbody>();
    }
    
    void Update() {
        _time += Time.deltaTime;
        
        if (!Input.GetMouseButton(0)) return;
        if (_time < _fireRate) return;

        Shoot();
        _time = 0;
    }

    private void Shoot() {
        // Spawn bullet
        var bullet = Instantiate(_bulletPrefab, _bulletSpawn.position, _bulletSpawn.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * _shootingForce, ForceMode.Impulse);
        
        Destroy(bullet, 5f);
        
        // Recoil
        _playerRigidbody.AddForce(Camera.main.transform.forward * -_recoil);
    }
}
