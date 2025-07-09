using UnityEngine;
using UnityEngine.Assertions;

public class Staircase : MonoBehaviour
{
    private FloorManager _floorManager;
    private Player _player;

    private void Awake()
    {
        _player = FindAnyObjectByType<Player>();
        _floorManager = FindAnyObjectByType<FloorManager>();
        _floorManager.OnFloorChange += FloorChange;
    }

    private void FloorChange()
    {
        _floorManager.OnFloorChange -= FloorChange;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Assert.IsTrue(collision.gameObject == _player.gameObject, "Should only collide with player");

        _floorManager.NextFloor();
    }
}
