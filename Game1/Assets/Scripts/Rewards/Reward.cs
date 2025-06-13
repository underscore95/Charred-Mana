using UnityEngine;

public abstract class Reward : MonoBehaviour
{
    [SerializeField] private string _title;
    public string Title { get { return _title; } }

    [SerializeField] private string _description;
    public string Description { get { return _description; } }

    public abstract void Give();
}
