
using UnityEngine.Assertions;

public enum WorldBiomesType
{
    Overworld
}

public static class WorldBiomesRegistry
{
    private static readonly IWorldBiomes OVERWORLD = new OverworldWorldBiomes();

    public static IWorldBiomes Get(WorldBiomesType biomes)
    {
        switch (biomes)
        {
            case WorldBiomesType.Overworld:
                return OVERWORLD;
            default:
                Assert.IsTrue(false, "Missing case");
                return null;
        }
    }
}