using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private StatContainer _baseStats = new();

    public PlayerMana PlayerMana { get; private set; }
    public Camera Camera { get; private set; }
    public Vector3 PositionAtFrameStart { get; set; }
    public Stats Stats { get; private set; }

    private void Awake()
    {
        Camera = GetComponentInChildren<Camera>();
        PlayerMana = GetComponentInChildren<PlayerMana>();
        Stats = new(_baseStats);
    }

    public Stats GetStats()
    {
        return Stats;
    }

    void IDamageable.DamageReceiveEvent(float damage) { }
}
