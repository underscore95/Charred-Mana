using UnityEngine;
using UnityEngine.Events;

public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField] private GameObject _deathUi;

    private Player _player;

    public UnityAction OnPlayerDie { get; set; } = () => { };

    private void Awake()
    {
        _player = FindAnyObjectByType<Player>();
        _player.OnDamaged() += HandleDamage;
    }

    private void HandleDamage(float _)
    {
        if (_player.IsDead())
        {
            OnPlayerDie.Invoke();
            UIState.IsDeathStateUiOpen = true;
            _deathUi.SetActive(true);
            _player.OnDamaged() -= HandleDamage;
        }
    }
}
