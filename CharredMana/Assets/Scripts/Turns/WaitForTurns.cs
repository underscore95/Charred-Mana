
using UnityEngine;
using UnityEngine.Assertions;

public class WaitForTurns : CustomYieldInstruction
{
    internal static TurnManager _turnManager;

    private int _turnsRemaining;

    public WaitForTurns(int turns)
    {
        if (_turnManager == null) _turnManager = MonoBehaviour.FindAnyObjectByType<TurnManager>();
        _turnsRemaining = turns;
        _turnManager.OnTurnChange += OnTurnChange;
    }

    private void OnTurnChange()
    {
        _turnsRemaining--;
        if (_turnsRemaining <= 0)
        {
            _turnManager.OnTurnChange -= OnTurnChange;
        }
    }

    public override bool keepWaiting
    {
        get
        {
            return _turnsRemaining > 0;
        }
    }
}