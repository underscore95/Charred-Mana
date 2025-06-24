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
        ILivingEntity.Damage(_entity, Amplification);
    }
}
