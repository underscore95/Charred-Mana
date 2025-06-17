using UnityEngine;

public class HealReward : Reward
{
    private Stats _playerStats;

    private void Start()
    {
        _playerStats = FindAnyObjectByType<Player>().Stats;
    }

    public override void Give()
    {
        _playerStats.Heal();
    }
    public override bool CanGive()
    {
        return true;
    }
}
