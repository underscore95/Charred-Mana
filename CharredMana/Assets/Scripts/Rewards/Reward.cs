using UnityEngine;

public abstract class Reward : MonoBehaviour
{
    [SerializeField] private string _title;
    public string Title { get { return _title; } internal set { _title = value; } }

    [SerializeField] private string _description;
    public string Description { get { return _description; } internal set { _description = value; } }

    [SerializeField] private RewardCategory _category;
    public RewardCategory Category { get { return _category; } internal set { _category = value; } }

    [SerializeField] private Rarity _rarity;
    public Rarity Rarity { get { return _rarity; } internal set { _rarity = value; } }

    public abstract void Give();

    // If true, the reward can be given, if false it can't. A situation where the reward can't be given is it being a spell reward and the spell is already unlocked.
    public abstract bool CanGive();
}
