using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int _disableUntilTurn = 0;
    [SerializeField] private int _spawnCooldown = 5;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private int _maxEnemies = 10;

    private Player _player;
    private ObjectPool _pool;
    private int _turnsSinceSpawn = 0;
    private TurnManager _turnManager;

    private void Awake()
    {
        _player = FindAnyObjectByType<Player>();
        _pool = new(_enemyPrefab, _maxEnemies, transform);

        _turnManager = FindAnyObjectByType<TurnManager>();
        _turnManager.OnTurnChange += TrySpawnEnemy;

        Assert.IsTrue(_disableUntilTurn >= 0);
        Assert.IsTrue(Mathf.Approximately(transform.position.x, 0));
        Assert.IsTrue(Mathf.Approximately(transform.position.y, 0));
        Assert.IsTrue(Mathf.Approximately(transform.position.z, 0));
    }

    private void Start()
    {
        if (_disableUntilTurn == 0)
        {
            // Start with one enemy
            _turnsSinceSpawn = _spawnCooldown;
            TrySpawnEnemy();
        }
    }

    private void TrySpawnEnemy()
    {
        _turnsSinceSpawn++;
        if (_turnManager.CurrentTurn < _disableUntilTurn) return;
        if (_turnsSinceSpawn < _spawnCooldown) return;
        if (_pool.IsFull()) return;

        _turnsSinceSpawn = 0;

        GameObject enemyObj = _pool.ActivateObject();
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        enemy.Pool = _pool;
    }

    public System.Collections.Generic.IEnumerable<GameObject> GetAliveEnemies()
    {
        return _pool.Alive;
    }
}
