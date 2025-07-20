using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public abstract class BaseSpawner : ObjectPoolMonoBehaviour
{
    [SerializeField] protected int _spawnInterval = 10;
    [SerializeField] protected bool _overwriteOldest = true; // If true, delete oldest object when exceeded capacity and spawning a new one, if false then don't spawn any more
    [SerializeField] protected Vector2 _minDistanceFromPlayer = new(4.5f, 7.5f); // offscreen assuming sprite is 0.1x0.1 
    [SerializeField] protected Vector2 _maxDistanceFromPlayer = new(6, 9);

    public UnityAction<GameObject> OnEntitySpawn { get; set; } = ent => { };

    protected int _turnsSinceSpawn;
    protected Player _player;
    protected TurnManager _turnManager;
    protected bool _disable = false;

    protected new void Awake()
    {
        base.Awake();

        _player = FindAnyObjectByType<Player>();
        _turnManager = FindAnyObjectByType<TurnManager>();

        Assert.IsTrue(_minDistanceFromPlayer.x <= _maxDistanceFromPlayer.x);
        Assert.IsTrue(_minDistanceFromPlayer.y <= _maxDistanceFromPlayer.y);
        Assert.IsTrue(Mathf.Approximately(transform.position.x, 0));
        Assert.IsTrue(Mathf.Approximately(transform.position.y, 0));
    }

    protected void OnEnable()
    {
        _turnManager.OnTurnChange += HandleTurnChange;
    }

    protected void OnDisable()
    {
        _turnManager.OnTurnChange -= HandleTurnChange;
    }

    private void HandleTurnChange()
    {
        if (_disable) return;

        _turnsSinceSpawn++;

        if (_turnsSinceSpawn < _spawnInterval) return;

        _turnsSinceSpawn = 0;

        if (_pool.IsFull())
        {
            if (!_overwriteOldest) return;
            _pool.ReleaseOldestObject();
        }

        GameObject ent = _pool.ActivateObject(); ent.transform.position = GetRandomPosition();
        OnEntitySpawn.Invoke(ent);
    }

    private Vector3 GetRandomPosition()
    {
        Vector2 position = new(
            Random.Range(_minDistanceFromPlayer.x, _maxDistanceFromPlayer.x),
            Random.Range(_minDistanceFromPlayer.y, _maxDistanceFromPlayer.y)
            );

        if (Random.Range(0, 2) == 0) position.x *= -1;
        if (Random.Range(0, 2) == 0) position.y *= -1;

        position.x += _player.transform.position.x;
        position.y += _player.transform.position.y;

        return ((Vector3)position) + Vector3.forward * _player.transform.position.z;
    }
}
