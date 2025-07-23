using UnityEngine;

public class ContentCompletionTracker : MonoBehaviour
{
    [SerializeField] private string _completedFinalTurnAndFloorMessage = "Congratulations! You have reached the final floor and turn.\nThere is no more content after this point.";
    [SerializeField] private int _turnBuffer = 50; // turns after the final thing activates

    private int _finalTurn = 0;
    private int _finalFloorTurn = 0;

    private PlayerMessageManager _messageManager;
    private FloorManager _floorManager;
    private TurnManager _turnManager;
    private EnemyTurnStatBoosts _enemyTurnStatBoosts;
    private bool _reachedFinalTurnAndFloor = false;

    private void Awake()
    {
        _messageManager = FindAnyObjectByType<PlayerMessageManager>();
        _floorManager = FindAnyObjectByType<FloorManager>();
        _turnManager = FindAnyObjectByType<TurnManager>();
        _enemyTurnStatBoosts = FindAnyObjectByType<EnemyTurnStatBoosts>();

        CalculateFinalTurn();
        _turnManager.OnTurnChange += HandleTurnChange;
    }

    // what turn does the final spawner activate on the final floor / final stat boost activate
    private void CalculateFinalTurn()
    {
        GameSpawner[] spawners = FindObjectsByType<GameSpawner>(FindObjectsSortMode.None);

        foreach (GameSpawner spawner in spawners)
        {
            if (!spawner.FloorActivity.Floors.Contains(_floorManager.FinalFloor)) continue; // spawner isn't activate on final floor
            _finalFloorTurn = Mathf.Max(_finalTurn, spawner.FloorTurnActivity.Begin);
        }

        foreach (DelayedStatBoost statBoost in _enemyTurnStatBoosts.AllEnemyStatBoosts)
        {
            _finalTurn = Mathf.Max(_finalTurn, statBoost.RequiredTurn);
        }

        _finalTurn += _turnBuffer;
        _finalFloorTurn += _turnBuffer;
    }

    private void OnDestroy()
    {
        _turnManager.OnTurnChange -= HandleTurnChange;
    }

    private void HandleTurnChange()
    {
        if (_reachedFinalTurnAndFloor) return;
        if (_floorManager.CurrentFloor < _floorManager.FinalFloor) return;

        if (_floorManager.TurnsSinceFloorChange >= _finalFloorTurn && _turnManager.CurrentTurn >= _finalTurn)
        {
            _messageManager.SendPlayerMessage(_completedFinalTurnAndFloorMessage);
            _reachedFinalTurnAndFloor = true;
        }
    }
}
