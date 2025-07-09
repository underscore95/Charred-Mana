using UnityEngine;

public interface IWorldBiomes
{
    public BiomeType BiomeTypeAt(World world, Vector2Int worldCoords);
}