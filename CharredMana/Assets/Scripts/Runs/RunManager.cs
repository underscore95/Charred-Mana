using System;
using UnityEngine;

public class RunManager : MonoBehaviour
{
    private SaveManager _saveManager;

    private void Awake()
    {
        _saveManager = FindAnyObjectByType<SaveManager>();
    }

    public int GetPlayedRuns()
    {
        return _saveManager.Save.RunsPlayed;
    }

    public void IncrementPlayedRuns()
    {
        _saveManager.Save.RunsPlayed++;
    }

    public void SetPlayedRuns(int run)
    {
        _saveManager.Save.RunsPlayed = run;
    }
}
