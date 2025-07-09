
using UnityEngine;

public class OverworldWorldBiomes : IWorldBiomes
{
    private readonly float NOISE_FREQUENCY = 96;
    public BiomeType BiomeTypeAt(World world, Vector2Int worldCoords)
    {
        float temp = Mathf.Clamp01(Mathf.PerlinNoise(123489 + worldCoords.x / NOISE_FREQUENCY, 9192 + worldCoords.y / NOISE_FREQUENCY));
        float moisture = Mathf.Clamp01(Mathf.PerlinNoise(-238832 + worldCoords.x / NOISE_FREQUENCY, -234895 + worldCoords.y / NOISE_FREQUENCY));

        if (temp > 0.4 && moisture < 0.35)
        {
            return BiomeType.Desert;
        }

        return BiomeType.Plains;
    }
}