using System;
using System.Collections.Generic;
using UnityEngine;
public class Stats
{
    private readonly Dictionary<StatType, float> _baseStats;
    private readonly StatModifiersContainer _modifiers = new(); // todo operation order
    private readonly Dictionary<StatType, float> _modifiedStats = new();

    public float CurrentHealth { get; set; }

    public Stats(Dictionary<StatType, float> stats = null, StatModifiersContainer modifiers = null)
    {
        _baseStats = stats ?? new();
        foreach (StatType type in _baseStats.Keys)
        {
            if (_baseStats.ContainsKey(type)) continue;
            _baseStats[type] = 0;
        }

        foreach (var (k, v) in _baseStats)
        {
            _modifiedStats[k] = v;
        }

        if (modifiers != null)
        {
            _modifiers = modifiers;
            foreach (var (type, _) in _baseStats)
            {
               RecalculateModifiers(type);
            }
        }

        CurrentHealth = Get(StatType.MaxHealth);
    }

    public Stats(SerializableStats stats)
    {
        _baseStats = new();
        foreach (var pair in stats.GetStats())
        {
            if (_baseStats.ContainsKey(pair.Type))
            {
                Debug.LogWarningFormat("Stat type {} appears twice in serializable stats {}, using maximum value", pair.Type, stats);
                if (pair.Value <= _baseStats[pair.Type]) continue;
            }
            _baseStats[pair.Type] = pair.Value;
        }
        foreach (StatType type in _baseStats.Keys)
        {
            if (_baseStats.ContainsKey(type)) continue;
            Debug.LogWarningFormat("Stat type {} is missing in serializable stats {}, using 0", type, stats);
            _baseStats[type] = 0;
        }

        foreach (var (k, v) in _baseStats)
        {
            _modifiedStats[k] = v;
        }

        CurrentHealth = Get(StatType.MaxHealth);
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

    public float Get(StatType type) => _modifiedStats[type];
    public void Set(StatType type, float value) {
        _baseStats[type] = value;
        RecalculateModifiers(type);
    }

    public float GetDamageWhenAttackedBy(Stats attacker)
    {
        return Mathf.Max(1, attacker.Get(StatType.Damage) - Get(StatType.Defense));
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
        float temp = _baseStats[type];

        if (_modifiers.Modifiers.ContainsKey(type))
        {
            foreach (var modifier in _modifiers.Modifiers[type])
                modifier.Apply(ref temp);
        }

        _modifiedStats[type] = temp;
    }

    public void Heal()
    {
        CurrentHealth = Get(StatType.MaxHealth);
    }
}