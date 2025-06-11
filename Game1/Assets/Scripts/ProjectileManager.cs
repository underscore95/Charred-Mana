using System;
using UnityEngine;
using UnityEngine.Assertions;

public class ProjectileManager : MonoBehaviour
{
    private ObjectPool _projectiles;

    public void Init(GameObject proj)
    {
        Assert.IsNull(_projectiles);
        _projectiles = new(proj, 10, transform);
        foreach (var projectile in _projectiles.AliveAndDead)
        {
            projectile.AddComponent<ManagedProjectile>()._projManager = this;
        }
    }

    private void Start()
    {
        Assert.IsNotNull(_projectiles);
    }

    public GameObject SpawnProjectile()
    {
        if (_projectiles.IsFull())
        {
            _projectiles.ReleaseOldestObject();
        }
        return _projectiles.ActivateObject();
    }

    public void RemoveProjectile(GameObject proj)
    {
        _projectiles.ReleaseObject(proj);
    }
}
