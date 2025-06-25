using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField] private GameObject _deathUi;

    private void Awake()
    {
        Player player = FindAnyObjectByType<Player>();
        player.OnDamaged() += _ =>
        {
            if (player.IsDead())
            {
                UIState.IsDeathStateUiOpen = true;
                _deathUi.SetActive(true);
            }
        };
    }
}
