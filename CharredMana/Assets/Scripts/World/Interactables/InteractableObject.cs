using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public abstract class InteractableObject : MonoBehaviour
{
    private static InteractableObject _interactingWith = null;

    protected Player _player;
    private GameObject _interactUi;
    private InteractableSettings _settings;

    protected virtual void Awake()
    {
        _player = FindAnyObjectByType<Player>();
        _settings = FindAnyObjectByType<InteractableSettings>();

        _interactUi = Instantiate(_settings.UiPrefab, transform);
        _interactUi.GetComponentInChildren<Textbox>().SetText("E", 100, 100); // todo
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

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        Assert.IsTrue(collision.gameObject == _player.gameObject, "Should only collide with player");
        if (_interactingWith == this) return;
        if (_interactingWith != null)
        {
            float ourDistanceSqr = ((Vector2)transform.position - (Vector2)_player.transform.position).sqrMagnitude;
            float otherDistanceSqr = ((Vector2)_interactingWith.transform.position - (Vector2)_player.transform.position).sqrMagnitude;
            if (otherDistanceSqr <= ourDistanceSqr) return;

            _interactingWith.PlayerLeftRange();
        }
        _interactingWith = this;
        _interactUi.SetActive(true);
        OnEnterRange();
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        Assert.IsTrue(collision.gameObject == _player.gameObject, "Should only collide with player");
        if (_interactingWith == this)
        {
            PlayerLeftRange();
        }
    }

    private void PlayerLeftRange()
    {
        _interactingWith = null;
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