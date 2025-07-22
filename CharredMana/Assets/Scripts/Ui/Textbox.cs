using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class Textbox : MonoBehaviour
{
    private static Vector2 TEXT_MARGIN = Vector2.one * 15;

    [SerializeField] private float _interactUiFloatDuration = 0.75f;
    [SerializeField] private Vector3 _interactUiOffset = Vector3.up + Vector3.back;
    [SerializeField] private FloatRange _interactUiYOffsetRange = new(-0.05f, 0.05f);
    public bool FloatingAnimationEnabled { get; set; } = true;

    public string Text
    {
        get
        {
            return _text.text;
        }
    }

    private float _secondsVisible = 0;
    private bool _invertT = false;
    private TextMeshProUGUI _text;
    public RectTransform Rect { get; private set; }

    private void Awake()
    {
        Rect = GetComponent<RectTransform>();
        _text = GetComponentInChildren<TextMeshProUGUI>();

        _interactUiYOffsetRange.AssertValid();
        Assert.IsTrue(_interactUiFloatDuration > 0);
    }

    private void Update()
    {
        if (!FloatingAnimationEnabled) return;

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

    public void SetText(string text, float width = 400, float height = 250)
    {
        Rect.sizeDelta = new(width, height);
        _text.autoSizeTextContainer = true;
        _text.text = text;
        _text.rectTransform.sizeDelta = Rect.sizeDelta - TEXT_MARGIN;
    }

    public void SetTextAndFontSize(string text, float fontSize, float width = 400)
    {
        _text.autoSizeTextContainer = false;
        _text.fontSize = fontSize;
        _text.text = text;
        Rect.sizeDelta = new(width, Rect.sizeDelta.y);
        Rect.sizeDelta = new(Rect.sizeDelta.x, _text.preferredHeight);
        _text.rectTransform.sizeDelta = Rect.sizeDelta - TEXT_MARGIN;
    }
}
