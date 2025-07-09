using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class Chunk : MonoBehaviour
{
    private static readonly float PERLIN_NOISE_FREQUENCY = 8; // integer x y coordinates are divided by this value, keeping it a power of 2 is more efficient
    public static readonly int CHUNK_SIZE = 16;

    public Vector2Int ChunkCoords { get; private set; } = Vector2Int.zero;
    private BiomeManager _biomeManager;
    private readonly List<ChunkTilemap> _chunkTilemaps = new();
    private World _world;

    private void Awake()
    {
        _biomeManager = FindAnyObjectByType<BiomeManager>();

        _world = GetComponentInParent<World>();
        Assert.IsNotNull(_world);

        // order to load the tilemaps
        _chunkTilemaps.Add(GetComponent<ChunkTerrain>());
        _chunkTilemaps.Add(GetComponent<ChunkDecoration>());
        _chunkTilemaps.Add(GetComponent<ChunkCollisionDecorations>());
        Assert.IsTrue(_chunkTilemaps.Count == GetComponentsInChildren<Tilemap>().Length);

        foreach (var tilemap in _chunkTilemaps)
        {
            tilemap.OnFirstLoad();
        }
    }

    private void OnEnable()
    {
        foreach (var tilemap in _chunkTilemaps)
        {
            tilemap.OnLoad();
        }
    }

    public Vector2Int LocalCoordsToWorldCoords(Vector2Int local)
    {
        return new(ChunkCoords.x * CHUNK_SIZE + local.x, ChunkCoords.y * CHUNK_SIZE + local.y);
    }

    // Should only be called by the world
    internal void SetChunkPos(int x, int y)
    {
        ChunkCoords = new(x, y);
        transform.position = new(x * CHUNK_SIZE, y * CHUNK_SIZE, transform.position.z);
    }
    public float PerlinNoise(int localX, int localY)
    {
        Vector2Int world = LocalCoordsToWorldCoords(new(localX, localY));
        float noise = Mathf.PerlinNoise(world.x / PERLIN_NOISE_FREQUENCY, world.y / PERLIN_NOISE_FREQUENCY);
        noise = Mathf.Clamp01(noise);
        Assert.IsTrue(noise >= 0);
        Assert.IsTrue(noise <= 1);
        return noise;
    }
    public float WhiteNoise(int localX, int localY)
    {
        Vector2Int world = LocalCoordsToWorldCoords(new(localX, localY));
        float noise = Utils.WhiteNoise(world.x, world.y);
        Assert.IsTrue(noise >= 0);
        Assert.IsTrue(noise <= 1);
        return noise;
    }

    public Biome GetBiome(Vector2Int localCoords)
    {
        Vector2Int worldCoords = LocalCoordsToWorldCoords(localCoords);
        BiomeType biomeType = _world.WorldGenSettings.WorldBiomes.BiomeTypeAt(_world, worldCoords);
        return _biomeManager.GetBiome(biomeType);
    }
}
