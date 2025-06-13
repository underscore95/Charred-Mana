using UnityEngine;

public class HeakReward : Reward
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
}
