using UnityEngine;

public class StatReward : Reward
{
    [SerializeField] private SerializableStatModifiersContainer _modifiers = new();
    private Stats _playerStats;

    private void Start()
    {
        _playerStats = FindAnyObjectByType<Player>().Stats;
    }

    public override void Give()
    {
        float oldMaxHealth = _playerStats.MaxHealth;
        _playerStats.ApplyModifiers(_modifiers);
        if (_playerStats.MaxHealth > oldMaxHealth)
        {
            _playerStats.CurrentHealth += (_playerStats.MaxHealth - oldMaxHealth);
        }
    }

    public override bool CanGive()
    {
        return true;
    }
}
