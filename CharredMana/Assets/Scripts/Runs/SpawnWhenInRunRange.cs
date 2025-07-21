using UnityEngine;

public class SpawnWhenInRunRange : MonoBehaviour
{
    [SerializeField] private IntRange _range = new(1, 1);
    [SerializeField] private GameObject _prefab;

    private void Start()
    {
        int runs = FindAnyObjectByType<RunManager>().GetPlayedRuns();
        if (_range.Contains(runs))
        {
            Instantiate(_prefab, transform);
        }

        Destroy(this);
    }
}
