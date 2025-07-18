
using UnityEngine;
using UnityEngine.UI;

/// When editing this, need to add field to <see cref="StatContainer"/> and update StatTypes (below)
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

public class StatTypes : MonoBehaviour
{
    //  [SerializeField] private SerializableEnumDictionary<StatType, Color> _statColors = new();
    //  public EnumDictionary<StatType, Color> StatColors { get; private set; } = null;


    [SerializeField] private SerializableEnumDictionary<StatType, AltarStatInfo> _altarStatInfo = new();
    public EnumDictionary<StatType, AltarStatInfo> AltarStatInfo { get; private set; } = null;

    private void Awake()
    {
        // StatColors = _statColors.ToEnumDictionary();
        AltarStatInfo = _altarStatInfo.ToEnumDictionary(SerializableEnumDictionary<StatType, AltarStatInfo>.MissingValues.Allow);
    }

    public static string ToString(StatType type)
    {
        return type switch
        {
            StatType.MaxHealth => "Max HP",
            StatType.Damage => "Damage",
            StatType.Defense => "Defense",
            StatType.ManaRegen => "MP Regen",
            StatType.Focus => "Focus",
            StatType.HealthRegen => "HP Regen",
            StatType.MaxMana => "Max MP",
            _ => type.ToString(),
        };
    }

}