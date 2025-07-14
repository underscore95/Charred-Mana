using NUnit.Framework;
using UnityEngine;

public class FloorActivityGuardMonoBehaviour : MonoBehaviour
{
    [SerializeField] private FloorActivityGuard _guard;

    private void Awake()
    {
        _guard.Init(gameObject);
    }
}
