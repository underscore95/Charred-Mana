using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class Enemy : LivingEntity
{
    [SerializeField] private StatContainer _baseStats = new();
    [SerializeField] private float _experienceDropped = 10;
    [SerializeField] private float _maxDistanceToPlayerBeforeTeleporting = 15.0f;
    [SerializeField] private bool _shouldBeIgnoredBySupportiveEnemies = false;

    private Player _player;
    private IEnemyAttack _attack;
    private IEnemyTargeter _targeter;
    public IEnemyController Controller { get; private set; }
    public LivingEntity CurrentTarget { get; private set; }
    private ObjectPoolRef _pool;
    private TurnManager _turnManager;
    private EffectManager _effectManager;
    private bool _isDead = false;

    private void Awake()
    {
        _player = FindAnyObjectByType<Player>();
        _attack = GetComponent<IEnemyAttack>();
        _turnManager = FindAnyObjectByType<TurnManager>();
        Controller = GetComponent<IEnemyController>();
        _targeter = GetComponent<IEnemyTargeter>();
        _effectManager = FindAnyObjectByType<EffectManager>();
        _pool = GetComponent<ObjectPoolRef>();

        OnDamaged() += OnDamage;
    }

    private void OnEnable()
    {
        _isDead = false;
        transform.position = PickSpawnLocation();

        _turnManager.OnTurnChange += PlayTurn;
        EntityStats = new(_baseStats);
        EntityStats.ApplyModifiers(_turnManager.CurrentEnemyStatBoost);
        EntityStats.Heal();
    }

    private void OnDisable()
    {
        _turnManager.OnTurnChange -= PlayTurn;
    }

    private void PlayTurn()
    {
        float distToPlayerSquared = Vector3.SqrMagnitude((Vector2)transform.position - (Vector2)_player.transform.position);
        if (distToPlayerSquared > _maxDistanceToPlayerBeforeTeleporting * _maxDistanceToPlayerBeforeTeleporting)
        {
            transform.position = PickSpawnLocation();
        }

        CurrentTarget = _targeter.GetTarget();
        Controller.HandleMovement();
        _attack.HandleAttack();
    }

    private void OnDamage(float damage)
    {
        if (EntityStats.IsDead())
        {
            if (_isDead) return;
            _isDead = true;
            _player.MonstersKilled++;
            _player.PlayerLevel.Experience += _experienceDropped;

            _effectManager.RemoveAllEffects(this);

            _pool.Pool.ReleaseObject(gameObject);
        }
    }

    public static Collider2D[] GetNearbyEnemies(Vector2 position, float range)
    {
        Assert.IsTrue(LayerMask.NameToLayer("Enemy") != -1);
        return Physics2D.OverlapCircleAll(position, range, LayerMask.GetMask("Enemy"));
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
        else offset.x += cameraSize.x * 1.1f;
        if (offset.y < 0) offset.y -= cameraSize.y * 1.1f;
        else offset.y += cameraSize.y * 1.1f;

        return location + offset;
    }

    public bool ShouldBeIgnoredBySupportiveEnemies()
    {
        return _shouldBeIgnoredBySupportiveEnemies;
    }
}
