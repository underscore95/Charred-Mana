
/// Need to add field to <see cref="StatContainer"/> and StatTypes (below)
public enum StatType
{
    MaxHealth,
    Damage,
    Defense,
    ManaRegen,
    Focus,
    HealthRegen,
    MaxMana
}

public static class StatTypes
{
    public static string ToString(StatType type)
    {
        switch (type)
        {
            case StatType.MaxHealth:
                return "Max HP";
            case StatType.Damage:
                return "Damage";
            case StatType.Defense:
                return "Defense";
            case StatType.ManaRegen:
                return "MP Regen";
            case StatType.Focus:
                return "Focus";
            case StatType.HealthRegen:
                return "HP Regen";
            case StatType.MaxMana:
                return "Max MP";
            default:
                return type.ToString();
        }
    }

}