using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private SerializableStatModifiersContainer _statModifiers;
    public IHasStats Shooter;

    private Rigidbody2D _rigidBody;
    private ManagedProjectile _managed;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(_statModifiers);
    }

    private void Start()
    {
        Assert.IsNotNull(Shooter);
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
            Stats stats = Shooter.GetStats().DuplicateAndAddModifiers(_statModifiers);
            float dmg = damageable.GetStats().GetDamageWhenAttackedBy(stats);
            IDamageable.Damage(damageable, dmg);
        }

        _managed.DestroyProjectile();
    }
}
