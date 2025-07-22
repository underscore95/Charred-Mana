using UnityEngine;

public class HealPlayerOnFloorChange : MonoBehaviour
{
    private FloorManager _floorManager;
    private Player _player;


    private void Awake()
    {
        _floorManager = FindAnyObjectByType<FloorManager>();
        _player = FindAnyObjectByType<Player>();
    }

    private void OnEnable()
    {
        _floorManager.OnFloorChange += HandleFloorChange;
    }

    private void OnDisable()
    {
        _floorManager.OnFloorChange -= HandleFloorChange;
    }

    private void HandleFloorChange()
    {
        _player.EntityStats.Heal();
    }
}
