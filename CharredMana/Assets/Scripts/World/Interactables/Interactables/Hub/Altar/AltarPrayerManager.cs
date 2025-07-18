using UnityEngine;
using UnityEngine.Assertions;

public class AltarPrayerManager : MonoBehaviour
{
    private SaveManager _saveManager;
    private Player _player;
    private StatTypes _statTypes;

    private void Awake()
    {
        _player = FindAnyObjectByType<Player>();
        _statTypes = FindAnyObjectByType<StatTypes>();

        _saveManager = FindAnyObjectByType<SaveManager>();
    }

    private void Start()
    {
        // Apply stat increases
        foreach (var (stat, level) in _saveManager.Save.AltarPrayerLevels)
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
        Assert.IsNotNull(_saveManager.Save, "Attempted to get prayer level for stat " + stat + " when save wasn't loaded yet");
        if (_saveManager.Save.AltarPrayerLevels.TryGetValue(stat, out int level))
        {
            return level;
        }
        return 0;
    }

    public void UpgradePrayerLevel(StatType stat)
    {
        _saveManager.Save.AltarPrayerLevels[stat] = GetPrayerLevel(stat) + 1;

        // increase stat
        AltarStatInfo info = _statTypes.AltarStatInfo[stat];
        _player.EntityStats.ApplyModifier(stat, info.StatBoostPerPrayerLevel);
    }
}
