using UnityEngine;
using UnityEngine.Assertions;

public class RangedAttack : MonoBehaviour, IEnemyAttack
{
    [SerializeField] private string _projectileManagerName = "";
    [SerializeField] private float _attackRange = 5;
    [SerializeField] private float _chanceToShoot = 0.3f;
    private ProjectileManager _projectileManager;
    private Player _player;
    private Enemy _enemy;

    private void Awake()
    {
        _player = FindAnyObjectByType<Player>();
        _projectileManager = GameObject.Find(_projectileManagerName).GetComponent<ProjectileManager>();
        _enemy = GetComponent<Enemy>();
        Assert.IsTrue(_projectileManager._shouldUseInspectorProperties);

        int enemyProjectileLayer = LayerMask.NameToLayer("EnemyProjectile");
        Assert.IsTrue(enemyProjectileLayer != -1);
        Assert.IsTrue(enemyProjectileLayer == _projectileManager._prefab.layer, "Projectile shot by ranged enemy must be on enemy projectile layer");
    }

    private void OnValidate()
    {
        GameObject projManager = GameObject.Find(_projectileManagerName);
        if (projManager == null)
        {
            Debug.LogError("No projectile manager component found for ranged enemy: " + _projectileManagerName);
            return;
        }

        if (!projManager.TryGetComponent<ProjectileManager>(out var _))
        {
            Debug.LogError("Projectile manager object " + _projectileManagerName + "doesn't have a projectile manager component");
        }
    }

    public void HandleAttack()
    {
        if (Random.Range(0.0f, 1.0f) > _chanceToShoot) return;

        if (Vector3.Distance(transform.position, _player.PositionAtFrameStart) <= _attackRange)
        {
            Vector3 enemyPos = transform.position;
            Vector3 toPlayer = (_player.PositionAtFrameStart - enemyPos).normalized;

            // Slight random offset to spawn position and direction
            Vector3 spawnPos = enemyPos + toPlayer * 0.5f;
            spawnPos.x += Random.Range(-0.15f, 0.15f);
            spawnPos.y += Random.Range(-0.15f, 0.15f);
            toPlayer.x += Random.Range(-0.01f, 0.01f);
            toPlayer.y += Random.Range(-0.01f, 0.01f);

            // Spawn projectile
            GameObject projObj = _projectileManager.SpawnProjectile(_enemy).gameObject;
            projObj.transform.SetPositionAndRotation(
                spawnPos,
                Quaternion.Euler(0, 0, Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg)
            );
        }
    }
}
