using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static readonly float TURN_CHANGE_COOLDOWN = 0.5f;

    [SerializeField]
    private InputActionReference _moveInput;
    private bool _canGoToNextTurn = true;
    private Player _player;
    private Rigidbody2D _rigidBody;
    private TurnManager _turnManager;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _turnManager = FindAnyObjectByType<TurnManager>();
    }

    private void Update()
    {
        _player.PositionAtFrameStart = transform.position;

        if (!_canGoToNextTurn) return;

        var moveDirection = _moveInput.ToInputAction().ReadValue<Vector2>();
        if (moveDirection == Vector2.zero) return;

        Vector3 displacement = Vector3.zero;
        if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
        {
            displacement += moveDirection.x > 0 ? Vector3.right : Vector3.left;
        }
        else
        {
            displacement += moveDirection.y > 0 ? Vector3.up : Vector3.down;
        }

        StartCoroutine(Utils.MoveRigidBody(_rigidBody, displacement));
        _turnManager.OnTurnChange.Invoke();
        StartCoroutine(StartNextTurnCooldown());
    }

    private IEnumerator StartNextTurnCooldown()
    {
        Assert.IsTrue(_canGoToNextTurn);
        _canGoToNextTurn = false;
        yield return new WaitForSeconds(TURN_CHANGE_COOLDOWN + 0.05f);
        _canGoToNextTurn = true;
    }
}
