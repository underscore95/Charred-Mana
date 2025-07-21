using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class Textbox : MonoBehaviour
{
    [SerializeField] private float _interactUiFloatDuration = 0.75f;
    [SerializeField] private Vector3 _interactUiOffset = Vector3.up + Vector3.back;
    [SerializeField] private FloatRange _interactUiYOffsetRange = new(-0.05f, 0.05f);

    public string Text
    {
        get
        {
            return _text.text;
        }
        set
        {
            _text.text = value;
        }
    }

    private float _secondsVisible = 0;
    private bool _invertT = false;
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();

        _interactUiYOffsetRange.AssertValid();
        Assert.IsTrue(_interactUiFloatDuration > 0);
    }

    private void Update()
    {
        // Floating animation
        _secondsVisible += Time.deltaTime;
        if (_secondsVisible > _interactUiFloatDuration)
        {
            _secondsVisible -= _interactUiFloatDuration;
            _invertT = !_invertT;
        }
        float t = Mathf.InverseLerp(0, _interactUiFloatDuration, _secondsVisible);
        if (_invertT) t = 1 - t;
        transform.localPosition = _interactUiOffset + _interactUiYOffsetRange.Lerp(t) * Vector3.up;
    }
}
