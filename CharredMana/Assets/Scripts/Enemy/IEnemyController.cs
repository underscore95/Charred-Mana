
using UnityEngine;

public interface IEnemyController
{
    // Displacement of the enemy last turn, may be 0, 0
    public Vector2 GetDeltaPosLastTurn();
    public void HandleMovement();
}