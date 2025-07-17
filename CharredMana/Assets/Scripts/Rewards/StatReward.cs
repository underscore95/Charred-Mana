using UnityEngine;

public class StatReward : Reward
{
    [SerializeField] internal SerializableStatModifiersContainer _modifiers = new();
    private Player _player;
    private Stats _playerStats;

    private void Start()
    {
        _player = FindAnyObjectByType<Player>();
        _playerStats = _player.EntityStats;
    }

    public override void Give()
    {
        float oldMaxHealth = _playerStats.MaxHealth;
        _playerStats.ApplyModifiers(_modifiers);
        if (_playerStats.MaxHealth > oldMaxHealth)
        {
            _playerStats.SetCurrentHealthSilently(_playerStats.CurrentHealth + _playerStats.MaxHealth - oldMaxHealth); // Heal the player if they gained max health
        }

        _player.UpdateStatDisplayText();
    }

    public override bool CanGive()
    {
        return true;
    }
}
