using UnityEngine;

public class DamageOverTimeEffect : Effect
{
    private LivingEntity _entity;

    public override void OnPoolEnter()
    {
        _entity = transform.parent.GetComponent<LivingEntity>();   
    }

    public override void OnPoolLeave()
    {
        _entity = null;
    }

    public override void OnTurnChange()
    {
        if (_entity == null) return;

        LivingEntity.Damage(_entity, Amplifier, DamageSource.StatusEffect);
    }
}
