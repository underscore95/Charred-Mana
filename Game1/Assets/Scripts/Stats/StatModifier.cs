using System;
using System.Collections.Generic;
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

    public void Apply(ref float value)
    {
        value = MathOps.Eval(Operation, value, Value);
    }

    // Merge other modifier into this, returns true if successful, returns false if not, in which case this modifier wasn't changed
    public bool Merge(StatModifier other)
    {
        if (other.Operation != Operation) return false;
        Value += other.Value;
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
            if (list[i].Merge(modifier)) return;
        }
        list.Add(modifier);
    }
}