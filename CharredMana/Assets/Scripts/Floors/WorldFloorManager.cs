using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// Handles regenerating the world whenever the floor changes
public class WorldFloorManager : MonoBehaviour
{
    [SerializeField] private List<WorldGenSettings> _worldGenSettingsPerFloor = new();

    private World _world;
    private void Start()
    {
        Assert.IsTrue(_worldGenSettingsPerFloor.Count > 0);

        _world = GetComponent<World>();

        FloorManager floorManager = FindAnyObjectByType<FloorManager>();
        floorManager.OnFloorChange += () => UpdateWorld(floorManager.CurrentFloor);

        UpdateWorld(floorManager.CurrentFloor);
    }

    private void UpdateWorld(int newFloor)
    {
        if (newFloor < _worldGenSettingsPerFloor.Count)
        {
            _world.RegenerateWorld(_worldGenSettingsPerFloor[newFloor]);
        }
    }
}
