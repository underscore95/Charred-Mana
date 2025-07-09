
using UnityEngine;

public class OverworldWorldBiomes : IWorldBiomes
{
    public BiomeType BiomeTypeAt(World world, Vector2Int worldCoords)
    {
        WorldGenSettings wgs = world.Settings;
        float inverseNoiseFrequency = 1.0f / wgs.NoiseFrequency;
        float temp = Mathf.Clamp01(Mathf.PerlinNoise(wgs.TemperatureSeed.x + worldCoords.x * inverseNoiseFrequency, wgs.TemperatureSeed.y + worldCoords.y * inverseNoiseFrequency));
        float moisture = Mathf.Clamp01(Mathf.PerlinNoise(wgs.MoistureSeed.x + worldCoords.x * inverseNoiseFrequency, wgs.MoistureSeed.y + worldCoords.y * inverseNoiseFrequency));

        if (temp > 0.4 && moisture < 0.35)
        {
            return BiomeType.Desert;
        }

        return BiomeType.Plains;
    }
}