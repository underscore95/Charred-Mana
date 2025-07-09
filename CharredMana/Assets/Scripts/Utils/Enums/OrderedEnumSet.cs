using System;
using System.Collections.Generic;
using UnityEngine;

// Ordered collection of enums with no duplicates
[Serializable]
public class OrderedEnumSet<K> where K : Enum
{
    [SerializeField] private List<K> _values;

    public OrderedEnumSet()
    {
        _values = new List<K>();
    }

    public OrderedEnumSet(IEnumerable<K> values)
    {
        _values = new List<K>();
        HashSet<K> seen = new();
        foreach (var value in values)
        {
            if (!seen.Add(value))
            {
                Debug.LogWarningFormat("EnumSet<{0}> contains duplicate key '{1}', ignoring.", typeof(K).Name, value);
                continue;
            }
            _values.Add(value);
        }
    }

    public static implicit operator List<K>(OrderedEnumSet<K> enumSet)
    {
        return new List<K>(enumSet._values);
    }

    public static implicit operator OrderedEnumSet<K>(List<K> list)
    {
        return new OrderedEnumSet<K>(list);
    }

    public bool Contains(K value) => _values.Contains(value);

    public void Add(K value)
    {
        if (_values.Contains(value))
        {
            Debug.LogWarningFormat("EnumSet<{0}> already contains '{1}', not adding.", typeof(K).Name, value);
            return;
        }
        _values.Add(value);
    }

    public void Remove(K value) => _values.Remove(value);

    public int Count => _values.Count; 
    
    public K Get(int index) => _values[index]; 
}
