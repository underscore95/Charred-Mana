using System;
using System.Collections.Generic;
using UnityEngine;

// Generates StatReward objects as children of this object, since most stat rewards should be the same number this is used to reduce repetition
public class GenerateStatRewards : MonoBehaviour
{
    [Serializable]
    public class GeneratedStatType
    {
        [SerializeField] public StatType Type;
        [Multiline(3)] [SerializeField] public string Desc = "Increases damage dealt by {0}.";
    }

    [Serializable]
    public class GeneratedStatValues
    {
        [SerializeField] public MathOp Operation;
        [SerializeField] public List<float> Values;
    }

    [SerializeField] private List<GeneratedStatType> _statTypes;
    [SerializeField] private List<GeneratedStatValues> _statValues;
    [SerializeField] private int _numDecimals = 0;

    private void Awake()
    {
        HashSet<StatType> types = new();
        HashSet<MathOp> ops = new();

        // Generate all the stat modifiers
        List<StatModifier> modifiers = new();
        foreach (GeneratedStatValues opType in _statValues)
        {
            if (!ops.Add(opType.Operation))
            {
                Debug.LogWarningFormat("{0} (GenerateStatRewards script) is attempting to generate values for the same operation ({1}), skipping the latter one(s).", gameObject.name, opType.Operation);
                continue;
            }

            foreach (float val in opType.Values)
            {
                StatModifier mod = new(opType.Operation, val);
                modifiers.Add(mod);
            }
        }

        // Add rewards for each stat
        foreach (GeneratedStatType type in _statTypes)
        {
            if (!types.Add(type.Type))
            {
                Debug.LogWarningFormat("{0} (GenerateStatRewards script) is attempting to generate values for the same stat ({1}), skipping the latter one(s).", gameObject.name, type.Type);
                continue;
            }

            foreach (StatModifier mod in modifiers)
            {
                GameObject obj = new(
                    string.Format("StatReward {0} {1}", StatTypes.ToString(type.Type), mod.ToString(_numDecimals)),
                    new Type[] { typeof(StatReward) }
                    );

                obj.transform.parent = transform;

                StatReward reward = obj.GetComponent<StatReward>();
                reward._modifiers.AddModifierNoMerging(type.Type, mod);
                reward.Title = mod.ToString(_numDecimals) + " " + StatTypes.ToString(type.Type);
                reward.Description = string.Format(type.Desc, mod.ToString(_numDecimals));
            }
        }
    }
}