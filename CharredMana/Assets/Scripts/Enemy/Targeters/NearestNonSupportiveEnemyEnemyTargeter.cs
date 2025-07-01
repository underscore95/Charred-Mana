using UnityEngine;

public class NearestNonSupportiveEnemyEnemyTargeter : MonoBehaviour, IEnemyTargeter
{
    public ILivingEntity GetTarget()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        float closestDistance = float.MaxValue;
        ILivingEntity closest = null;
        Vector2 xy = transform.position;
        foreach (Enemy enemy in enemies)
        {
            if (enemy.gameObject == gameObject) continue;
            if (enemy.ShouldBeIgnoredBySupportiveEnemies()) continue;
            float distance = Vector3.SqrMagnitude((Vector2)enemy.transform.position - xy);
            if (distance < closestDistance)
            {
                closest = enemy;
                closestDistance = distance;
            }
        }

        closest ??= FindAnyObjectByType<Player>(); // backup so we don't ever have no target
        return closest;
    }
}
