using UnityEditor.Rendering;
using UnityEngine;

public class GameSettings
{
    public float SoundVolume = 0.5f;
    public float MusicVolume = 0.5f;
}

public class SettingsManager : MonoBehaviour
{
    private static readonly string SETTINGS_FILE = "Settings.json";

    public GameSettings Settings { get; private set; } = new();

    private void Awake()
    {
        Settings = JsonUtils.Load<GameSettings>(SETTINGS_FILE, false);
        Settings ??= new();
    }

    private void OnDestroy()
    {
        SaveSettings();
    }

    public void SaveSettings()
    {
        JsonUtils.Save(SETTINGS_FILE, Settings);
    }
}
