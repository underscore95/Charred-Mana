using UnityEngine;

public class ProjectileSpell : PlayerSpell
{
    [SerializeField] private GameObject _prefab; // projectile prefab, will be put into an object pool
    private ProjectileManager _projManager;
    private Player _player;

    protected new void Awake()
    {
        base.Awake();

        _projManager = gameObject.AddComponent<ProjectileManager>();
        _projManager.Init(_prefab);

        _player = FindAnyObjectByType<Player>();
    }

    public override void OnTrigger(SpellTriggerInfo info)
    {
        GameObject proj = _projManager.SpawnProjectile();

        Vector3 pos = _player.transform.position - Vector3.forward;
        Vector2 toTarget = _player.Camera.ScreenToWorldPoint(info.MousePos) - pos;

        proj.transform.SetPositionAndRotation(pos, Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(toTarget.y, toTarget.x)));
    }
}
