using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public abstract class InteractableObject : MonoBehaviour
{
    protected Player _player;
    private GameObject _interactUi;
    private InteractableSettings _settings;

    protected virtual void Awake()
    {
        _player = FindAnyObjectByType<Player>();
        _settings = FindAnyObjectByType<InteractableSettings>();

        _interactUi = Instantiate(_settings.UiPrefab, transform);
        _interactUi.GetComponentInChildren<Textbox>().SetText("E"); // todo
        Assert.IsNotNull(_interactUi);
        _interactUi.SetActive(false);
    }

    protected virtual void Update()
    {
        if (!IsPlayerInRange()) return;


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