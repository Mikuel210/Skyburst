using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyController : MonoBehaviour {

	[FormerlySerializedAs("radius")] [SerializeField] private float _radius;
	private Transform _player;
	private Transform _weapon;
	private WeaponController _weaponController;

	void Start() {
		_weapon = transform.Find("WeaponPivot");
		_weaponController = _weapon.GetComponentInChildren<WeaponController>();
		try { _player = GameObject.FindGameObjectWithTag("Player").transform.Find("Skeleton/Hips/Spine/Chest"); } catch { }
	}

	private float _time;
	
    void Update() {
		_time += Time.deltaTime;

		if (_player == null) return;
		
		transform.LookAt(_player);
		transform.eulerAngles = new(0, transform.eulerAngles.y, 0);
		
		_weapon.LookAt(_player);
		_weapon.eulerAngles = new(_weapon.eulerAngles.x, _weapon.eulerAngles.y, 0);

		if (!Physics.OverlapSphere(transform.position, _radius).ToList().Exists(e => e.CompareTag("Player"))) return;
		if (_time < _weaponController.fireRate) return;
		
		_weaponController.Shoot();
		_time = 0;
	}
}
