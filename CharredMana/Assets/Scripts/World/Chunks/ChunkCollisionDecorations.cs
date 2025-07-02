using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkCollisionDecorations : ChunkTilemap
{
    private ChunkDecoration _decoration;

    public override void OnFirstLoad()
    {
        base.OnFirstLoad();
        _decoration = GetComponent<ChunkDecoration>();
    }

    public override void OnLoad()
    {
        for (int x = 0; x < Chunk.CHUNK_SIZE; x++)
        {
            for (int y = 0; y < Chunk.CHUNK_SIZE; y++)
            {
                if (_decoration.Tilemap.HasTile(new(x, y))) continue;
                if (_chunk.WhiteNoise(x + 2397, 23978 + y) < 0.99f) continue;

                Biome biome = _chunk.GetBiome(new(x, y));
                if (biome.CollisionDecorations.Count < 1) continue;

                int decoIndex = (int)(biome.CollisionDecorations.Count * _chunk.PerlinNoise(x, y));
                _tilemap.SetTile(new(x, y, 0), biome.CollisionDecorations[decoIndex]);
            }
        }
    }
}
