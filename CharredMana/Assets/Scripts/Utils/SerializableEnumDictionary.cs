
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class SerializableEnumDictionary<K, V>
    where K : Enum
{
    [SerializeField] private List<Pair<K, V>> _values;

    // Convert to List<V>, requires that all enum values are consecutive
    public static implicit operator EnumDictionary<K, V>(SerializableEnumDictionary<K, V> dict)
    {
        EnumDictionary<K, V> result = new();

        HashSet<K> seenKeys = new();

        foreach (var pair in dict._values)
        {
            if (seenKeys.Contains(pair.First))
            {
                Debug.LogWarningFormat("SerializableEnumDictionary<{0}, {1}> has duplicate key '{2}', using the first occurrence.", typeof(K).Name, typeof(V).Name, pair.First);
                continue;
            }

            seenKeys.Add(pair.First);
            result[pair.First] = pair.Second;
        }

        foreach (K key in Enum.GetValues(typeof(K)))
        {
            if (!seenKeys.Contains(key))
            {
                Debug.LogErrorFormat("SerializableEnumDictionary<{0}, {1}> is missing value for enum key '{2}', using default.", typeof(K).Name, typeof(V).Name, key);
            }
        }

        return result;
    }
}