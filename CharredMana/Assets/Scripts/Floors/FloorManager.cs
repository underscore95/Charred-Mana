using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class FloorManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _floorText;
    [SerializeField] private int _finalFloor = 1;

    public int TurnsSinceFloorChange { get; private set; } = 0;
    public int CurrentFloor { get; private set; } = 0;
    public UnityAction OnFloorChange { get; set; } = () => { };
    public int FinalFloor { get { return _finalFloor; } }

    private void Awake()
    {
        OnFloorChange += () => _floorText.text = "Floor " + CurrentFloor;

        FindAnyObjectByType<TurnManager>().OnTurnChange += () => TurnsSinceFloorChange++;
    }

    public void NextFloor()
    {
        Assert.IsTrue(_finalFloor > CurrentFloor, $"Cannot exceed _finalFloor ({_finalFloor})");
        CurrentFloor++;
        TurnsSinceFloorChange = 0;
        OnFloorChange.Invoke();
    }

    public bool IsHubFloor()
    {
        return CurrentFloor == 0;
    }
}
