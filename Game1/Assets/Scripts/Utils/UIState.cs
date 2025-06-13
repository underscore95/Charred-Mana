public static class UIState
{
    public static bool IsLevelUpRewardsUiOpen = false;

    public static bool IsAnyUiOpen()
    {
        return 
            IsLevelUpRewardsUiOpen ||
            false
            ;
    }
}