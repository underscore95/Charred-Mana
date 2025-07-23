using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnStatBoosts : MonoBehaviour
{
    [SerializeField] private List<DelayedStatBoost> _enemyStatBoosts = new();
    public StatModifiersContainer CurrentEnemyStatBoost { get; private set; } = new();
    public IReadOnlyList<DelayedStatBoost> AllEnemyStatBoosts { get { return _enemyStatBoosts; } }
    private TurnManager _turnManager;

    private void Awake()
    {
        _turnManager = FindAnyObjectByType<TurnManager>();

        _turnManager.OnPreTurnChange += HandleTurnChange;
    }

    private void OnDestroy()
    {
        _turnManager.OnPreTurnChange -= HandleTurnChange;
    }

    private void HandleTurnChange()
    {
        CurrentEnemyStatBoost = DelayedStatBoost.GetMergedStatBoost(_enemyStatBoosts, _turnManager.CurrentTurn);
    }
}
