using UnityEngine;
using UnityEngine.Assertions;

public interface ILivingEntity
{
    Stats GetStats();
    GameObject GetGameObject();

    // Damage the entity, returns true if the entity is dead
    public static bool Damage(ILivingEntity entity, float damage)
    {
        Assert.IsTrue(damage > 0);
        entity.GetStats().SetCurrentHealthSilently(entity.GetStats().CurrentHealth - damage);
        entity.DamageReceiveEvent(damage);
        return entity.GetStats().CurrentHealth <= 0;
    }

    void DamageReceiveEvent(float damage);
}