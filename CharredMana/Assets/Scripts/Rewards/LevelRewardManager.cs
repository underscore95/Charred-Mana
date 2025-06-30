using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class LevelRewardManager : MonoBehaviour
{
    [SerializeField] private GameObject _levelUpUi;
    [SerializeField] private TextMeshProUGUI _levelUpText;
    [SerializeField] private string _levelUpMessage = "You levelled up to Level {0}!\nSelect a reward.";
    [SerializeField] private Transform _registeredRewardsParent;
    [SerializeField] private Transform _availableRewardUisParent;
    [SerializeField] private Reward _defaultReward; // Reward to be used if we fail to find one that we can give

    private List<AvailableRewardUi> _availableRewardUis = new();
    private List<Reward> _registeredRewards = new();
    private List<List<List<Reward>>> _sortedRewards = new(); // _sortedRewards[category][rarity]
    private int _levelUpsRemaining = 0;
    private PlayerLevel _playerLevel;
    private bool _unlockedSpell = false;

    private void Start()
    {
        // Get registered rewards
        TransformUtils.GetChildrenWithComponent<Reward>(ref _registeredRewards, _registeredRewardsParent, true);
        Assert.IsTrue(_registeredRewards.Count > 0);

        // Sort rewards
        // Prepare list
        foreach (RewardCategory category in Enum.GetValues(typeof(RewardCategory)))
        {
            List<List<Reward>> list = new();
            foreach (Rarity rarity in Enum.GetValues(typeof(Rarity)))
            {
                list.Add(new List<Reward>());
            }
            _sortedRewards.Add(list);
        }

        // Sort
        foreach (Reward reward in _registeredRewards)
        {
            _sortedRewards[(int)reward.Category][(int)reward.Rarity].Add(reward);
        }

        // Get uis to display an available reward
        TransformUtils.GetChildrenWithComponent<AvailableRewardUi>(ref _availableRewardUis, _availableRewardUisParent, true);
        Assert.IsTrue(_availableRewardUis.Count > 0);

        // Listen for level ups
        _playerLevel = FindAnyObjectByType<PlayerLevel>();
        _playerLevel.OnLevelUp += () => _levelUpsRemaining++;

        CloseUi();
    }

    private void Update()
    {
        if (_levelUpsRemaining > 0 && !_levelUpUi.activeInHierarchy)
        {
            OpenUi();
        }
    }

    // Get all rewards of a specific category and rarity
    public IReadOnlyList<Reward> GetRewards(RewardCategory category, Rarity rarity)
    {
        return _sortedRewards[(int)category][(int)rarity];
    }

    private void SelectRewards()
    {
        Assert.IsTrue(_defaultReward.CanGive());
        Assert.IsTrue(_availableRewardUis.Count == 3); // this function was written assuming 3 rewards
        Assert.IsTrue(Enum.GetValues(typeof(RewardCategory)).Length == 3); // spell, stat, utility

        // Randomly choose which ui is used for which category
        int spellRewardIndex = UnityEngine.Random.Range(0, _availableRewardUis.Count);
        int statRewardIndex = (spellRewardIndex + UnityEngine.Random.Range(1, _availableRewardUis.Count)) % _availableRewardUis.Count;
        Assert.IsTrue(spellRewardIndex != statRewardIndex);
        int otherRewardIndex = 0;
        while (otherRewardIndex == spellRewardIndex || otherRewardIndex == statRewardIndex) otherRewardIndex = (otherRewardIndex + 1) % _availableRewardUis.Count;

        // Set rewards randomly
        SetAvailableReward(_availableRewardUis[spellRewardIndex], RewardCategory.Spell);
        SetAvailableReward(_availableRewardUis[statRewardIndex], RewardCategory.Stat);
        SetAvailableReward(_availableRewardUis[otherRewardIndex], RewardCategory.Utility);
    }

    // Sets the reward in a ui to a reward in the category
    // rarity is chosen at random (weighted towards more common rewards)
    private void SetAvailableReward(AvailableRewardUi ui, RewardCategory category)
    {
        ui.SetReward(_defaultReward);

        // Pick rarity
        if (RewardRarities.Roll(rarity => IsAvailableRewardRarityValid(category, rarity), out var rarity))
        {
            // Pick reward in rarity
            for (int i = 0; i < 500; ++i)
            {
                Reward reward = GetRewards(category, rarity)[UnityEngine.Random.Range(0, GetRewards(category, rarity).Count)];
                if (reward.CanGive())
                {
                    ui.SetReward(reward);
                    return;
                }
            }
        }

        // Didn't find a reward
        if (category == RewardCategory.Spell)
        {
            // No more spells available, give a stat upgrade instead
            SetAvailableReward(ui, RewardCategory.Stat);
        }
    }

    // Can this rarity be picked for this reward category?
    private bool IsAvailableRewardRarityValid(RewardCategory category, Rarity rarity)
    {
        foreach (Reward reward in GetRewards(category, rarity))
        {
            if (reward.CanGive()) return true;
        }
        return false;
    }

    private void OpenUi()
    {
        int levelWhenOpened = _playerLevel.Level - _levelUpsRemaining + 1;
        _levelUpText.text = string.Format(_levelUpMessage, levelWhenOpened);

        SelectRewards();

        _levelUpUi.SetActive(true);
        UIState.IsLevelUpRewardsUiOpen = true;
    }

    private void CloseUi()
    {
        _levelUpUi.SetActive(false);
        UIState.IsLevelUpRewardsUiOpen = false;

        if (_levelUpsRemaining <= 0 && _unlockedSpell)
        {
            _unlockedSpell = false;
        }
    }

    public IReadOnlyList<Reward> GetRegisteredRewards()
    {
        return _registeredRewards;
    }

    public void NotifyRewardClaimed(Reward claimed)
    {
        if (claimed is SpellReward)
        {
            _unlockedSpell = true;
        }

        Assert.IsTrue(_levelUpsRemaining > 0);
        _levelUpsRemaining--;
        CloseUi();
        if (_levelUpsRemaining > 0)
        {
            OpenUi();
        }
    }
}
