using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class FloorManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _floorText;

    public int Floor { get; private set; } = 0;
    public UnityAction OnFloorChange { get; set; } = () => { };

    private void Awake()
    {
        OnFloorChange += () => _floorText.text = "Floor " + Floor;
    }

    public void NextFloor()
    {
        Floor++;
        OnFloorChange.Invoke();
    }

    public bool IsHubFloor()
    {
        return Floor == 0;
    }
}
