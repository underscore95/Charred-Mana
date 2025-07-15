using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] internal bool _shouldUseInspectorProperties = false;
    [SerializeField] internal GameObject _prefab;
    [SerializeField] private int _capacity = 10;
    [SerializeField] private bool _releaseProjectilesOnFloorChange = true;

    private ObjectPool _projectiles;
    private FloorManager _floorManager;

    private void Awake()
    {
        _floorManager = FindAnyObjectByType<FloorManager>();

        if (_shouldUseInspectorProperties)
        {
            Init(_prefab);
        }

        if (!_shouldUseInspectorProperties || !_releaseProjectilesOnFloorChange)
        {
            _floorManager.OnFloorChange += FloorChange;
        }
    }

    private void OnDestroy()
    {
        if (!_shouldUseInspectorProperties || !_releaseProjectilesOnFloorChange)
        {
            _floorManager.OnFloorChange -= FloorChange;
        }
    }

    private void OnValidate()
    {
        if (!_shouldUseInspectorProperties)
        {
            Assert.IsNull(_prefab, "Projectile manager is set to not use inspector properties, but a projectile prefab is set");
        }
    }

    private void FloorChange()
    {
        ReleaseAllProjectiles();
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

    public void ReleaseProjectile(GameObject proj)
    {
        _projectiles.ReleaseObject(proj);
    }

    public void ReleaseAllProjectiles()
    {
        for (int i = _projectiles.CurrentSize; i > 0; i--)
        {
            _projectiles.ReleaseOldestObject();
        }
        Assert.IsTrue(_projectiles.CurrentSize == 0);
    }

    public ObjectPool InternalPool() { return _projectiles; }
}
