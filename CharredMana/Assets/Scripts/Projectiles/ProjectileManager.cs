using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] internal bool _shouldUseInspectorProperties = false;
    [SerializeField] internal GameObject _prefab;
    [SerializeField] private int _capacity = 10;

    private ObjectPool _projectiles;

    private void Awake()
    {
        if (_shouldUseInspectorProperties)
        {
            Init(_prefab);
        }
    }

    private void OnValidate()
    {
        if (!_shouldUseInspectorProperties)
        {
            Assert.IsNull(_prefab, "Projectile manager is set to not use inspector properties, but a projectile prefab is set");
        }
    }

    public void Init(GameObject proj)
    {
        Assert.IsNull(_projectiles);
        _projectiles = new(proj, _shouldUseInspectorProperties ? _capacity : 10, transform);
        foreach (var projectile in _projectiles.AliveAndDead)
        {
            projectile.AddComponent<ManagedProjectile>()._projManager = this;
        }

        FindAnyObjectByType<TurnManager>().OnLateTurnChange += OnLateTurnChange;
    }

    private void Start()
    {
        Assert.IsNotNull(_projectiles);
    }

    private readonly List<ManagedProjectile> _projectilesToReleaseAfterTurn = new();
    private void OnLateTurnChange()
    {
        _projectilesToReleaseAfterTurn.Clear();
        foreach (var proj in _projectiles.Alive)
        {
            var m = proj.GetComponent<ManagedProjectile>();
            if (m.IsPastLifespan())
            {
                _projectilesToReleaseAfterTurn.Add(m);
            }
        }
        foreach (ManagedProjectile m in _projectilesToReleaseAfterTurn)
        {
            m.ReleaseProjectile();
        }
    }

    public Projectile SpawnProjectile(LivingEntity shooter)
    {
        if (_projectiles.IsFull())
        {
            _projectiles.ReleaseOldestObject();
        }
        Projectile projectile = _projectiles.ActivateObject().GetComponent<Projectile>();
        projectile.Shooter = shooter;
        return projectile;
    }

    public void RemoveProjectile(GameObject proj)
    {
        _projectiles.ReleaseObject(proj);
    }

    public ObjectPool InternalPool() { return _projectiles; }
}
