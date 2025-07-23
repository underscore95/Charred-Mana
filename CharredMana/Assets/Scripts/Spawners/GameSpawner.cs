using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class GameSpawner : BaseSpawner
{

    [SerializeField] private FloorActivityGuard _activity;
    [SerializeField] private int _waitTurnsBeforeFirstSpawnPerFloor = 0;
    [SerializeField] private bool _resetFirstSpawnWaitEveryFloor = true;
    [SerializeField] private IntRange _activeDuringFloorTurns = new(0, 999999);

    private FloorManager _floorManager;
    private int _turnsSinceEnterFloor = 0;
    public FloorActivityGuard FloorActivity { get { return _activity; } }
    public IntRange FloorTurnActivity { get { return _activeDuringFloorTurns; } }

    protected new void Awake()
    {
        base.Awake();

        OnFloorChange();

        _floorManager = FindAnyObjectByType<FloorManager>();
        _floorManager.OnFloorChange += OnFloorChange;
    }

    protected void OnDestroy()
    {
        _floorManager.OnFloorChange -= OnFloorChange;
    }

    protected new void OnEnable()
    {
        base.OnEnable();
        _activity.Init(gameObject);
        _turnManager.OnTurnChange += OnTurnChange;
    }

    protected new void OnDisable()
    {
        base.OnDisable();
        _turnManager.OnTurnChange -= OnTurnChange;
    }

    private void OnFloorChange()
    {
        if (_resetFirstSpawnWaitEveryFloor)
        {
            _turnsSinceSpawn = _spawnInterval - _waitTurnsBeforeFirstSpawnPerFloor; // wait turns before spawning again
        }
        _turnsSinceEnterFloor = 0;
    }

    private void OnTurnChange()
    {
        _turnsSinceEnterFloor++;
        _disable = !_activeDuringFloorTurns.Contains(_turnsSinceEnterFloor);
    }
}
