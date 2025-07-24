using UnityEngine;

public class EnemyAttack : MonoBehaviour, IEnemyAttack
{
    [SerializeField] private float _attackRange = Mathf.Sqrt(1.1f);
    private Player _player;
    private ParticleManager _particleManager;

    private void Awake()
    {
        _player = FindAnyObjectByType<Player>();
        _particleManager = FindAnyObjectByType<ParticleManager>();
    }

    public void HandleAttack()
    {
        if (Vector3.Distance(transform.position, _player.PositionAtFrameStart) <= _attackRange)
        {
            Vector3 toPlayer = _player.transform.position - transform.position;
            toPlayer.z = 0;
            toPlayer = toPlayer.normalized;

            float dotProduct = Vector3.Dot(toPlayer, Vector3.left); // both of these are normalised, so we have cos(a) here
            _particleManager.SpawnParticle(
                ParticleType.Swipe,
                transform.position + toPlayer * 0.5f + Vector3.back,
                Quaternion.Euler(0, 0, Mathf.Acos(dotProduct) * Mathf.Rad2Deg)
                );

            LivingEntity.Damage(_player, Random.Range(3.0f, 9.0f), DamageSource.EnemyMelee);
        }
    }
}
