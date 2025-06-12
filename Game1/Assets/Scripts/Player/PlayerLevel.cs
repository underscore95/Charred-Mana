using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class PlayerLevel : PlayerValue
{
    public UnityAction OnLevelUp { get; set; } = () => { };
    public float Level { get; private set; } = 1;

    public float Experience
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
            while (_value >= _maxValue)
            {
                _value -= _maxValue;
                Level++;
                CalculateExperienceToNextLevel();
                OnLevelUp.Invoke();
            }
            UpdateText();
        }
    }

    protected new void Awake()
    {
        base.Awake();
    }

    private void CalculateExperienceToNextLevel()
    {
        _maxValue = 100;
    }

    private new void UpdateText()
    {
        _unitName = "Level: " + Level + "\nExperience";
        base.UpdateText();
    }
}
