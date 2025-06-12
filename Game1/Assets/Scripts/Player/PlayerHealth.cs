using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

// Display health of parent object
public class PlayerHealth : PlayerValue
{
    private Stats _stats;

    public float Health
    {
        get => _stats.CurrentHealth;
        set
        {
            _stats.CurrentHealth = value;
            UpdateText();
        }
    }

    private new void Awake()
    {
        _unitName = "Health";
        _value = 0;
        _maxValue = 0;
        base.Awake();
    }

    private void Start()
    {
        _stats = transform.parent.GetComponent<IHasStats>().GetStats();
    }

    private void Update()
    {
        _value = Mathf.CeilToInt(_stats.CurrentHealth);
        _maxValue = Mathf.CeilToInt(_stats.Get(StatType.MaxHealth));
        UpdateText();
    }
}
