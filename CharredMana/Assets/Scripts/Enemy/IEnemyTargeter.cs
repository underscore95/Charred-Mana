using UnityEngine;

// What entity should the enemy target?
public interface IEnemyTargeter
{
    ILivingEntity GetTarget();
}
