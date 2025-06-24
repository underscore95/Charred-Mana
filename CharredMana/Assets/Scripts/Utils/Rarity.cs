
// May want to see RewardRarities if adding another rarity
// Also see RewardColors for the rarity colour
public enum Rarity
{
    Common, Uncommon, Rare, Epic, Legendary
}

public static class Rarities
{
    public static Rarity Worst = Rarity.Common;
    public static Rarity Best = Rarity.Legendary;

    // Upgrade rarity to the next tier, returns false if already at the best tier
    public static bool Upgrade(ref Rarity rarity)
    {
        if (rarity >= Best) { rarity = Best; return false; }
        rarity++;
        return true;
    }

    // Downgrade rarity to the previous tier, returns false if already at the worst tier
    public static bool Downgrade(ref Rarity rarity)
    {
        if (rarity <= Worst) { rarity = Worst; return false; }
        rarity--;
        return true;
    }
}