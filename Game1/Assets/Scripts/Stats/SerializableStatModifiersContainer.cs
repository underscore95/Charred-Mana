using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableStatModifiersContainer
{
    [Serializable]
    public struct Entry
    {
        public StatType Type;
        public StatModifier Modifier;
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
}
