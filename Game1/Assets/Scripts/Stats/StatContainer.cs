
using JetBrains.Annotations;
using System;
using UnityEngine.Assertions;

[System.Serializable]
public class StatContainer
{
    public float MaxHealth = 100;
    public float Defense = 0;
    public float Damage = 5;

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
        MaxHealth = other.MaxHealth;
        Defense = other.Defense;
        Damage = other.Damage;
    }
}