using System;
using UnityEngine;

[Serializable]
public class GameSave
{
    public EnumDictionary<CurrencyType, float> Currencies = new();
    public EnumDictionary<StatType, int> AltarPrayerLevels = new();
    public int RunsPlayed = 0;
}
