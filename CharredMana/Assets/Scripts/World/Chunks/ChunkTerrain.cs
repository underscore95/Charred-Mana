using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class ChunkTerrain : ChunkTilemap
{
    public override void OnLoad()
    {
        for (int x = 0; x < Chunk.CHUNK_SIZE; x++)
        {
            for (int y = 0; y < Chunk.CHUNK_SIZE; y++)
            {
                Biome biome = _chunk.GetBiome(new(x, y));
                _tilemap.SetTile(new(x, y, 0), biome.Terrain);
            }
        }
    }
}
