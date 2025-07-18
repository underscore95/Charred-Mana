using UnityEngine;

public class AltarPrayerManager : MonoBehaviour
{
    private GameSave _save;
    private Player _player;
    private StatTypes _statTypes;

    private void Start()
    {
        _player = FindAnyObjectByType<Player>();
        _statTypes = FindAnyObjectByType<StatTypes>();

        SaveManager saveManager = FindAnyObjectByType<SaveManager>();
        _save = saveManager.Save;

        foreach (var (stat, level) in _save.AltarPrayerLevels)
        {
            AltarStatInfo info = _statTypes.AltarStatInfo[stat];
            for (int i = 0; i < level; i++)
            {
                _player.EntityStats.ApplyModifier(stat, info.StatBoostPerPrayerLevel);
            }
        }
    }

    public int GetPrayerLevel(StatType stat)
    {
        if (_save.AltarPrayerLevels.TryGetValue(stat, out int level))
        {
            return level;
        }
        return 0;
    }

    public void UpgradePrayerLevel(StatType stat)
    {
        _save.AltarPrayerLevels.Set(stat, GetPrayerLevel(stat) + 1);
    }
}
