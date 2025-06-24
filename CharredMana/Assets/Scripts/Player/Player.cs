using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, ILivingEntity
{
    [SerializeField] private StatContainer _baseStats = new();
    [SerializeField] private TextMeshProUGUI _playerStatsText;

    public PlayerMana PlayerMana { get; private set; }
    public PlayerLevel PlayerLevel { get; private set; }
    public Camera Camera { get; private set; }
    public Vector3 PositionAtFrameStart { get; set; }
    public Stats Stats { get; private set; }
    private UnityAction<float> _onDamaged = (dmg) => { };
    private DebugMenu _debugMenu;

    private void Awake()
    {
        Camera = GetComponentInChildren<Camera>();
        PlayerMana = GetComponentInChildren<PlayerMana>();
        Stats = new(_baseStats);
        PlayerLevel = GetComponentInChildren<PlayerLevel>();
        _onDamaged += _ => Sfx.PlayPlayerDamaged();
        _debugMenu=FindAnyObjectByType<DebugMenu>();
    }

    private void Update()
    {
        _playerStatsText.text = string.Format("DMG: {0:0.0}\nDEF: {1:0.0}\nMP RGN: {2:0.0}", Stats.Damage, Stats.Defense, Stats.ManaRegen);
    }

    public Stats GetStats()
    {
        return Stats;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public ref UnityAction<float> OnDamaged()
    {
        return ref _onDamaged;
    }

    public bool IsDead()
    {
        return Stats.CurrentHealth <= 0 && _debugMenu.Options.CanDie;
    }
}
