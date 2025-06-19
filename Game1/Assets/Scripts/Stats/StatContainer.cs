
using System;
using UnityEngine.Assertions;

[System.Serializable]
public class StatContainer
{
    // todo private setters in Stats
    // if adding field, don't forget to add condition to set and get functions
    public float MaxHealth = 100;
    public float Defense = 0;
    public float Damage = 5;
    public float ManaRegen = 5; // Only affects players
    public float Focus = 3.0f; // Only affects players
    public float HealthRegen = 2.0f; // Only affects players
    public float MaxMana = 100.0f; // Only affects players
    public float Get(StatType type)
    {
        switch (type)
        {
            case StatType.MaxHealth:
                return MaxHealth;
            case StatType.Defense:
                return Defense;
            case StatType.Damage:
                return Damage;
            case StatType.ManaRegen:
                return ManaRegen;
            case StatType.Focus:
                return Focus;
            case StatType.HealthRegen:
                return HealthRegen;
            case StatType.MaxMana:
                return MaxMana;
            default:
                Assert.IsTrue(false);
                return 0;
        }
    }

    public void Set(StatType type, float value)
    {
        switch (type)
        {
            case StatType.MaxHealth:
                MaxHealth = value;
                break;
            case StatType.Defense:
                Defense = value;
                break;
            case StatType.Damage:
                Damage = value;
                break;
            case StatType.ManaRegen:
                ManaRegen = value;
                break;
            case StatType.Focus:
                Focus = value;
                break;
            case StatType.HealthRegen:
                HealthRegen = value;
                break;
            case StatType.MaxMana:
                MaxMana = value;
                break;
            default:
                Assert.IsTrue(false);
                break;
        }
    }

    public StatContainer() { }

    public StatContainer(StatContainer copy)
    {
        SetEqualTo(copy);
    }

    protected void SetEqualTo(StatContainer other)
    {
        foreach (StatType type in Enum.GetValues(typeof(StatType)))
        {
            Set(type, other.Get(type));
        }
    }
}