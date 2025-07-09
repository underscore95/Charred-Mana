
using System;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class WorldGenSettings
{
    [SerializeField] private WorldBiomesType _biomes;
    private IWorldBiomes _cachedWorldBiomes = null;
    public IWorldBiomes WorldBiomes { get { Assert.IsNotNull(_cachedWorldBiomes); return _cachedWorldBiomes; } }

    [SerializeField] private int _noiseFrequency = 96;
    public int NoiseFrequency { get { return _noiseFrequency; } }

    [SerializeField] private int _seed = 0;
    public Vector2Int TemperatureSeed { get; private set; }
    public Vector2Int MoistureSeed { get; private set; }
    public Vector4Int DecorationSeed { get; private set; }
    public Vector2Int CollisionDecorationSeed { get; private set; }

    public void Init()
    {
        _cachedWorldBiomes = WorldBiomesRegistry.Get(_biomes);
        TemperatureSeed = CreateSubSeed2(391278);
        MoistureSeed = CreateSubSeed2(123794);
        DecorationSeed = CreateSubSeed4(23974809);
        CollisionDecorationSeed = CreateSubSeed2(10239047);
    }

    private Vector4Int CreateSubSeed4(int seed)
    {
        Vector2Int seeds = CreateSubSeed2(seed);
        return new(CreateSubSeed2(seeds.x * 12836724), CreateSubSeed2(seeds.y * 2323978));
    }

    private Vector2Int CreateSubSeed2(int seed)
    {
        int a = 374761393;
        int b = 668265263;
        int c = 1274126177;
        int d = 168254137;

        int s1 = (int)(Utils.WhiteNoise(seed * a + b, _seed) * 110371) + 231745;
        int s2 = (int)(Utils.WhiteNoise(seed * b + c, _seed) * 133794) + 239074;
        int s3 = (int)(Utils.WhiteNoise(seed * c + d, _seed) * 102358) + 232378;
        int s4 = (int)(Utils.WhiteNoise(seed * d + a, _seed) * 120789) + 235078;

        return new Vector2Int(
            (int)(Utils.WhiteNoise(s1, _seed) * s2),
            (int)(Utils.WhiteNoise(s3, _seed) * s4)
        );
    }
}