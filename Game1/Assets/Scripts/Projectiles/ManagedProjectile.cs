using UnityEngine;

/// <summary>
/// Component added to any projectile managed by a projectile manager
/// </summary>
class ManagedProjectile : MonoBehaviour
{
    public ProjectileManager _projManager;

    public void DestroyProjectile()
    {
        _projManager.RemoveProjectile(gameObject);
    }

    private void OnBecameInvisible()
    {
        if (enabled)
        {
            DestroyProjectile();
        }
    }
}