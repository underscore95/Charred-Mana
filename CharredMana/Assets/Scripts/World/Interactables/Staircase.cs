using UnityEngine;
using UnityEngine.Assertions;

public class Staircase : MonoBehaviour
{
    private FloorManager _floorManager;
    private Player _player;

    private ObjectPoolRef _poolRefOptional;

    private void Awake()
    {
        _player = FindAnyObjectByType<Player>();
        _floorManager = FindAnyObjectByType<FloorManager>();
        _floorManager.OnFloorChange += FloorChange;
    }

    private void Start()
    {
        if (TryGetComponent<ObjectPoolRef>(out var r))
        {
            _poolRefOptional = r;
        }
    }

    private void FloorChange()
    {
        _floorManager.OnFloorChange -= FloorChange;
        if (_poolRefOptional != null)
        {
            // this object is pooled
            Assert.IsNotNull(_poolRefOptional.Pool);
            _poolRefOptional.Pool.ReleaseObject(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Assert.IsTrue(collision.gameObject == _player.gameObject, "Should only collide with player");

        _floorManager.NextFloor();
    }
}

