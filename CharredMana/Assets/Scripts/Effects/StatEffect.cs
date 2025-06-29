
using System;
using System.Collections;
using UnityEngine;

public abstract class StatEffect : Effect
{
    private ILivingEntity _entity;
    private StatModifiersContainer _appliedLastTurn = null;

    public override void OnPoolEnter()
    {
        _entity = transform.parent.GetComponent<ILivingEntity>();
        StartCoroutine(UpdateStats());
    }

    public override void OnPoolLeave()
    {
        if (_entity != null)
        {
            RemoveStatsAppliedLastTurn();
        }
        _entity = null;
        _appliedLastTurn = null;
    }

    public override void OnTurnChange()
    {
        StartCoroutine(UpdateStats());
    }

    private IEnumerator UpdateStats()
    {
        yield return new WaitForEndOfFrame();
        if (_entity == null) yield break;

        RemoveStatsAppliedLastTurn();
        _appliedLastTurn = GetStatsForCurrentAmplifier();
        _entity.GetStats().ApplyModifiers(_appliedLastTurn);
    }

    protected abstract StatModifiersContainer GetStatsForCurrentAmplifier();

    private void RemoveStatsAppliedLastTurn()
    {
        if (_appliedLastTurn != null)
        {
            _appliedLastTurn.Invert();
            _entity.GetStats().ApplyModifiers(_appliedLastTurn);
            _appliedLastTurn = null;
        }
    }
}