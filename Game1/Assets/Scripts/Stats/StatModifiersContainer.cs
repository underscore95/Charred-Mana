using System.Collections.Generic;

public class StatModifiersContainer
{
    private readonly Dictionary<StatType, List<StatModifier>> _modifiers = new();

    public StatModifiersContainer()
    {

    }

    // Copy constructor
    public StatModifiersContainer(StatModifiersContainer modifiers)
    {
        Merge(modifiers);
    }

    public IReadOnlyDictionary<StatType, List<StatModifier>> Modifiers => _modifiers;

    public void Add(StatType type, StatModifier modifier)
    {
        if (!_modifiers.TryGetValue(type, out var list))
            list = _modifiers[type] = new();

        StatModifier.ApplyModifierToList(ref list, modifier);
    }

    public void Merge(StatModifiersContainer other)
    {
        foreach (var (type, modifiers) in other.Modifiers)
        {
            if (!_modifiers.ContainsKey(type)) _modifiers[type] = new();
            List<StatModifier> ourMods = _modifiers[type];
            foreach (StatModifier mod in modifiers)
            {
                StatModifier.ApplyModifierToList(ref ourMods, mod);
            }
        }
    }
}
