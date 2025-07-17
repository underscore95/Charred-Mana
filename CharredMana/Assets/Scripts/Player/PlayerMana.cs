using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerMana : PlayerValue
{
    public float MaxMana
    {
        get
        {
            return _maxValue;
        }
    }

    public float Mana
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
            if (_value > _maxValue) _value = _maxValue;
            UpdateText();
        }
    }

    private Player _player;

    protected new void Awake()
    {
        _unitName = "Mana";
        _maxValue = 100;
        _value = 100;
        base.Awake();

        _player = FindAnyObjectByType<Player>();
        FindAnyObjectByType<TurnManager>().OnTurnChange += () => Mana = Mathf.Min(MaxMana, Mana + _player.EntityStats.ManaRegen);
    }

    private void Update()
    {
        _maxValue = _player.EntityStats.MaxMana;
    }
}
