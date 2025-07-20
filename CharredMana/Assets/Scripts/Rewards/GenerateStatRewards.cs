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
        [Multiline(3)][SerializeField] public string Desc = "Increases damage dealt by {0}.";
    }

    [Serializable]
    public class GeneratedStatValues
    {
        [SerializeField] public Rarity Rarity;
        [SerializeField] public MathOp Operation;
        [SerializeField] public List<float> Values;
    }

    private struct StatRewardBase
    {
        public Rarity Rarity;
        public StatModifier Modifier;
    }

    [SerializeField] private List<GeneratedStatType> _statTypes;
    [SerializeField] private List<GeneratedStatValues> _statValues;
    [SerializeField] private int _numDecimals = 0;

    private void Awake()
    {
        HashSet<StatType> types = new();
        HashSet<MathOp> ops = new();

        // Generate all the stat modifiers
        List<StatRewardBase> baseRewards = new();
        foreach (GeneratedStatValues opType in _statValues)
        {
            foreach (float val in opType.Values)
            {
                StatModifier mod = new(opType.Operation, val);
                baseRewards.Add(new()
                {
                    Rarity = opType.Rarity,
                    Modifier = mod
                });
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

            foreach (StatRewardBase baseReward in baseRewards)
            {
                GameObject obj = new(
                    string.Format("{2} StatReward {0} {1}", StatTypes.ToString(type.Type), baseReward.Modifier.ToString(_numDecimals, true), baseReward.Rarity),
                    new Type[] { typeof(StatReward) }
                    );

                obj.transform.parent = transform;

                StatReward reward = obj.GetComponent<StatReward>();
                reward._modifiers.AddModifierNoMerging(type.Type, baseReward.Modifier);
                reward.Title = baseReward.Modifier.ToString(_numDecimals, true) + " " + StatTypes.ToString(type.Type);
                reward.Description = string.Format(type.Desc, baseReward.Modifier.ToString(_numDecimals, true));
                reward.Category = RewardCategory.Stat;
                reward.Rarity = baseReward.Rarity;
            }
        }
    }
}