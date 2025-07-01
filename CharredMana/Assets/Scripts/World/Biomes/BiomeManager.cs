using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.Assertions;

public class BiomeManager : MonoBehaviour
{
    private readonly float NOISE_FREQUENCY = 128;
    private EnumDictionary<BiomeType, Biome> _biomes = new();

    private void Awake()
    {
        TransformUtils.FillEnumDictionaryFromChildren(ref _biomes, transform, b => b.Type);
    }

    public Biome GetBiome(Vector2Int worldCoords)
    {
        float temp = Mathf.Clamp01(Mathf.PerlinNoise(123489 + worldCoords.x / NOISE_FREQUENCY, 9192 + worldCoords.y / NOISE_FREQUENCY));
        float moisture = Mathf.Clamp01(Mathf.PerlinNoise(-238832 + worldCoords.x / NOISE_FREQUENCY, -234895 + worldCoords.y / NOISE_FREQUENCY));

        return _biomes[BiomeFromTempMoisture(temp, moisture)];
    }

    private BiomeType BiomeFromTempMoisture(float temp, float moisture)
    {
        Assert.IsTrue(_biomes.Count > 0);

        if (temp > 0.6 && moisture < 0.35)
        {
            return BiomeType.Desert;
        }

        return BiomeType.Plains;
    }
}
