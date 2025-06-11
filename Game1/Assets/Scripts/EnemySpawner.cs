using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int _spawnCooldown = 5;
    [SerializeField] private GameObject _enemyPrefab;

    private Player _player;
    private ObjectPool _pool;
    private int _turnsSinceSpawn = 0;

    private void Awake()
    {
        _player = FindAnyObjectByType<Player>();
        _pool = new(_enemyPrefab, 10, transform);

        TurnManager.Instance().OnTurnChange += TrySpawnEnemy;
    }

    private void TrySpawnEnemy()
    {
        _turnsSinceSpawn++;
        if (_turnsSinceSpawn < _spawnCooldown) return;
        if (_pool.IsFull()) return;

        _turnsSinceSpawn = 0;

        GameObject enemy = _pool.ActivateObject();
        enemy.transform.position = PickSpawnLocation();
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
        return  location + offset;
    }
}
