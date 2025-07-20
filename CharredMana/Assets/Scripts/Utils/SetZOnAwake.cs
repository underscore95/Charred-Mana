using UnityEngine;

public class SetZOnAwake : MonoBehaviour
{
    [SerializeField] private float _z = 0;

    private void Awake()
    {
        transform.position = new(transform.position.x, transform.position.y, _z);
    }
}
