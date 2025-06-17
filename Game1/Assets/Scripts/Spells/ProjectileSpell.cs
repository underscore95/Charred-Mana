using UnityEngine;

public class ProjectileSpell : PlayerSpell
{
    [SerializeField] private GameObject _prefab; // projectile prefab, will be put into an object pool
    [SerializeField] private GameObject _targetParticlePrefab;
    private ProjectileManager _projManager;
    private Player _player;

    protected new void Awake()
    {
        base.Awake();

        _projManager = gameObject.AddComponent<ProjectileManager>();
        _projManager.Init(_prefab);
        foreach (var obj in _projManager.InternalPool().AliveAndDead)
        {
            obj.GetComponent<Projectile>().Shooter = _player;
        }

        _player = FindAnyObjectByType<Player>();
    }

    public override void OnTrigger(SpellTriggerInfo info)
    {
        GameObject proj = _projManager.SpawnProjectile(_player).gameObject;

        Vector3 pos = _player.transform.position - Vector3.forward;
        Vector3 mouseScreenPos = _player.Camera.ScreenToWorldPoint(info.MousePos);
        Vector2 toTarget = mouseScreenPos - pos;

        proj.transform.SetPositionAndRotation(pos, Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(toTarget.y, toTarget.x))); // right vector points towards position of mouse when triggered
       
    }

    public override void OnPreTrigger(SpellTriggerInfo info)
    {
        Vector3 mouseScreenPos = _player.Camera.ScreenToWorldPoint(info.MousePos);
        mouseScreenPos.z = 1;
        Instantiate(_targetParticlePrefab).transform.position = mouseScreenPos;
    }
}
