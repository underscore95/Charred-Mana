using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkTerrain : ChunkTilemap
{
    [SerializeField] private TileBase _grass;

    protected void OnEnable()
    {
        for (int x = 0; x < Chunk.CHUNK_SIZE; x++)
        {
            for (int y = 0; y < Chunk.CHUNK_SIZE; y++)
            {
                _tilemap.SetTile(new(x, y, 0), _grass);
            }
        }
    }
}
