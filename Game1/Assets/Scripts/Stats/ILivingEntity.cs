using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public interface ILivingEntity
{
    Stats GetStats();
    GameObject GetGameObject();

    // Damage the entity, returns true if the entity is dead
    public static bool Damage(ILivingEntity entity, float damage)
    {
        Assert.IsTrue(damage > 0);
        entity.GetStats().SetCurrentHealthSilently(entity.GetStats().CurrentHealth - damage);
        entity.OnDamaged().Invoke(damage);
        return entity.GetStats().CurrentHealth <= 0;
    }

    // Event that will be called whenever the entity takes damage
    // parameters:
    // float damage
    ref UnityAction<float> OnDamaged();
}