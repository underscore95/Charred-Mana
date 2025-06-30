using System.Collections.Generic;
using System.Text;

public class StatModifiersContainer
{
    private readonly Dictionary<StatType, List<StatModifier>> _modifiers = new();

    public StatModifiersContainer()
    {

    }

    public StatModifiersContainer(StatType type, StatModifier modifier)
    {
        Add(type, modifier);
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

    // Invert the container so that applying this and the inverted results in no change
    public void Invert()
    {
        foreach (var (_, modifiers) in Modifiers)
        {
            foreach (var mod in modifiers)
            {
                mod.Invert();
            }
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.AppendLine("StatModifiersContainer{");
        foreach (var (type, modifiers) in Modifiers)
        {
            sb.AppendLine(type + ": [");
            foreach (var mod in modifiers)
            {
                sb.AppendLine(mod.ToString());
            }
            sb.AppendLine("]");
        }
        sb.AppendLine("}");
        return sb.ToString();
    }
}
