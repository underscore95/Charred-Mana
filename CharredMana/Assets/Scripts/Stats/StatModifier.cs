using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class StatModifier
{
    public MathOp Operation;
    public float Value;

    public StatModifier(MathOp op, float value)
    {
        Operation = op;
        Value = value;
    }

    public StatModifier(StatModifier statModifier) : this(statModifier.Operation, statModifier.Value)
    {

    }

    public override string ToString()
    {
        return ToString(5, false);
    }

    public string ToString(int numDecimals, bool trimTrailingZeroes)
    {
        return MathOps.ToString(Operation, Value, numDecimals, trimTrailingZeroes);
    }

    public void Apply(ref float value)
    {
        value = MathOps.Eval(Operation, value, Value);
    }

    // Merge other modifier into this, returns true if successful, returns false if not, in which case this modifier wasn't changed
    public bool Merge(StatModifier other)
    {
        if (other.Operation != Operation) return false;
        Value = MathOps.Merge(Operation, other.Value, Value);
        return true;
    }

    // Merge all the modifiers that can be merged
    public static void MergeList(ref List<StatModifier> list)
    {
        for (int mergingIntoIndex = 0; mergingIntoIndex < list.Count; mergingIntoIndex++)
        {
            for (int possiblyRemovingIndex = 0; possiblyRemovingIndex < list.Count; possiblyRemovingIndex++)
            {
                if (possiblyRemovingIndex == mergingIntoIndex) continue;
                if (list[mergingIntoIndex].Merge(list[possiblyRemovingIndex]))
                {
                    list.RemoveAt(possiblyRemovingIndex);
                    possiblyRemovingIndex--;
                    continue;
                }
            }
        }
    }

    public static void MergeEveryList(ref Dictionary<StatType, List<StatModifier>> mods)
    {
        foreach (var type in mods.Keys)
        {
            List<StatModifier> list = mods[type];
            MergeList(ref list);
            Assert.IsTrue(list.Count == mods.Count);
        }
    }

    public static void ApplyModifierToList(ref List<StatModifier> list, StatModifier modifier)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Merge(new(modifier)))
            {
                return;
            }
        }
        list.Add(modifier);
    }

    // Invert the modifier  so that applying this and the inverted results in no change
    public void Invert()
    {
        Value *= -1;
    }

    // Return the modifier you get if you apply the modifier n times
    public static StatModifier MergeN(StatModifier modifier, int n)
    {
        Assert.IsTrue(n >= 0);
        if (n == 0) return new(modifier.Operation, MathOps.GetDefaultValue(modifier.Operation));
        StatModifier mod = new(modifier);
        for (int i = 1; i < n; i++)
        {
            bool merged = mod.Merge(modifier);
            Assert.IsTrue(merged);
        }
        return mod;
    }
}