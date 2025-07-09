using UnityEngine;

public class SetPositionOnFloorChange : MonoBehaviour
{
    [SerializeField] private Vector2 _location;

    private void Awake()
    {
        FindAnyObjectByType<FloorManager>().OnFloorChange += () =>
        {
            transform.position = new(_location.x, _location.y, transform.position.z);
        };
    }
}
