using UnityEngine;

public class SuicideButton : MonoBehaviour
{
    [SerializeField] private GameObject _confirmMenu;
    private Player _player;

    private void Awake()
    {
        _player = FindAnyObjectByType<Player>();
    }

    public void OpenConfirmMenu()
    {
        _confirmMenu.SetActive(true);
        UIState.IsSuicideUiOpen = true;
    }

    public void CloseConfirmMenu()
    {
        _confirmMenu.SetActive(false);
        UIState.IsSuicideUiOpen = false;
    }

    public void KillPlayer()
    {
        LivingEntity.Damage(_player, float.MaxValue, DamageSource.Other);
    }
}
