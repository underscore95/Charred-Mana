using UnityEngine;

public class WaitForFloors : CustomYieldInstruction
{
    internal static FloorManager _floorManager;

    private int _floorsRemaining;

    public WaitForFloors(int floors)
    {
        if (_floorManager == null) _floorManager = MonoBehaviour.FindAnyObjectByType<FloorManager>();
        _floorsRemaining = floors;
        _floorManager.OnFloorChange += OnFloorChange;
    }

    private void OnFloorChange()
    {
        _floorsRemaining--;
        if (_floorsRemaining <= 0)
        {
            _floorManager.OnFloorChange -= OnFloorChange;
        }
    }

    public override bool keepWaiting
    {
        get
        {
            return _floorsRemaining > 0;
        }
    }
}