
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public static class TransformUtils
{
    public static void GetChildrenWithComponent<T>(ref List<T> children, Transform parent, bool warnIfMissing = true)
        where T : Component
    {
        foreach (Transform t in parent)
        {
            if (t.gameObject.TryGetComponent<T>(out T comp))
            {
                children.Add(comp);
            }
            else
            {
                Debug.LogWarningFormat("{0} didn't have {1} component", t.gameObject.name, typeof(T).Name);
            }
        }
    }

    public static bool GetFirstComponentInChildren<T>(out T component, Transform parent, int recursions)
        where T : Component
    {
        if (recursions < 1 || parent.childCount < 1)
        {
            component = default;
            return false;
        }

        foreach (Transform child in parent)
        {
            if (child.gameObject.TryGetComponent<T>(out component)) return true;
        }

        return GetFirstComponentInChildren(out component, parent, recursions - 1);
    }

    public static void GetComponentsInChildren<T>(ref List<T> components, Transform parent, int recursions)
        where T : Component
    {
        if (recursions < 1 || parent.childCount < 1)
        {
            return;
        }

        foreach (Transform child in parent)
        {
            if (child.gameObject.TryGetComponent<T>(out T comp))
            {
                components.Add(comp);
            }
        }

        GetComponentsInChildren(ref components, parent, recursions - 1);
    }

    // Fill an enum dictionary from components on child objects
    // dict - The dictionary to fill
    // parent - Parent object
    // getEnumFunc - Function to convert from the unity component to enum
    // requireAllValues - Throw exception if there isn't a value for every enum
    // recursions - How deep to look in child objects? 1 is only direct children
    public static void FillEnumDictionaryFromChildren<K, V>(ref EnumDictionary<K, V> dict,
        Transform parent, Func<V, K> getEnumFunc,
        bool requireAllValues = true,
        int recursions = 1)
        where K : Enum
        where V : Component
    {
        List<V> comps = new();
        GetComponentsInChildren(ref comps, parent, recursions);

        foreach (V v in comps)
        {
            K k = getEnumFunc(v);
            Assert.IsFalse(dict.ContainsKey(k), $"{parent.gameObject.name} contains duplicate {typeof(K).Name} enum value {k} (recursions: {recursions})");
            dict.Set(k, v);
        }

        if (requireAllValues && !dict.IsFull())
        {
            Debug.LogError($"Failed to find all values of enum {typeof(K).Name} from child components of {parent.gameObject.name} (recursions: {recursions})");
        }
    }
}