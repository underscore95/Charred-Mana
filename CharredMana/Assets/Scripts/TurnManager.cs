
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _turnText;
    [SerializeField] private List<DelayedStatBoost> _enemyStatBoosts = new();
    public StatModifiersContainer CurrentEnemyStatBoost { get; private set; } = new();
    public UnityAction OnTurnChange { get; set; } = () => { }; // Invoked by PlayerController
    public UnityAction OnLateTurnChange { get; set; } = () => { };
    public int CurrentTurn { get; private set; } = 0;

    private void Awake()
    {
        OnTurnChange += OnTurnChangeOrAwake;
        OnTurnChange += () => CurrentTurn++;
        OnTurnChange += Sfx.PlayPlayerMove;
        
        OnTurnChangeOrAwake();
    }

    private void OnTurnChangeOrAwake()
    {
        _turnText.text = "Turn #" + (CurrentTurn + 1);
        CurrentEnemyStatBoost = DelayedStatBoost.GetMergedStatBoost(_enemyStatBoosts, CurrentTurn);
    }
}