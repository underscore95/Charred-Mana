using UnityEngine;

public class StatReward : Reward
{
    [SerializeField] internal SerializableStatModifiersContainer _modifiers = new();
    private Stats _playerStats;

    private void Start()
    {
        _playerStats = FindAnyObjectByType<Player>().EntityStats;
    }

    public override void Give()
    {
        float oldMaxHealth = _playerStats.MaxHealth;
        _playerStats.ApplyModifiers(_modifiers);
        if (_playerStats.MaxHealth > oldMaxHealth)
        {
            _playerStats.SetCurrentHealthSilently(_playerStats.CurrentHealth + _playerStats.MaxHealth - oldMaxHealth); // Heal the player if they gained max health
        }
    }

    public override bool CanGive()
    {
        return true;
    }
}
