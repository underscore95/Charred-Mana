
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _turnText;
    [SerializeField] private List<DelayedStatBoost> _enemyStatBoosts = new();
    public StatModifiersContainer CurrentEnemyStatBoost { get; private set; } = new();
    public UnityAction OnTurnChange { get; set; } = () => { };
    public UnityAction OnLateTurnChange { get; set; } = () => { };
    public int CurrentTurn { get; private set; } = 0;
    public int FloorEnterTurn { get; private set; } = 0; // Turn when we entered the current floor

    private FloorManager _floorManager;

    private void Awake()
    {
        _floorManager = FindAnyObjectByType<FloorManager>();

        OnTurnChange += OnTurnChangeOrAwake;
        OnTurnChange += () => CurrentTurn++;
        OnTurnChange += Sfx.PlayPlayerMove;

        OnTurnChangeOrAwake();
    }

    private void Start()
    {
        _floorManager.OnFloorChange += () => FloorEnterTurn = CurrentTurn;
    }

    private void OnTurnChangeOrAwake()
    {
        _turnText.text = "Turn #" + (CurrentTurn + 1);
        CurrentEnemyStatBoost = DelayedStatBoost.GetMergedStatBoost(_enemyStatBoosts, CurrentTurn);
    }

    public void NextTurn()
    {
        OnTurnChange.Invoke();
        OnLateTurnChange.Invoke();
    }

    public void ResetTurn()
    {
        CurrentTurn = 0;
        NextTurn();
    }
}