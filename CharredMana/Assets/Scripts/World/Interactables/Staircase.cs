using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class Staircase : InteractableObject
{
    public static List<Staircase> StaircasesInWorld { get; private set; } = new();

    private FloorManager _floorManager;

    private ObjectPoolRef _poolRefOptional;

    protected new void Awake()
    {
        base.Awake();
        _floorManager = FindAnyObjectByType<FloorManager>();
        _floorManager.OnFloorChange += FloorChange;
    }

    protected void Start()
    {
        if (TryGetComponent<ObjectPoolRef>(out var r))
        {
            _poolRefOptional = r;
        }
    }

    private void OnEnable()
    {
        StaircasesInWorld.Add(this);
    }

    private void OnDisable()
    {
        StaircasesInWorld.Remove(this);
    }

    private void FloorChange()
    {
        _floorManager.OnFloorChange -= FloorChange;
        if (_poolRefOptional != null)
        {
            // this object is pooled
            Assert.IsNotNull(_poolRefOptional.Pool);
            _poolRefOptional.Pool.ReleaseObject(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected override void OnInteract()
    {
        _floorManager.NextFloor();
    }
}

