using System.Collections;
using UnityEngine;

public class DisplayOverlay : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;

    private float _elapsed = 0f;
    private float _transitionIn;
    private float _duration;
    private float _transitionOut;
    private Color _targetColor;
    private Color _originalColor;
    private bool _active = false;

    public void ApplyOverlay(Color color, float duration, float transitionIn, float transitionOut)
    {
        float fadeInProgress = _active ? Mathf.Clamp01(_elapsed / _transitionIn) : 1f;

        if (!_active)
        {
            _originalColor = _sprite.color;
        }
        _targetColor = color;

        _transitionIn = Mathf.Lerp(0, transitionIn, fadeInProgress);
        _duration = duration;
        _transitionOut = transitionOut;

        _elapsed = 0f;
        _active = true;
    }

    private void Update()
    {
        if (!_active) return;

        _elapsed += Time.deltaTime;
        float total = _transitionIn + _duration + _transitionOut;

        if (_elapsed >= total)
        {
            _sprite.color = _originalColor;
            _active = false;
            return;
        }

        float t = _elapsed;

        float blend;
        if (t < _transitionIn)
        {
            blend = t / _transitionIn;
        }
        else if (t < _transitionIn + _duration)
        {
            blend = 1f;
        }
        else
        {
            blend = 1f - (t - _transitionIn - _duration) / _transitionOut;
        }

        _sprite.color = Color.Lerp(_originalColor, _targetColor, Mathf.Clamp01(blend));
    }

    private void OnEnable()
    {
        if (_active)
        {
            _active = false;
            _sprite.color = _originalColor;
        }
    }
}
