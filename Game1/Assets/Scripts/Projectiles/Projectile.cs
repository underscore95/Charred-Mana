using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private SerializableStatModifiersContainer _statModifiers;
    public ILivingEntity Shooter;
    public List<SerializableEffect> Effects = new(); // applies when it hits an entity, before the damage

    private Rigidbody2D _rigidBody;
    private ManagedProjectile _managed;
    private TurnManager _turnManager;
    private EffectManager _effectManager;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(_statModifiers);
        _turnManager = FindAnyObjectByType<TurnManager>();
        _effectManager = FindAnyObjectByType<EffectManager>();
    }

    private void Start()
    {
        Assert.IsNotNull(Shooter);
        _managed = GetComponent<ManagedProjectile>();
    }

    private void OnEnable()
    {
        _turnManager.OnTurnChange += TurnChange;
    }

    private void OnDisable()
    {
        _turnManager.OnTurnChange -= TurnChange;
    }

    private void TurnChange()
    {
        StartCoroutine(Utils.MoveRigidBody(_rigidBody, transform.right * _speed));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<ILivingEntity>(out var entity))
        {
            Stats stats = Shooter.GetStats().DuplicateAndAddModifiers(_statModifiers);
            float dmg = entity.GetStats().GetDamageWhenAttackedBy(stats);
            _effectManager.ApplyEffects(entity, Effects);
            ILivingEntity.Damage(entity, dmg);
        }

        _managed.ReleaseProjectile();
    }
}
