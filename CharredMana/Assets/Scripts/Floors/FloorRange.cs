
using System;
using UnityEngine;
using UnityEngine.Assertions;

// Represents a range of floors. Init must be called.
[Serializable]
public class FloorRange
{
    [SerializeField] private IntRange _floors;
    public IntRange Floors { get { return _floors; } }

    protected FloorManager _floorManager;

    public void Init()
    {
        _floorManager = MonoBehaviour.FindAnyObjectByType<FloorManager>();

        Assert.IsFalse(WasWrongInitCalled(), "You need to call Init(GameObject) on FloorActivityGuard");
    }

    private bool WasWrongInitCalled()
    {
        // check if we called Init on a FloorActivityGuard
        if (this is FloorActivityGuard g)
        {
            return !g.HasGameObject();
        }
        return false;
    }

    public bool IsInRange()
    {
        Assert.IsNotNull(_floorManager, "No floor manager found, did you forget to call Init()?");
        return  _floors.Contains(_floorManager.CurrentFloor);
    }

    public bool ExceededFinalFloor()
    {
        Assert.IsNotNull(_floorManager, "No floor manager found, did you forget to call Init()?");
        return _floorManager.CurrentFloor > _floors.End;
    }
}