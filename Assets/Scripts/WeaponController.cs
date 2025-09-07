using UnityEngine;
using UnityEngine.Serialization;

public class WeaponController : MonoBehaviour {

    public bool isPlayer;
    public AudioClip audio;
    
    [Space] public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public bool multiple;
    
    [Space] public float fireRate;
    public float shootingForce;
    public float recoil;

    private SimulatedRigidbody _playerRigidbody;
    private FollowCamera _followCamera;
    public float time;
    
    void Start() {
        _playerRigidbody = GameObject.Find("PlayerParent").GetComponent<SimulatedRigidbody>();    
        _followCamera = GameObject.Find("RenderingCamera").GetComponent<FollowCamera>();
    }
    
    void Update() {
        if (!isPlayer) return;
        
        if (!Input.GetMouseButton(0)) return;
        if (time < fireRate) return;

        Shoot();
        time = 0;
    }

    public void Shoot() {
        // Spawn bullet
        if (!multiple) {
            var bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * shootingForce, ForceMode.Impulse);
        
            Destroy(bullet, 5f);   
        }
        else {
            for (int i = 0; i < 3; i++) {
                var euler = bulletSpawn.eulerAngles;
                var dispersion = 3;

                var rotation = Quaternion.Euler(new(euler.x + Random.Range(-dispersion, dispersion),
                    euler.y + Random.Range(-dispersion, dispersion),
                    euler.z + Random.Range(-dispersion, dispersion)));
                
                var bullet = Instantiate(bulletPrefab, bulletSpawn.position, rotation);
                bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * shootingForce, ForceMode.Impulse);
        
                Destroy(bullet, 5f);   
            }
        }
        
        // Recoil
        if (!isPlayer) return;
        _playerRigidbody.AddForce(Camera.main.transform.forward * -recoil);
        
        // Camera shake
        _followCamera.StartShake(fireRate / 2, recoil / 350f);
        
        // Audio
        if (audio == null) return;
        AudioSource.PlayClipAtPoint(audio, transform.position);
    }
}
