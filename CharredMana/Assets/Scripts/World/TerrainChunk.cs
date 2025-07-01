using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainChunk : MonoBehaviour
{
    public static readonly int CHUNK_SIZE = 16;

    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileBase _grass;

    private Vector2 _chunkCoords = Vector2.zero;

    private void Awake()
    {
        _tilemap.size = new(CHUNK_SIZE, CHUNK_SIZE, 1);
    }

    private void OnEnable()
    {
        for (int x = 0; x < CHUNK_SIZE; x++)
        {
            for (int y = 0; y < CHUNK_SIZE; y++)
            {
                _tilemap.SetTile(new(x, y, 0), _grass);
            }
        }
    }

    public void SetChunkPos(int x, int y)
    {
        _chunkCoords.x = x;
        _chunkCoords.y = y;
        transform.position = new(x * CHUNK_SIZE, y * CHUNK_SIZE, transform.position.z);
    }
}
