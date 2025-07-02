using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public abstract class LivingEntity : MonoBehaviour
{
    private Stats _stats;
    public Stats EntityStats
    {
        get
        {
            return _stats;
        }
        protected set
        {
            _stats = value;
        }
    }
    // Damage the entity, returns true if the entity is dead
    public static bool Damage(LivingEntity entity, float damage)
    {
        Assert.IsTrue(damage > 0);
        entity.EntityStats.SetCurrentHealthSilently(entity.EntityStats.CurrentHealth - damage);
        entity.OnDamaged().Invoke(damage);
        return entity.EntityStats.CurrentHealth <= 0;
    }

    // Event that will be called whenever the entity takes damage
    // parameters:
    // float damage
    private UnityAction<float> _onDamaged = (dmg) => { };

    public ref UnityAction<float> OnDamaged() { return ref _onDamaged; }
}