using UnityEngine;

public class SingleStatPercentageUpEffect : StatEffect
{
    [SerializeField] private StatType _whatStat = StatType.Damage;
    [SerializeField] private float _percentIncreasePerLevel = 10;

    protected override StatModifiersContainer GetStatsForCurrentAmplifier()
    {
        return new StatModifiersContainer(_whatStat, new(MathOp.Multiply, _percentIncreasePerLevel / 100 * Amplifier + 1));
    }
}
