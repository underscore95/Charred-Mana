using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Player :  LivingEntity
{
    [SerializeField] private StatContainer _baseStats = new();
    [SerializeField] private TextMeshProUGUI _playerStatsText;

    public PlayerMana PlayerMana { get; private set; }
    public PlayerLevel PlayerLevel { get; private set; }
    public Camera Camera { get; private set; }
    public Vector3 PositionAtFrameStart { get; set; }
    public int MonstersKilled { get; set; } = 0;
    public int SpellsCast { get; set; } = 0;
    private DebugMenu _debugMenu;

    private void Awake()
    {
        Camera = GetComponentInChildren<Camera>();
        PlayerMana = GetComponentInChildren<PlayerMana>();
        EntityStats = new(_baseStats);
        PlayerLevel = GetComponentInChildren<PlayerLevel>();
        OnDamaged() += _ => Sfx.PlayPlayerDamaged();
        _debugMenu=FindAnyObjectByType<DebugMenu>();
    }

    private void Update()
    {
        _playerStatsText.text = string.Format("DMG: {0:0.0}\nDEF: {1:0.0}\nMP RGN: {2:0.0}", EntityStats.Damage, EntityStats.Defense, EntityStats.ManaRegen);
    }

    public bool IsDead()
    {
        return EntityStats.CurrentHealth <= 0 && _debugMenu.Options.CanDie;
    }
}
