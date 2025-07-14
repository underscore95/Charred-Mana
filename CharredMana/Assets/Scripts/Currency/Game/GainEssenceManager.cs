using UnityEngine;

public class GainEssenceManager : MonoBehaviour
{
    private PlayerDeathHandler _deathHandler;
    private void Awake()
    {
        _deathHandler = FindAnyObjectByType<PlayerDeathHandler>();
        _deathHandler.OnPlayerDie += GiveEssence;
    }

    private void OnDestroy()
    {
        _deathHandler.OnPlayerDie -= GiveEssence;
    }

    private void GiveEssence()
    {
        int totalExp = FindAnyObjectByType<PlayerLevel>().TotalExperienceGained;
        FindAnyObjectByType<CurrencyManager>().Add(CurrencyType.Essence, totalExp);
        Destroy(gameObject);
    }
}
