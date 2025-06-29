using UnityEngine;

public class DamageUpEffect : StatEffect
{
    [SerializeField] private float _percentIncreasePerLevel = 10;

    protected override StatModifiersContainer GetStatsForCurrentAmplifier()
    {
        return new StatModifiersContainer(StatType.Damage, new(MathOp.Multiply, _percentIncreasePerLevel / 100 * Amplifier + 1));
    }
}
