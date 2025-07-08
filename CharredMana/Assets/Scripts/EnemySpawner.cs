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
    [SerializeField] private int _floor = 0;

    private ObjectPool _pool;
    private int _turnsSinceSpawn = 0;
    private TurnManager _turnManager;
    private FloorManager _floorManager;

    private void Awake()
    {
        _turnManager = FindAnyObjectByType<TurnManager>();

        _floorManager = FindAnyObjectByType<FloorManager>();
        _floorManager.OnFloorChange += OnFloorChange;

        Assert.IsTrue(_activeDuringTurns.Begin >= 0);
        Assert.IsTrue(_activeDuringTurns.End >= _activeDuringTurns.Begin);
        Assert.IsTrue(Mathf.Approximately(transform.position.x, 0));
        Assert.IsTrue(Mathf.Approximately(transform.position.y, 0));
        Assert.IsTrue(Mathf.Approximately(transform.position.z, 0));
        Assert.IsTrue(_floor > 0, "Floor must be greater than 0");

        if (transform.parent.name != "Floor" + _floor)
        {
            Debug.LogWarning($"Parent of enemy spawner {name} was not Floor{_floor}, is that correct? It will work, this warning just lets you know you could have the spawner as a child of the wrong parent. Parent is actually {transform.parent.name}");
        }
    }

    private void OnFloorChange()
    {
        if (_floorManager.Floor == _floor)
        {
            _turnManager.OnTurnChange += TrySpawnEnemy;
            _pool = new(_enemyPrefab, _maxEnemies, transform);

            _activeDuringTurns.OffsetRangeBy(_turnManager.FloorEnterTurn);
            if (_activeDuringTurns.Begin == _turnManager.FloorEnterTurn)
            {
                // Start with one enemy
                _turnsSinceSpawn = _spawnCooldown;
                TrySpawnEnemy();
            }
        }
        else if (_floorManager.Floor > _floor)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        _turnManager.OnTurnChange -= TrySpawnEnemy;
        _floorManager.OnFloorChange -= OnFloorChange;
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
