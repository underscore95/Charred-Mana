using UnityEngine;

public class EnemyAttack : MonoBehaviour, IEnemyAttack
{
    [SerializeField] private float _attackRange = Mathf.Sqrt(1.1f);
    [SerializeField] private GameObject _swipeAnimationPrefab;
    private Player _player;

    private void Awake()
    {
        _player = FindAnyObjectByType<Player>();
    }

    public void TryAttack()
    {
        if (Vector3.Distance(transform.position, _player.PositionAtFrameStart) <= _attackRange)
        {
            Vector3 toPlayer = _player.transform.position - transform.position;
            toPlayer.z = 0;
            toPlayer = toPlayer.normalized;

            Transform swipe = Instantiate(_swipeAnimationPrefab).transform;
            swipe.position = transform.position + toPlayer * 0.5f;
            float dotProduct = Vector3.Dot(toPlayer, Vector3.left); // both of these are normalised, so we have cos(a) here
            swipe.rotation = Quaternion.Euler(0, 0, Mathf.Acos(dotProduct) * Mathf.Rad2Deg);

            _player.GetComponentInChildren<PlayerHealth>().Damage(Random.Range(3, 9));
        }
    }
}
