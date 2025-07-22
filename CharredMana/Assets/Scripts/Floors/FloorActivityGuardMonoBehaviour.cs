using NUnit.Framework;
using UnityEngine;

public class FloorActivityGuardMonoBehaviour : MonoBehaviour
{
    [SerializeField] private FloorActivityGuard _guard;

    protected void Awake()
    {
        _guard.Init(gameObject);
    }
}
