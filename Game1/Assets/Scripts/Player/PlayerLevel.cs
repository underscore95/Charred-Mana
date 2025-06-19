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
    }

    private void CalculateExperienceToNextLevel()
    {
        _maxValue = Level switch
        {
            1 => 25,
            2 => 40,
            3 => 50,
            4 => 70,
            5 => 90,
            _ => (float)100,
        };
    }

    private new void UpdateText()
    {
        _unitName = "Level: " + Level + "\nExperience";
        base.UpdateText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
           Experience = _maxValue;
        }
    }
}
