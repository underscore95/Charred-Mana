using UnityEngine;
using UnityEngine.Assertions;

public class GainEssenceManager : MonoBehaviour
{
    private PlayerDeathHandler _deathHandler;
    private CurrencyManager _currencyManager;
    private PlayerLevel _level;
    private TurnManager _turnManager;
    private void Awake()
    {
        _level = FindAnyObjectByType<PlayerLevel>();
        _currencyManager = FindAnyObjectByType<CurrencyManager>();
        _turnManager = FindAnyObjectByType<TurnManager>();

        _deathHandler = FindAnyObjectByType<PlayerDeathHandler>();
        _deathHandler.OnPlayerDie += GiveEssence;
    }

    private void OnDestroy()
    {
        _deathHandler.OnPlayerDie -= GiveEssence;
    }

    private void GiveEssence()
    {
        int essence = GetEssenceGainedThisRun();
        _currencyManager.Add(CurrencyType.Essence, essence);
        Destroy(gameObject);
    }

    public int GetEssenceGainedThisRun()
    {
        int totalExp = _level.TotalExperienceGained;
        int turns = _turnManager.CurrentTurn;
        return totalExp / 77 + turns / 46;
    }
}
