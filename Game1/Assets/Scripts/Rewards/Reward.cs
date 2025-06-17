using UnityEngine;

public abstract class Reward : MonoBehaviour
{
    [SerializeField] protected string _title;
    public string Title { get { return _title; } }

    [SerializeField] protected string _description;
    public string Description { get { return _description; } }

    public abstract void Give();

    // If true, the reward can be given, if false it can't. A situation where the reward can't be given is it being a spell reward and the spell is already unlocked.
    public abstract bool CanGive();
}
