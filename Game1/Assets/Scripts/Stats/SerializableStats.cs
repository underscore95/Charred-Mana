
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatEntry
{
    public StatType Type;
    public float Value;
}

[Serializable]
public class SerializableStats
{
    [SerializeField]
    private List<StatEntry> _serializedStats = new();

    public List<StatEntry> GetStats()
    {
        return _serializedStats;
    }

    public static implicit operator Stats(SerializableStats s) => new(s);
}