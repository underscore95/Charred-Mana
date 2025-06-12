using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private SerializableStats _serialisedStats = new();
    private Stats _stats;

    private Player _player;
    private IEnemyAttack _attack;
    private GameObject _spawner;
    private Rigidbody2D _rigidBody;
    public ObjectPool Pool; // pool that contains this enemy
    private TurnManager _turnManager;

    private void Awake()
    {
        _player = FindAnyObjectByType<Player>();
        _attack = GetComponent<IEnemyAttack>();
        _spawner = transform.parent.gameObject;
        _rigidBody = GetComponent<Rigidbody2D>();
        _turnManager = FindAnyObjectByType<TurnManager>();
    }

    private void Start()
    {
        Assert.IsNotNull(Pool);
    }

    private void OnEnable()
    {
        _turnManager.OnTurnChange += PlayTurn;
        _stats = _serialisedStats;
        _stats.ApplyModifiers(_turnManager.CurrentEnemyStatBoost);
        _stats.Heal();
    }

    private void OnDisable()
    {
        _turnManager.OnTurnChange -= PlayTurn;
    }

    private void PlayTurn()
    {
        Vector3 deltaPos = new(
            _player.PositionAtFrameStart.x - transform.position.x,
            _player.PositionAtFrameStart.y - transform.position.y,
            0.0f
        );

        foreach (Transform obj in _spawner.transform)
        {
            // boids
        }

        StartCoroutine(Utils.MoveRigidBody(_rigidBody, deltaPos.normalized));

        _attack.TryAttack();
    }

    void IDamageable.DamageReceiveEvent(float damage)
    {
        if (_stats.IsDead())
        {
            Pool.ReleaseObject(gameObject);
        }
    }

    public Stats GetStats()
    {
        return _stats;
    }
}
