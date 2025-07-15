using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Component added to any projectile managed by a projectile manager
/// </summary>
class ManagedProjectile : MonoBehaviour
{
    private const int LIFESPAN = 15;

    public ProjectileManager _projManager;
    private int _destroyOnTurn = 0;
    private TurnManager _turnManager;

    private void Awake()
    {
        _turnManager = FindAnyObjectByType<TurnManager>();
    }

    private void OnEnable()
    {
        _destroyOnTurn = _turnManager.CurrentTurn + LIFESPAN;
    }

    public void ReleaseProjectile()
    {
        _projManager.ReleaseProjectile(gameObject);
    }

    public bool IsPastLifespan()
    {
        Assert.IsTrue(enabled);
        return _turnManager.CurrentTurn >= _destroyOnTurn;
    }
}