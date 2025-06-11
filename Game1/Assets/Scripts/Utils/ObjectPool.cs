using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ObjectPool
{
    private readonly GameObject[] _values;
    private readonly Stack<int> _freeIndices = new();
    private readonly Dictionary<GameObject, int> _activeIndices = new();

    public int Capacity => _values.Length;
    public int CurrentSize => _activeIndices.Count;

    public ObjectPool(GameObject prefab, int capacity, Transform parent = null)
    {
        _values = new GameObject[capacity];

        for (int i = 0; i < capacity; i++)
        {
            var obj = parent == null ? UnityEngine.Object.Instantiate(prefab) : UnityEngine.Object.Instantiate(prefab, parent);
            obj.SetActive(false);
            _values[i] = obj;
            _freeIndices.Push(i);
        }
    }

    public bool IsFull() => _freeIndices.Count == 0;

    public GameObject ActivateObject()
    {
        Assert.IsTrue(!IsFull(), "Tried to allocate from a full pool.");

        int index = _freeIndices.Pop();
        var obj = _values[index];

        Assert.IsFalse(_activeIndices.ContainsKey(obj), "Object is already active.");

        obj.SetActive(true);
        _activeIndices[obj] = index;

        return obj;
    }

    public void ReleaseObject(GameObject obj)
    {
        if (!_activeIndices.TryGetValue(obj, out int index))
        {
            Debug.LogError($"{obj.name} is not currently active in the pool.");
            return;
        }

        obj.SetActive(false);
        _activeIndices.Remove(obj);
        _freeIndices.Push(index);
    }

    public void ReleaseOldestObject()
    {
        foreach (var kvp in _activeIndices)
        {
            ReleaseObject(kvp.Key);
            break;
        }
    }

    public IEnumerable<GameObject> Alive
    {
        get
        {
            foreach (var obj in _activeIndices.Keys)
                yield return obj;
        }
    }

    public IEnumerable<GameObject> AliveAndDead
    {
        get
        {
            foreach (var obj in _values)
                yield return obj;
        }
    }
}
