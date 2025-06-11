
using UnityEngine.Events;

public  class TurnManager
{
    private static readonly TurnManager _instance = new();

    public static TurnManager Instance() {  return _instance; }

    public UnityAction OnTurnChange;

    public int CurrentTurn { get; private set; } = 0;

    public TurnManager() {
        OnTurnChange += () => CurrentTurn++;
    }    
}