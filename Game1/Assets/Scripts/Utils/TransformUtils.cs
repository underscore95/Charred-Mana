
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
}