using System;
using System.Collections.Generic;

// Represents a stat boost that won't be enabled until the game has reached a specific turn, see TurnManager for example usage
[Serializable]
public struct DelayedStatBoost
{
    public int RequiredTurn;
    public SerializableStatModifiersContainer StatModifiers;

    public static StatModifiersContainer GetMergedStatBoost(List<DelayedStatBoost> boosts, int currentTurn)
    {
        StatModifiersContainer boost = new();
        foreach (var delayedBoost in boosts)
        {
            if (delayedBoost.RequiredTurn <= currentTurn)
            {
                boost.Merge(delayedBoost.StatModifiers);
            }
        }
        return boost;
    }
}