using System;
using StarterAssets;
using UnityEngine;
using UnityEngine.Serialization;

public class Health : MonoBehaviour {

	public AudioClip damageClip;
	
	[SerializeField] private float _recoverHealth;

	[SerializeField] private bool _isPlayer;
	[field: SerializeField] public float MaximumHealth { get; private set; } = 100f;

	[SerializeField] private float currentHealth;
	public float CurrentHealth
	{
		get => currentHealth;
        
		private set
		{
			currentHealth = Mathf.Clamp(value, 0, MaximumHealth);
            
			OnHealthChanged?.Invoke();

			if (currentHealth <= 0)
			{
				OnDeath?.Invoke();
			}
		}
	}
    
	[SerializeField] private bool destroyOnDeath = true;
    
	public event Action OnDeath;
	public event Action OnHealthChanged;
    
	public void TakeDamage(float damage) => CurrentHealth -= damage;
	public void Heal(float heal) => CurrentHealth += heal;

	void Start()
	{
		CurrentHealth = MaximumHealth;
		
		OnDeath += () => {
			if (_isPlayer) {
				GameManager.Instance.gameOver = true;
				UIManager.Instance.GameOver();
				transform.parent.GetComponent<PlayerController>().dead = true;
				transform.parent.GetComponent<PlayerController>().UpdateWeapons();
				gameObject.SetActive(false);
				StarterAssetsInputs.Instance.cursorLocked = false;
				StarterAssetsInputs.Instance.OnApplicationFocus(true);
			}
			
			if (destroyOnDeath) Destroy(gameObject);
		};
	}

	void Update() {
		Heal(_recoverHealth * Time.deltaTime);
	}
	
	private void OnTriggerEnter(Collider other) {
		if (!other.gameObject.CompareTag("Projectile")) return;

		var data = other.gameObject.GetComponent<ProjectileData>();
		if (data.damagePlayer != _isPlayer) return;
		
		if (!_isPlayer) UIManager.Popup(data.damage.ToString(), transform.position + Vector3.up, Color.red);
		if (damageClip != null) AudioSource.PlayClipAtPoint(damageClip, transform.position);
		
		Destroy(other.gameObject);
		TakeDamage(data.damage);
	}

}