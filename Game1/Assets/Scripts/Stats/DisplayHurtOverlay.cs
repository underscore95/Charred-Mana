using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

// Entity can display a red overlay when they get hurt
public class DisplayHurtOverlay : MonoBehaviour
{
    private static readonly float DURATION = 0.25f; // excludes transition
    private static readonly float TRANSITION_DURATION = 0.15f;
    private static readonly Color COLOR = new(0.8f, 0.0f, 0.0f);

    [SerializeField] private SpriteRenderer _sprite;
    private float _secondsRed = 0;
    private bool _shouldBeRed = false;
    private Color _previousColor;

    private void Awake()
    {
        GetComponent<ILivingEntity>().OnDamaged() += OnTakeDamage;
    }

    private void Update()
    {
        if (!_shouldBeRed) return;
        _secondsRed += Time.deltaTime;
        float redness = Utils.GetTransitionValueInOut(_secondsRed, TRANSITION_DURATION, DURATION, TRANSITION_DURATION);
        Color c = Color.Lerp(_previousColor, COLOR, redness);
        _sprite.color = c;
       _shouldBeRed = redness > 0;
    }

    private void OnTakeDamage(float damage)
    {
        if (!_shouldBeRed) _previousColor = _sprite.color;
        _secondsRed = Mathf.Min(TRANSITION_DURATION, _secondsRed);
        _shouldBeRed = true;
    }
}
