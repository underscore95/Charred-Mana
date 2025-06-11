using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    private Player _player;
    private IEnemyAttack _attack;
    private GameObject _spawner;
    private Rigidbody2D _rigidBody;

    private void Awake()
    {
        _player = FindAnyObjectByType<Player>();
        _attack = GetComponent<IEnemyAttack>();
        _spawner = transform.parent.gameObject;
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        TurnManager.Instance().OnTurnChange += PlayTurn;
    }

    private void OnDisable()
    {
        TurnManager.Instance().OnTurnChange -= PlayTurn;
    }

    private void PlayTurn()
    {
        Vector3 deltaPos = new(
            _player.PositionAtFrameStart.x - transform.position.x,
            _player.PositionAtFrameStart.y - transform.position.y,
            0.0f
        );

        foreach (Transform obj in _spawner.transform)
        {
            // boids
        }

        StartCoroutine(Utils.MoveRigidBody(_rigidBody, deltaPos.normalized));

        _attack.TryAttack();
    }

    public void OnDamage(float damage)
    {
        print("enemy damaged for " + damage);
    }
}
