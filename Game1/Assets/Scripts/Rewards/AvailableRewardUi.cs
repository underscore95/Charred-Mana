using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class AvailableRewardUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _desc;
    [SerializeField] private Button _selectButton;
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
    }

    public void GiveReward()
    {
        _reward.Give();
        _reward = null;
        _rewardManager.NotifyRewardClaimed();
    }
}
