using System.Collections.Generic;
using System.Linq;
using Helpers;
using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _timeText2;
    
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemy2Prefab;
    
    [SerializeField] private float _interval;
    [SerializeField] private float _intervalDecreaseSpeed;
    [SerializeField] private float _minInterval;
    
    [SerializeField] private float _lavaRiseSpeed;
    [SerializeField] private float _maxLavaHeight;
    [SerializeField] private float _lavaOffsetSpeed;
    
    private GameObject _lava;
    private Material _lavaMaterial;
    
    [SerializeField] private List<GameObject> _spawns;
    private float _time;
    private float _startingInterval;

    public bool gameOver;

    void Start() {
        _lava = GameObject.Find("Lava");
        _lavaMaterial = _lava.GetComponent<Renderer>().material;
        _spawns = GameObject.FindGameObjectsWithTag("EnemySpawn").ToList();
        _startingInterval = _interval;
    }
    
    void Update() {
        if (!gameOver) {
            var time = Time.timeSinceLevelLoad;
            string minutes = (time / 60).ToString("00");
            string seconds = (time % 60).ToString("00");
            _timeText.text = $"<mspace=0.45em>{minutes}</mspace>:<mspace=0.45em>{seconds}";
            _timeText2.text = $"<mspace=0.45em>{minutes}</mspace>:<mspace=0.45em>{seconds}";   
        }
        
        _time += Time.deltaTime;
        _lavaMaterial.mainTextureOffset = new(_lavaMaterial.mainTextureOffset.x + _lavaOffsetSpeed * Time.deltaTime, 0);
        
        if (_interval > _minInterval)
            _interval -= _intervalDecreaseSpeed * Time.deltaTime;
        
        if (_lava.transform.position.y < _maxLavaHeight)
            _lava.transform.Translate(new Vector3(0, _lavaRiseSpeed, 0) * Time.deltaTime);

        if (_time < _interval) return;

        foreach (var spawn1 in new List<GameObject>(_spawns))
            if (spawn1.transform.Find("MaxHeight").position.y <= _lava.transform.position.y) _spawns.Remove(spawn1);
        
        GameObject spawn = _spawns[Random.Range(0, _spawns.Count)];
        var colliders = spawn.GetComponents<BoxCollider>();
        var collider = colliders[Random.Range(0, colliders.Length - 1)];
        Vector3 point = RandomPointInBounds(collider.bounds);

        var progress = 1 - (_interval - _minInterval) / _startingInterval;
        var chance = progress * 0.1f;
        var enemy = Random.Range(0f, 1f) <= chance ? _enemy2Prefab : _enemyPrefab;
        
        Instantiate(enemy, point, Quaternion.identity);
        
        _time = 0;
    }
    
    public static Vector3 RandomPointInBounds(Bounds bounds) {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}
