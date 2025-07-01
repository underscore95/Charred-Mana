using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class World : MonoBehaviour
{
    [SerializeField] private GameObject _chunkPrefab;
    private ObjectPool _pool;
    private readonly Dictionary<Vector2Int, Chunk> _loadedChunks = new();
    private Chunk _centerChunk;
    private Player _player;

    private void Awake()
    {
        _player = FindAnyObjectByType<Player>();

        _pool = new(_chunkPrefab, 9, transform);

        LoadAndUnloadChunksIfNecessary();
        FindAnyObjectByType<TurnManager>().OnTurnChange += LoadAndUnloadChunksIfNecessary;
    }

    private void LoadAndUnloadChunksIfNecessary()
    {
        // New center chunk
        Vector2Int newCenterChunkCoords = GetChunkCoords(Utils.ToV2I(_player.transform.position));
        if (_centerChunk != null && _centerChunk.ChunkCoords == newCenterChunkCoords) return; // don't need to change what chunks are loaded

        // What chunks should be loaded since the center changed?
        List<Vector2Int> newLoadedChunks = GetChunksAround(newCenterChunkCoords);

        // What chunks do we no longer need and what chunks do we already have?
        List<Vector2Int> chunksToUnload = new();
        foreach (var (chunkCoords, _) in _loadedChunks)
        {
            if (!newLoadedChunks.Contains(chunkCoords)) chunksToUnload.Add(chunkCoords);
            else newLoadedChunks.Remove(chunkCoords);
        }

        // Unload unused chunks
        foreach (var unloadCoords in chunksToUnload)
        {
            _pool.ReleaseObject(_loadedChunks[unloadCoords].gameObject);
            _loadedChunks.Remove(unloadCoords);
        }

        // Load new chunks
        foreach (var chunkCoords in newLoadedChunks)
        {
            GameObject chunk = _pool.ActivateObject(chunk => chunk.GetComponent<Chunk>().SetChunkPos(chunkCoords.x, chunkCoords.y));
            _loadedChunks[chunkCoords] = chunk.GetComponent<Chunk>();
        }

        _centerChunk = _loadedChunks[newCenterChunkCoords];
        Assert.IsNotNull(_centerChunk);
    }

    // Returns all chunks that would be loaded if the chunkCenterCoords was the center chunk
    // includes chunkCenterCoords
    private List<Vector2Int> GetChunksAround(Vector2Int chunkCenterCoords)
    {
        List<Vector2Int> chunks = new();
        for (int x = chunkCenterCoords.x - 1; x <= chunkCenterCoords.x + 1; x++)
        {
            for (int y = chunkCenterCoords.y - 1; y <= chunkCenterCoords.y + 1; y++)
            {
                chunks.Add(new(x, y));
            }
        }
        return chunks;
    }

    public Vector2Int GetChunkCoords(Vector2Int worldCoords)
    {
        return new Vector2Int(Mathf.FloorToInt(worldCoords.x / (float)Chunk.CHUNK_SIZE), Mathf.FloorToInt(worldCoords.y / (float)Chunk.CHUNK_SIZE));
    }

    public bool IsChunkLoaded(Vector2Int chunkCoords)
    {
        return _loadedChunks.ContainsKey(chunkCoords);
    }

    public Chunk GetChunk(Vector2Int chunkCoords)
    {
        Assert.IsTrue(IsChunkLoaded(chunkCoords));
        return _loadedChunks[chunkCoords];
    }
}
