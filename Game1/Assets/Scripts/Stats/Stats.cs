using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Stats : StatContainer
{
    private readonly StatContainer _baseStats;
    private readonly StatModifiersContainer _modifiers = new(); // todo operation order

    public float CurrentHealth { get; private set; }

    public Stats(StatContainer stats = null, StatModifiersContainer modifiers = null)
    {
        _baseStats = new StatContainer(stats) ?? new();
        SetEqualTo(_baseStats);

        if (modifiers != null)
        {
            _modifiers = modifiers;
            foreach (StatType type in Enum.GetValues(typeof(StatType)))
            {
                RecalculateModifiers(type);
            }
        }

        CurrentHealth = MaxHealth;
    }

    public void ApplyModifier(StatType type, StatModifier mod)
    {
        _modifiers.Add(type, mod);
        RecalculateModifiers(type);
    }

    public void ApplyModifiers(StatModifiersContainer currentEnemyStatBoost)
    {
        foreach (var (type, mods) in currentEnemyStatBoost.Modifiers)
        {
            foreach (var mod in mods)
            {
                ApplyModifier(type, mod);
            }
        }
    }

    public void SetBaseStat(StatType type, float value)
    {
        _baseStats.Set(type, value);
        RecalculateModifiers(type);
    }

    public new void Set(StatType type, float value)
    {
        Debug.LogError("Attempted to use Set on a Stats, operation not supported"); // Probably looking for SetBaseStat, the modified stat is automatically calculated and cannot be set
    }

    public float GetDamageWhenAttackedBy(Stats attacker)
    {
        float attackerDamage = attacker.Get(StatType.Damage);
        float defense = Get(StatType.Defense);
        return Mathf.Max(1, attackerDamage - defense);
    }

    public Stats DuplicateAndAddModifiers(StatModifiersContainer modifiers)
    {
        StatModifiersContainer mods = new(_modifiers);
        mods.Merge(modifiers);
        Stats s = new(_baseStats, mods);
        return s;
    }

    public bool IsDead()
    {
        return CurrentHealth <= 0;
    }

    // Recalculate the value after modifiers for a specific stat type
    private void RecalculateModifiers(StatType type)
    {
        float temp = _baseStats.Get(type);

        if (_modifiers.Modifiers.ContainsKey(type))
        {
            List<StatModifier> modifiers = _modifiers.Modifiers[type];
            // sort based on op
            modifiers.Sort((a, b) => MathOps.StatOpApplyOrder(a.Operation).CompareTo(MathOps.StatOpApplyOrder(b.Operation)));

            foreach (var modifier in modifiers)
                modifier.Apply(ref temp);
        }

        base.Set(type, temp);
    }

    public void Heal()
    {
        CurrentHealth = Get(StatType.MaxHealth);
    }

    // Sets the current health, this will not notify any death listeners, use ILivingEntity.Damage if damaging
    internal void SetCurrentHealthSilently(float value)
    {
        CurrentHealth = value;
    }
}