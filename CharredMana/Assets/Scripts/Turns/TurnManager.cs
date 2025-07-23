
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _turnText;
    public UnityAction OnPreTurnChange { get; set; } = () => { };
    public UnityAction OnTurnChange { get; set; } = () => { };
    public UnityAction OnLateTurnChange { get; set; } = () => { };
    public UnityAction OnTurnReset { get; set;  } = () => { };
    public UnityAction OnTurnChangeOrReset { get; set;  } = () => { };
    public int CurrentTurn { get; private set; } = 0;
    public int FloorEnterTurn { get; private set; } = 0; // Turn when we entered the current floor

    private FloorManager _floorManager;

    private void Awake()
    {
        _floorManager = FindAnyObjectByType<FloorManager>();

        OnTurnChangeOrReset += UpdateTurn;

        UpdateTurn();
    }

    private void Start()
    {
        _floorManager.OnFloorChange += () => FloorEnterTurn = CurrentTurn;
    }

    private void UpdateTurn()
    {
        _turnText.text = "Turn #" + (CurrentTurn + 1);
    }

    public void NextTurn()
    {
        CurrentTurn++;
        OnPreTurnChange.Invoke();
        OnTurnChangeOrReset.Invoke();
        OnTurnChange.Invoke();
        OnLateTurnChange.Invoke();
    }

    public void ResetTurn()
    {
        CurrentTurn = 0;
        OnTurnReset.Invoke();
        OnTurnChangeOrReset.Invoke();
    }
}