using System;
using UnityEngine;

[Serializable]
public class AltarStatInfo
{
    public string Title;
    public Sprite SplashImage;
    public StatModifier StatBoostPerPrayerLevel = new(MathOp.Add, 5);
}
