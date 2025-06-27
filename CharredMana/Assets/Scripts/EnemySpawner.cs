using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int _spawnCooldown = 5;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyPrefab2;

    private Player _player;
    private ObjectPool _pool;
    private int _turnsSinceSpawn = 0;
    private TurnManager _turnManager;

    private void Awake()
    {
        _player = FindAnyObjectByType<Player>();
        _pool = new(_enemyPrefab, 10, transform);

        _turnManager = FindAnyObjectByType<TurnManager>();
        _turnManager.OnTurnChange += TrySpawnEnemy;
    }

    private void Start()
    {
        // Start with one enemy
        _turnsSinceSpawn = _spawnCooldown;
        TrySpawnEnemy();

        var pool = new ObjectPool(_enemyPrefab2, 1, transform);
        GameObject obj=pool.ActivateObject();
        obj.transform.position += Vector3.right * 4;
        obj.GetComponent<Enemy>().Pool = _pool;
    }

    private void TrySpawnEnemy()
    {
        _turnsSinceSpawn++;
        if (_turnsSinceSpawn < _spawnCooldown) return;
        if (_pool.IsFull()) return;

        _turnsSinceSpawn = 0;

        GameObject enemyObj = _pool.ActivateObject();
        enemyObj.transform.position = PickSpawnLocation();
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        enemy.Pool = _pool;
    }

    private Vector3 PickSpawnLocation()
    {
        Vector3 location = _player.transform.position;
        Vector2 cameraSize = _player.Camera.orthographicSize * new Vector2(Screen.width / Screen.height, 1);
        Vector3 offset = new Vector3(
            Random.Range(-cameraSize.x, cameraSize.x),
            Random.Range(-cameraSize.y, cameraSize.y),
            0) * 0.25f;
        if (offset.x < 0) offset.x -= cameraSize.x * 1.1f;
        else offset.x += cameraSize.x * 1.5f;
        if (offset.y < 0) offset.y -= cameraSize.y * 1.1f;
        offset.x = Mathf.Round(offset.x);
        offset.y = Mathf.Round(offset.y);
        return location + offset;
    }

    public System.Collections.Generic.IEnumerable<GameObject> GetAliveEnemies()
    {
        return _pool.Alive;
    }
}
