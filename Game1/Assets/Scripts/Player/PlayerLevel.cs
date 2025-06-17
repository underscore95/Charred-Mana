using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class PlayerLevel : PlayerValue
{
    public UnityAction OnLevelUp { get; set; } = () => { };
    public int Level { get; private set; } = 1;

    public float Experience
    {
        get
        {
            return _value;
        }
        set
        {
            Assert.IsTrue(_hasStarted); // Some listeners might not be registered yet
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

    private bool _hasStarted = false;

    protected new void Awake()
    {
        base.Awake();
        CalculateExperienceToNextLevel();
        UpdateText();
    }

    private void Start()
    {
        _hasStarted = true;
      //  Experience = 200;
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
