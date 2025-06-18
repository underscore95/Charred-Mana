using TMPro;
using UnityEngine;

public class Player : MonoBehaviour, ILivingEntity
{
    [SerializeField] private StatContainer _baseStats = new();
    [SerializeField] private TextMeshProUGUI _playerStatsText;

    public PlayerMana PlayerMana { get; private set; }
    public PlayerLevel PlayerLevel { get; private set; }
    public Camera Camera { get; private set; }
    public Vector3 PositionAtFrameStart { get; set; }
    public Stats Stats { get; private set; }

    private void Awake()
    {
        Camera = GetComponentInChildren<Camera>();
        PlayerMana = GetComponentInChildren<PlayerMana>();
        Stats = new(_baseStats);
        PlayerLevel = GetComponentInChildren<PlayerLevel>();
    }

    private void Update()
    {
        _playerStatsText.text = string.Format("DMG: {0:0.0}\nDEF: {1:0.0}\nMP RGN: {2:0.0}", Stats.Damage, Stats.Defense, Stats.ManaRegen);
    }

    public Stats GetStats()
    {
        return Stats;
    }

    void ILivingEntity.DamageReceiveEvent(float damage) { }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
