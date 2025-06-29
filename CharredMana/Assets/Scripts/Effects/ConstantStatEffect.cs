
using UnityEngine;

public class ConstantStatEffect : StatEffect
{
    [SerializeField] private SerializableStatModifiersContainer _modifiers = new();

    protected override StatModifiersContainer GetStatsForCurrentAmplifier()
    {
        return _modifiers;
    }
}