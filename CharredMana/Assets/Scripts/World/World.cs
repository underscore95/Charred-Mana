using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField] private bool GenerateWorldOnStart = false; // WorldFloorManager will generate for us
    [SerializeField] private WorldGenSettings _worldGenSettings;
    public WorldGenSettings Settings { get { return _worldGenSettings; } }

    private ChunkManager _chunkManager;

    private void Awake()
    {
        _chunkManager = GetComponent<ChunkManager>();
    }

    private void Start()
    {
        if (GenerateWorldOnStart)
        {
            RegenerateWorld(_worldGenSettings);
        }
    }

    public void RegenerateWorld(WorldGenSettings worldGenSettings)
    {
        _worldGenSettings = worldGenSettings;
        Settings.Init();
        _chunkManager.ReloadChunks();
    }
}
