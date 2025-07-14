using System;
using UnityEngine.Assertions;

public static class UIState
{
    public static bool IsLevelUpRewardsUiOpen = false;
    public static bool IsSpellSelectUiOpen = false;
    public static bool IsDeathStateUiOpen = false;

    public static bool IsAnyUiOpen()
    {
        return
            IsLevelUpRewardsUiOpen ||
            IsSpellSelectUiOpen ||
            IsDeathStateUiOpen ||
            false
            ;
    }

    public static void MarkAllUiClosed()
    {
        IsLevelUpRewardsUiOpen = false;
        IsSpellSelectUiOpen = false;
        IsDeathStateUiOpen = false;
        Assert.IsFalse(IsAnyUiOpen());
    }
}