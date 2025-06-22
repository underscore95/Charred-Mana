using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class AvailableRewardUi : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _desc;
    [SerializeField] private Button _selectButton;
    [SerializeField] private Image _background;
    [SerializeField] private RewardColors _rewardColors;
    private Reward _reward;
    private LevelRewardManager _rewardManager;

    private void Awake()
    {
        Assert.IsTrue(_title != null);
        Assert.IsTrue(_desc != null);
        Assert.IsTrue(_selectButton != null);
        Assert.IsTrue(_title.transform.parent == transform);
        Assert.IsTrue(_desc.transform.parent == transform);
        Assert.IsTrue(_selectButton.transform.parent == transform);
        _rewardManager = FindAnyObjectByType<LevelRewardManager>();

        _selectButton.onClick.AddListener(GiveReward);
    }

    public void SetReward(Reward reward)
    {
        Assert.IsNotNull(reward);
        _reward = reward;

        _title.text = _reward.Title;
        _desc.text = _reward.Description;
        _background.color = _rewardColors.GetColor(_reward.Rarity);
    }

    public void GiveReward()
    {
        Assert.IsTrue(_reward.CanGive());
        _reward.Give();
        _rewardManager.NotifyRewardClaimed(_reward);
    }
}
