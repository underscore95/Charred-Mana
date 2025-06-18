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
        set
        {
            _maxValue = value;
            UpdateText();
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
        FindAnyObjectByType<TurnManager>().OnTurnChange += () => Mana = Mathf.Min(MaxMana, Mana + _player.Stats.ManaRegen);
    }
}
