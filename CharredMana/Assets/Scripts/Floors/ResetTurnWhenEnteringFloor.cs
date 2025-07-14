using UnityEngine;

public class ResetTurnWhenEnteringFloor : MonoBehaviour
{
    [SerializeField] private int _floor = 1;

    private FloorManager _floorManager;
    private TurnManager _turnManager;

    private void Awake()
    {
        _floorManager = FindAnyObjectByType<FloorManager>();
        _turnManager = FindAnyObjectByType<TurnManager>();

        _floorManager.OnFloorChange += HandleFloorChange;
    }

    private void OnDestroy()
    {
        _floorManager.OnFloorChange -= HandleFloorChange;
    }

    private void HandleFloorChange()
    {
        if (_floorManager.CurrentFloor == _floor)
        {
            _turnManager.ResetTurn();
            Destroy(gameObject);
        }
    }
}
