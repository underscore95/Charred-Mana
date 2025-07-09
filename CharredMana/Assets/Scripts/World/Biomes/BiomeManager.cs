using UnityEngine;
using UnityEngine.Assertions;

public class BiomeManager : MonoBehaviour
{
    private EnumDictionary<BiomeType, Biome> _biomes = new();

    private void Awake()
    {
        TransformUtils.FillEnumDictionaryFromChildren(ref _biomes, transform, b => b.Type);
    }

    public Biome GetBiome(BiomeType biomeType)
    {
        return _biomes[biomeType];
    }
}
