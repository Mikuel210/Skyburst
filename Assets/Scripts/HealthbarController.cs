using UnityEngine;
using UnityEngine.UI;

public class HealthbarController : MonoBehaviour {
	
	[SerializeField] private Health health;
	[SerializeField] private Slider _slider;
	[SerializeField] private bool hide = true;
    
	void Start()
	{
		health ??= transform.parent.GetComponent<Health>();
		_slider ??= transform.Find("Slider").GetComponent<Slider>();
	}

	void Update() {
		if (health == null) return;
		
		UIManager.UpdateSlider(_slider, health.CurrentHealth / health.MaximumHealth * 100f, false);
        
		if (hide && health.CurrentHealth == health.MaximumHealth)
			_slider.gameObject.SetActive(false);
		else
			_slider.gameObject.SetActive(true);
	}
}