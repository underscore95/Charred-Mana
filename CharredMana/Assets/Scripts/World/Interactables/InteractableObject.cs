using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public abstract class InteractableObject : MonoBehaviour
{
    [SerializeField] private FloatRange _interactUiYOffsetRange = new(-0.05f, 0.05f);
    [SerializeField] private float _interactUiFloatDuration = 0.75f;
    [SerializeField] private Vector3 _interactUiOffset = Vector3.up + Vector3.back;

    protected Player _player;
    private GameObject _interactUi;
    private float _secondsVisible = 0;
    private bool _invertT = false;
    private InteractableSettings _settings;

    protected virtual void Awake()
    {
        _player = FindAnyObjectByType<Player>();
        _settings=FindAnyObjectByType<InteractableSettings>();

        _interactUi = Instantiate(_settings.UiPrefab, transform);
        Assert.IsNotNull(_interactUi);
        _interactUi.SetActive(false);

        _interactUiYOffsetRange.AssertValid();
        Assert.IsTrue(_interactUiFloatDuration > 0);
    }

    protected virtual void Update()
    {
        if (!IsPlayerInRange()) return;

        // Floating animation
        _secondsVisible += Time.deltaTime;
        if (_secondsVisible > _interactUiFloatDuration)
        {
            _secondsVisible -= _interactUiFloatDuration;
            _invertT = !_invertT;
        }
        float t = Mathf.InverseLerp(0, _interactUiFloatDuration, _secondsVisible);
        if (_invertT) t = 1 - t;
        _interactUi.transform.localPosition = _interactUiOffset + _interactUiYOffsetRange.Lerp(t) * Vector3.up;

        // Input
        if (_settings.Action.IsPressed())
        {
            OnInteract();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Assert.IsTrue(collision.gameObject == _player.gameObject, "Should only collide with player");
        _interactUi.SetActive(true);
        OnEnterRange();
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        Assert.IsTrue(collision.gameObject == _player.gameObject, "Should only collide with player");
        _interactUi.SetActive(false);
        OnExitRange();
    }

    public bool IsPlayerInRange()
    {
        return _interactUi.activeInHierarchy;
    }

    protected void OnEnterRange() { }
    protected void OnExitRange() { }
    protected abstract void OnInteract();
}