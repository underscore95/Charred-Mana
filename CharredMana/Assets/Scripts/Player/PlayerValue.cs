using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

// Display value, e.g. health or mana
public abstract class PlayerValue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    protected string _unitName = "no-unit-name"; // e.g. Mana

    protected float _maxValue;
    protected float _value;

    protected void Awake()
    {
        UpdateText();
    }

    public void UpdateText()
    { 
        _text.text = string.Format("{2}: {0} / {1}", Mathf.CeilToInt(_value - 0.001f), Mathf.CeilToInt(_maxValue - 0.001f), _unitName);
    }
}
