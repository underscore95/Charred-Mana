using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class ChunkTilemap : MonoBehaviour
{
    [SerializeField] protected Tilemap _tilemap;
    public Tilemap Tilemap { get { return _tilemap; } } 
    protected Chunk _chunk;

    public virtual void OnFirstLoad()
    {
        _chunk = GetComponent<Chunk>();

        _tilemap.size = new(Chunk.CHUNK_SIZE, Chunk.CHUNK_SIZE, 1);
    }

    public abstract void OnLoad();
}
