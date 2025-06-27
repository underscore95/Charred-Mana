
using UnityEngine;
using UnityEngine.Assertions;

// Boids Tutorial: https://youtu.be/Srg6GRFspps
public class BoidsEnemyController : MonoBehaviour, IEnemyController
{
    [SerializeField] private float _enemyNearbyRadius = 0.1f; // enemies within this distance are considered nearby
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private float _boidsWeight = 0.5f; // toPlayer + boidsWeight * boids
    [SerializeField] private float _cohesionThreshold = 1.0f; // cohesion force only matters if distance to center is greater than this
    private Enemy _enemy;
    private Rigidbody2D _rigidBody;
    private Vector2 _deltaPosLastTurn;

    private void Awake()
    {
        _enemy = GetComponentInParent<Enemy>();
        _rigidBody = GetComponent<Rigidbody2D>();
        Assert.IsTrue(_boidsWeight >= 0 && _boidsWeight <= 1, "_boidsStrength must be in range 0 to 1");
    }

    private void OnEnable()
    {
        _deltaPosLastTurn = Vector2.zero;
    }

    public void HandleMovement()
    {
        Vector2 deltaPos = Vector2.zero;
        Vector2 neighbourDeltaPosSum = Vector2.zero;
        Vector2 neighbourAveragePos = Vector2.zero;

        Collider2D[] nearbyEnemies = Enemy.GetNearbyEnemies(transform.position, _enemyNearbyRadius);
        foreach (Collider2D enemy in nearbyEnemies)
        {
            // Move away from nearby enemies
            deltaPos += CalculateSeperationForce(enemy);

            // Alignment (move in same direction)
            Vector2 enemyDeltaPos = enemy.GetComponent<Enemy>().Controller.GetDeltaPosLastTurn();
            if (enemyDeltaPos != Vector2.zero) neighbourDeltaPosSum += enemyDeltaPos.normalized;

            // Cohesion (move to center of group)
            neighbourAveragePos += (Vector2)enemy.transform.position;
        }

        // Alignment
        if (neighbourDeltaPosSum != Vector2.zero)
        {
            deltaPos += (neighbourDeltaPosSum / nearbyEnemies.Length).normalized;
        }

        // Cohesion 
        if (neighbourAveragePos != Vector2.zero)
        {
            neighbourAveragePos /= nearbyEnemies.Length;
            Vector2 toCenter = (neighbourAveragePos - (Vector2)transform.position);
            if (toCenter.sqrMagnitude > _cohesionThreshold * _cohesionThreshold)
            {
                deltaPos += toCenter.normalized;
            }
        }

        // Move to target
        deltaPos = ((Vector2)_enemy.CurrentTarget.GetGameObject().transform.position - (Vector2)transform.position).normalized + deltaPos.normalized * _boidsWeight;

        _deltaPosLastTurn = transform.position;
        StartCoroutine(Utils.MoveRigidBody(_rigidBody, deltaPos.normalized * _speed));
    }

    public Vector2 GetDeltaPosLastTurn()
    {
        return _deltaPosLastTurn;
    }

    private Vector2 CalculateSeperationForce(Collider2D enemy)
    {
        Vector2 toNeighbour = enemy.transform.position - transform.position;
        float distance = toNeighbour.magnitude;
        if (distance > 0)
        {
            return -(toNeighbour.normalized) / distance;
        }
        return Vector2.zero;
    }
}