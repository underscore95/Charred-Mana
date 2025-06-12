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

    protected new void Awake()
    {
        _unitName = "Mana";
        _maxValue = 100;
        _value = 100;
        base.Awake();
    }
}
