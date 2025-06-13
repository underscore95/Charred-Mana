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

    private List<AvailableRewardUi> _availableRewardUis = new();
    private List<Reward> _registeredRewards = new();
    private int _levelUpsRemaining = 0;
    private PlayerLevel _playerLevel;

    private void Awake()
    {
        // Get registered rewards
        TransformUtils.GetChildrenWithComponent<Reward>(ref _registeredRewards, _registeredRewardsParent, true);
        Assert.IsTrue(_registeredRewards.Count > 0);

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

    private void SelectRewards()
    {
        foreach (AvailableRewardUi ui in _availableRewardUis)
        {
            ui.SetReward(_registeredRewards[Random.Range(0, _registeredRewards.Count)]);
        }
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
    }

    public IReadOnlyList<Reward> GetRegisteredRewards()
    {
        return _registeredRewards;
    }

    public void NotifyRewardClaimed()
    {
        Assert.IsTrue(_levelUpsRemaining > 0);
        _levelUpsRemaining--;
        CloseUi();
        if (_levelUpsRemaining > 0)
        {
            OpenUi();
        }
    }
}
