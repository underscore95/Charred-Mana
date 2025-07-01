using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkDecoration : ChunkTilemap
{
    [SerializeField] private TileBase _grass;

    protected void OnEnable()
    {
        for (int x = 0; x < Chunk.CHUNK_SIZE; x++)
        {
            for (int y = 0; y < Chunk.CHUNK_SIZE; y++)
            {
                if (_chunk.Noise(x, y) < 0.15f) continue;
                if (_chunk.Random(x, y) < 0.85f) continue;

                _tilemap.SetTile(new(x, y, 0), _grass);
            }
        }
    }
}
