using UnityEngine;

public class DamageOverTimeEffect : Effect
{
    private ILivingEntity _entity;

    public override void OnPoolEnter()
    {
        _entity = transform.parent.GetComponent<ILivingEntity>();   
    }

    public override void OnPoolLeave()
    {
        _entity = null;
    }

    public override void OnTurnChange()
    {
        print("damaging for " + Amplification + " new health: " + (_entity.GetStats().CurrentHealth - Amplification));
        ILivingEntity.Damage(_entity, Amplification);
    }
}
