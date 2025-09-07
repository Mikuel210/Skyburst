using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    [Serializable]
    public struct Weapon {

        public WeaponController controller;
        public Image image;
        public Image button;

    }

    public List<Weapon> weapons;
    private int _weaponIndex;
    
    void Start()
    {
        UpdateWeapons();
    }
    
    void Update()
    {
        foreach (var weapon in weapons)
            weapon.controller.time += Time.deltaTime;
        
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            _weaponIndex = 0;
            UpdateWeapons();
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            _weaponIndex = 1;
            UpdateWeapons();
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            _weaponIndex = 2;
            UpdateWeapons();
        }

        UpdateUI();
    }

    public bool dead;

    public void UpdateWeapons()
    {
        for (int i = 0; i < weapons.Count; i++) {
            WeaponController weapon = weapons[i].controller;
            weapon.gameObject.SetActive(!dead && i == _weaponIndex);
        }
    }

    private void UpdateUI() {
        int i = 0;
        
        foreach (Weapon weapon in weapons) {
            weapon.image.fillAmount = weapon.controller.time / weapon.controller.fireRate;
            weapon.button.color = _weaponIndex == i ? new(0.5f, 0.5f, 0.5f) : new(1, 1, 1); 

            i++;
        }
    }

}
