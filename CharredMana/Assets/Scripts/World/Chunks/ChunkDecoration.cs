using UnityEngine;
using UnityEngine.Tilemaps;

// Decoration without collisions, e.g. grass, flowers
public class ChunkDecoration : ChunkTilemap
{
    public override void OnLoad()
    {
        Vector4Int seed = _chunk._world.Settings.DecorationSeed;
        for (int x = 0; x < Chunk.CHUNK_SIZE; x++)
        {
            for (int y = 0; y < Chunk.CHUNK_SIZE; y++)
            {
                if (_chunk.PerlinNoise(x + seed.x, y + seed.y) < 0.15f) continue;
                if (_chunk.WhiteNoise(x + seed.z, y + seed.w) < 0.925f) continue;

                Biome biome = _chunk.GetBiome(new(x, y));
                if (biome.Decoration.Count < 1) continue;

                int decoIndex = (int)(biome.Decoration.Count * _chunk.PerlinNoise(x, y));
                _tilemap.SetTile(new(x, y, 0), biome.Decoration[decoIndex]);
            }
        }
    }
}
