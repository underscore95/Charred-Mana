using System;
using System.Collections.Generic;
using UnityEngine;
using static FloorObjectManager;

// Enable objects on specific floors and destroys them after
public class FloorObjectManager : MonoBehaviour
{
    [Serializable]
    public struct FloorObjects
    {
        public int Floor;
        public GameObject[] Objects;
    }

    [SerializeField] private List<FloorObjects> _objects;
    private FloorManager _floorManager;

    private void Awake()
    {
        _floorManager = FindAnyObjectByType<FloorManager>();

        HashSet<int> floors = new();
        foreach (FloorObjects floorObjects in _objects)
        {
            if (!floors.Add(floorObjects.Floor))
            {
                Debug.LogError($"FloorObjectManager contains objects for floor {floorObjects.Floor} multiple times");
                continue;
            }

            if (floorObjects.Floor < 0)
            {
                Debug.LogError($"FloorObjectManager contains objects for floor {floorObjects.Floor} (negative?)");
                continue;
            }

            _floorManager.OnFloorChange += HandleFloorChange;
            HandleFloorChange();
        }
    }

    private void HandleFloorChange()
    {
        foreach (FloorObjects floorObjects in _objects)
        {
            foreach (GameObject obj in floorObjects.Objects)
            {
                obj.SetActive(floorObjects.Floor == _floorManager.CurrentFloor);

                if (floorObjects.Floor < _floorManager.CurrentFloor)
                {
                    Destroy(obj);
                }
            }
        }

        _objects.RemoveAll(floorObjects => floorObjects.Floor < _floorManager.CurrentFloor);
    }
}
