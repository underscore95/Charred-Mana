using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class SerializableStatModifiersContainer
{
    [Serializable]
    public struct Entry
    {
        public StatType Type;
        public StatModifier Modifier;

        public override readonly string ToString()
        {
            return "{" + Type + ": " + Modifier.ToString(3) + "}";
        }
    }

    [SerializeField]
    private List<Entry> entries = new();

    public List<Entry> Entries => entries;

    public void AddModifierNoMerging(StatType type,StatModifier modifier)
    {
        Entries.Add(new Entry { Type = type, Modifier = modifier });
    }

    public static implicit operator StatModifiersContainer(SerializableStatModifiersContainer serializable)
    {
        var container = new StatModifiersContainer();
        var seen = new HashSet<StatType>();

        foreach (var entry in serializable.entries)
        {
            if (!seen.Add(entry.Type))
                Debug.LogWarning($"StatType {entry.Type} appears multiple times in SerializableStatModifiersContainer");

            container.Add(entry.Type, entry.Modifier);
        }

        return container;
    }

    public override string ToString()
    {
        StringBuilder s = new();
        s.AppendLine("SerializableStatsContainer[");
        foreach (var entry in entries) s.AppendLine(entry.ToString());
        s.AppendLine("]");
        return s.ToString();
    }
}
