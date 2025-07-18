
using System;
using System.Collections.Generic;
using UnityEngine;

// Dictionary where keys are enums, all values in the enum must be consecutive, stored in a list rather than using hashing
[Serializable]
public class EnumDictionary<K, V> : IEnumerable<KeyValuePair<K, V>> where K : Enum
{
    [SerializeField] private List<V> _values;
    [SerializeField] private List<bool> _hasValues;

    public EnumDictionary()
    {
        var values = (K[])Enum.GetValues(typeof(K));
        int expected = 0;

        foreach (var val in values)
        {
            int actual = Convert.ToInt32(val);
            if (actual != expected++)
                throw new ArgumentException($"Enum '{typeof(K).Name}' must have consecutive values starting from 0.");
        }

        int size = values.Length;
        _values = new(size);
        _hasValues = new(size);

        for (int i = 0; i < size; i++)
        {
            _values.Add(default);
            _hasValues.Add(false);
        }
    }

    private int Index(K key) => Convert.ToInt32(key);

    public V this[K key]
    {
        get
        {
            int idx = Index(key);
            if (!_hasValues[idx])
                throw new KeyNotFoundException($"Key '{key}' not found.");
            return _values[idx];
        }
        set
        {
            int idx = Index(key);
            _values[idx] = value;
            _hasValues[idx] = true;
        }
    }

    public bool ContainsKey(K key)
    {
        return _hasValues[Index(key)];
    }

    public bool TryGetValue(K key, out V value)
    {
        int idx = Index(key);
        if (_hasValues[idx])
        {
            value = _values[idx];
            return true;
        }
        value = default;
        return false;
    }

    public void Set(K key, V value)
    {
        int idx = Index(key);
        if (_hasValues[idx])
            throw new ArgumentException($"An element with the same key '{key}' already exists.");
        _values[idx] = value;
        _hasValues[idx] = true;
    }

    public bool Remove(K key)
    {
        int idx = Index(key);
        if (_hasValues[idx])
        {
            _values[idx] = default;
            _hasValues[idx] = false;
            return true;
        }
        return false;
    }

    public void Clear()
    {
        for (int i = 0; i < _hasValues.Count; i++)
        {
            _values[i] = default;
            _hasValues[i] = false;
        }
    }

    public bool IsFull()
    {
        return _capacity <= Count;
    }

    private int _capacity = Enum.GetValues(typeof(K)).Length;
    public int Capacity { get { return _capacity; } }

    public int Count
    {
        get
        {
            int count = 0;
            for (int i = 0; i < _hasValues.Count; i++)
                if (_hasValues[i])
                    count++;
            return count;
        }
    }

    public IEnumerable<K> Keys
    {
        get
        {
            foreach (K key in Enum.GetValues(typeof(K)))
                if (_hasValues[Index(key)])
                    yield return key;
        }
    }

    public IEnumerable<V> Values
    {
        get
        {
            for (int i = 0; i < _hasValues.Count; i++)
                if (_hasValues[i])
                    yield return _values[i];
        }
    }


    public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
    {
        foreach (K key in Enum.GetValues(typeof(K)))
        {
            int idx = Index(key);
            if (_hasValues[idx])
                yield return new KeyValuePair<K, V>(key, _values[idx]);
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
