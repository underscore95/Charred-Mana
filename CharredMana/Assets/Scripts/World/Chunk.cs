using TreeEditor;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    private static readonly float PERLIN_NOISE_FREQUENCY = 32; // integer x y coordinates are divided by this value, keeping it a power of 2 is more efficient
    public static readonly int CHUNK_SIZE = 16;
    private static readonly int SEED = 0;

    public Vector2 ChunkCoords { get; private set; } = Vector2.zero;
    private readonly Perlin _noise = new();

    private void Awake()
    {
        _noise.SetSeed(SEED);
        SetChunkPos(0, 0);
    }

    // Should only be called by the world
    public void SetChunkPos(int x, int y)
    {
        ChunkCoords = new(x, y);
        transform.position = new(x * CHUNK_SIZE, y * CHUNK_SIZE, transform.position.z);
    }

    public float Noise(int x) { return _noise.Noise(x / PERLIN_NOISE_FREQUENCY); }
    public float Noise(int x, int y) { return _noise.Noise(x / PERLIN_NOISE_FREQUENCY, y / PERLIN_NOISE_FREQUENCY); }
    public float Random(int x) { return Utils.DeterministicRandom(x, SEED); }
    public float Random(int x, int y) { return Utils.DeterministicRandom(x, y, SEED); }
}
