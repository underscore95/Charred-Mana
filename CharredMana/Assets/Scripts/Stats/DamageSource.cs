
public enum DamageSource
{
    Player, EnemyMelee, EnemyRanged, StatusEffect, Other
}

public static class DamageSources
{
    public static bool BypassesDefense(DamageSource damageSource)
    {
        switch (damageSource)
        {
            case DamageSource.Player:
            case DamageSource.EnemyMelee:
            case DamageSource.EnemyRanged:
                return false;
            default:
                return true;
        }
    }
}