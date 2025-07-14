using System;
using UnityEngine;

// Activate the object only between floors, destroy it once it will never be active again. Init must be called
[Serializable]
public class FloorActivityGuard : FloorRange
{
    private GameObject _gameObject;

    public void Init(GameObject gameObject)
    {
        _gameObject = gameObject;
        Init();
    }

    private new void Init()
    {
        base.Init();
        _floorManager.OnFloorChange += HandleFloorChange;
        HandleFloorChange();
    }

    private void HandleFloorChange()
    {
        _gameObject.SetActive(IsInRange());

        if (ExceededFinalFloor())
        {
            Cleanup();
        }
    }


    private void Cleanup()
    {
        _floorManager.OnFloorChange -= HandleFloorChange;

        MonoBehaviour.Destroy(_gameObject);
    }

    internal bool HasGameObject()
    {
        return _gameObject != null;
    }
}