using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkTilemap : MonoBehaviour
{
    [SerializeField] protected Tilemap _tilemap;
    protected Chunk _chunk;

    protected void Awake()
    {
        _chunk = GetComponent<Chunk>();

        _tilemap.size = new(Chunk.CHUNK_SIZE, Chunk.CHUNK_SIZE, 1);
    }
}
