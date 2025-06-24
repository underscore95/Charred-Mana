

using System;
using UnityEngine;
using UnityEngine.Assertions;

public static class RewardRarities
{
    // What rarity should the reward be?
    // Takes in a function to determnie if the rarity is allowed (a reason it wouldn't be allowed is no rewards of that rarity)
    // If rarity isn't allowed, will be downgraded till it is, if no downgraded rarities are allowed then it will be upgraded till it is
    // Returns false if no valid rarity found
    public static bool Roll(Func<Rarity, bool> allowedRarity, out Rarity r)
    {
        float v = UnityEngine.Random.Range(0.0f, 1.0f);

        r = Rarity.Legendary;
        if (v <= 0.45f) r = Rarity.Common;
        else if (v <= 0.75f) r = Rarity.Uncommon;
        else if (v <= 0.85f) r = Rarity.Rare;
        else if (v <= 0.95f) r = Rarity.Epic;

        Rarity originalRarity = r;
        do
        {
            if (allowedRarity(r)) return true;
        } while (Rarities.Downgrade(ref r));

        Rarities.Upgrade(ref r);
        do
        {
            if (allowedRarity(r)) return true;
        } while (Rarities.Upgrade(ref r));

        r = originalRarity;
        return false;
    }
}