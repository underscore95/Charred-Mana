
using System.Collections.Generic;
using UnityEngine;

public static class TransformUtils
{
    public static void GetChildrenWithComponent<T>(ref List<T> children, Transform parent, bool warnIfMissing = true) where T : Component
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
}