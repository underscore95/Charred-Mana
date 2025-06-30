using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private IntRange _activeDuringTurns = new(0, 999999);
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

        Assert.IsTrue(_activeDuringTurns.Begin >= 0);
        Assert.IsTrue(_activeDuringTurns.End >= _activeDuringTurns.Begin);
        Assert.IsTrue(Mathf.Approximately(transform.position.x, 0));
        Assert.IsTrue(Mathf.Approximately(transform.position.y, 0));
        Assert.IsTrue(Mathf.Approximately(transform.position.z, 0));
    }

    private void Start()
    {
        if (_activeDuringTurns.Begin == 0)
        {
            // Start with one enemy
            _turnsSinceSpawn = _spawnCooldown;
            TrySpawnEnemy();
        }
    }

    private void OnDestroy()
    {
        _turnManager.OnTurnChange -= TrySpawnEnemy;
    }

    private void TrySpawnEnemy()
    {
        _turnsSinceSpawn++;
        if (_turnManager.CurrentTurn < _activeDuringTurns.Begin) return;
        if (_turnManager.CurrentTurn > _activeDuringTurns.End)
        {
            if (GetAliveEnemies().Count() == 0)
            {
                // we're disabled and all our enemies are dead, we can safely remove game object
                Destroy(gameObject);
            }
            return;
        }
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
