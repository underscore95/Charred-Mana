using TreeEditor;
using UnityEngine;
using UnityEngine.Assertions;

public class Chunk : MonoBehaviour
{
    private static readonly float PERLIN_NOISE_FREQUENCY = 8; // integer x y coordinates are divided by this value, keeping it a power of 2 is more efficient
    public static readonly int CHUNK_SIZE = 16;
    private static readonly int SEED = 0.GetHashCode();

    public Vector2Int ChunkCoords { get; private set; } = Vector2Int.zero;
    private readonly Perlin _noise = new();

    private void Awake()
    {
        _noise.SetSeed(SEED);
    }

    // Should only be called by the world
    internal void SetChunkPos(int x, int y)
    {
        ChunkCoords = new(x, y);
        transform.position = new(x * CHUNK_SIZE, y * CHUNK_SIZE, transform.position.z);
    }

    public float Noise(int x)
    {
        Assert.IsTrue(x >= 0 && x < CHUNK_SIZE);
        return _noise.Noise((ChunkCoords.x * CHUNK_SIZE + x) / PERLIN_NOISE_FREQUENCY);
    }
    public float Noise(int x, int y)
    {
        Assert.IsTrue(x >= 0 && x < CHUNK_SIZE);
        Assert.IsTrue(y >= 0 && y < CHUNK_SIZE);
        return _noise.Noise((ChunkCoords.x * CHUNK_SIZE + x) / PERLIN_NOISE_FREQUENCY, (ChunkCoords.y * CHUNK_SIZE + y) / PERLIN_NOISE_FREQUENCY);
    }
    public float Random(int x)
    {
        Assert.IsTrue(x >= 0 && x < CHUNK_SIZE);
        return Utils.DeterministicRandom((ChunkCoords.x * CHUNK_SIZE + x), SEED);
    }
    public float Random(int x, int y)
    {
        Assert.IsTrue(x >= 0 && x < CHUNK_SIZE);
        Assert.IsTrue(y >= 0 && y < CHUNK_SIZE);
        return Utils.DeterministicRandom((ChunkCoords.x * CHUNK_SIZE + x), (ChunkCoords.y * CHUNK_SIZE + y), SEED);
    }
}
