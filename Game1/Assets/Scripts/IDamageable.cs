using UnityEngine.Assertions;

public interface IDamageable : IHasStats
{
    // Damage the damageable, returns true if the entity is dead
    public static bool Damage(IDamageable damageable, float damage)
    {
        Assert.IsTrue(damage > 0);
        damageable.GetStats().CurrentHealth -= damage;
        damageable.DamageReceiveEvent(damage);
        return damageable.GetStats().CurrentHealth <= 0;
    }

    void DamageReceiveEvent(float damage);
}