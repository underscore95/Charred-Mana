public static class UIState
{
    public static bool IsLevelUpRewardsUiOpen = false;
    public static bool IsSpellSelectUiOpen = false;

    public static bool IsAnyUiOpen()
    {
        return 
            IsLevelUpRewardsUiOpen ||
            IsSpellSelectUiOpen ||
            false
            ;
    }
}