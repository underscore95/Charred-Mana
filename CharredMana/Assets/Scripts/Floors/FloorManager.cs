using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class FloorManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _floorText;

    public int CurrentFloor { get; private set; } = 0;
    public UnityAction OnFloorChange { get; set; } = () => { };

    private void Awake()
    {
        OnFloorChange += () => _floorText.text = "Floor " + CurrentFloor;
    }

    public void NextFloor()
    {
        CurrentFloor++;
        OnFloorChange.Invoke();
    }

    public bool IsHubFloor()
    {
        return CurrentFloor == 0;
    }
}
