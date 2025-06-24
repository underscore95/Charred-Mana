using System.IO;
using UnityEngine;

public static class JsonUtils
{
    // Load a file containing json and parse it into T
    // filePath should be relative to Application.persistentDataPath
    // may return null if the file doesn't exist, json is invalid, etc
    public static T Load<T>(string filePath, bool warnings = false)
    {
        filePath = Path.Combine(Application.persistentDataPath, filePath);

        if (!File.Exists(filePath))
        {
            if (warnings) Debug.LogWarning($"File not found: {filePath}");
            return default;
        }

        try
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<T>(json);
        }
        catch (System.Exception ex)
        {
            if (warnings) Debug.LogWarning($"Failed to load/parse JSON from {filePath}: {ex.Message}");
            return default;
        }
    }

    // Write data to a file
    // Stored in json
    // Creates file if it doesn't exist
    public static void Save<T>(string filePath, T data)
    {
        filePath = Path.Combine(Application.persistentDataPath, filePath);
        string directory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
    }
}
