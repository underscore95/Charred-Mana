using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private float _damage = 5;
    public float Damage { get { return _damage; } }
    private Rigidbody2D _rigidBody;
    private ManagedProjectile _managed;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _managed = GetComponent<ManagedProjectile>();
    }

    private void OnEnable()
    {
        TurnManager.Instance().OnTurnChange += TurnChange;
    }

    private void OnDisable()
    {
        TurnManager.Instance().OnTurnChange -= TurnChange;
    }

    private void TurnChange()
    {
        StartCoroutine(Utils.MoveRigidBody(_rigidBody, transform.right * _speed));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.OnDamage(Damage);
        }

        _managed.DestroyProjectile();
    }
}
