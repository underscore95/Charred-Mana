using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField] private WorldGenSettings _worldGenSettings;
    public WorldGenSettings Settings { get { return _worldGenSettings; } }

    private void Awake()
    {
        Settings.Init();
    }
}
