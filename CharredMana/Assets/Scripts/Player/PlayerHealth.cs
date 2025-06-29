using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

// Display health of parent object
public class PlayerHealth : PlayerValue
{
    private Stats _stats;
    private DebugMenu _debug;

    public float Health
    {
        get => _stats.CurrentHealth;
        set
        {
            _stats.SetCurrentHealthSilently(value);
            UpdateText();
        }
    }

    private new void Awake()
    {
        _debug = FindAnyObjectByType<DebugMenu>();
        _unitName = "Health";
        _value = 0;
        _maxValue = 0;
        base.Awake();

        FindAnyObjectByType<TurnManager>().OnTurnChange += () => Health = Mathf.Min(_stats.MaxHealth, Health + _stats.HealthRegen);
    }

    private void Start()
    {
        _stats = transform.parent.GetComponent<ILivingEntity>().GetStats();
    }

    private void Update()
    {
        _value = Mathf.CeilToInt(_stats.CurrentHealth - 0.001f);
        if (_value < 0 && _debug.Options.CanDie) _value = 0; // don't show negative health to the player
        _maxValue = Mathf.CeilToInt(_stats.Get(StatType.MaxHealth) - 0.001f);
        UpdateText();
    }
}
