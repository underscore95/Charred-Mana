using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

public class UiSprites : MonoBehaviour
{
    [SerializeField] private TextAsset _spriteToIndexMappingJsonFile;
    [SerializeField] private string _spriteTextureAssetName = "TMP_SpriteTextureAtlas";
    private Dictionary<string, int> _spriteToIndexMapping = null;

    public bool IsValidTag(string spriteTag)
    {
        return spriteTag != null && spriteTag.StartsWith("<sprite=");
    }

    public string CreateSpriteTag(Sprite sprite)
    {
        if (_spriteToIndexMapping == null)
        {
            LoadSpriteIndexMap();
        }
        Assert.IsNotNull(sprite, "Attempted to make sprite tag out of null sprite");

        string path = GetSpritePathRelativeToTextures(sprite).ToLower();
        int index = _spriteToIndexMapping[path];

        return $"<sprite=\"{_spriteTextureAssetName}\" index={index}>";
    }

    public static string GetSpritePathRelativeToTextures(Sprite sprite)
    {
        if (sprite == null)
            return null;

        string path = AssetDatabase.GetAssetPath(sprite).ToLower();
        int startIndex = "assets/textures/".Length;
        Assert.IsTrue(path.StartsWith("assets/textures/"));
        return path.Substring(startIndex);
    }

    public void LoadSpriteIndexMap()
    {
        _spriteToIndexMapping = new();

        if (_spriteToIndexMappingJsonFile == null)
        {
            Debug.LogError("Sprite to index mapping TextAsset is null.");
            return;
        }

        var raw = JsonUtility.FromJson<Wrapper>(_spriteToIndexMappingJsonFile.text);
        if (raw != null && raw.keys != null && raw.values != null)
        {
            for (int i = 0; i < raw.keys.Length && i < raw.values.Length; i++)
            {
                string normalizedKey = raw.keys[i].Replace('\\', '/').ToLower();
                _spriteToIndexMapping[normalizedKey] = raw.values[i];
            }
        }
        else
        {
            Debug.LogError("Failed to parse sprite index mapping JSON.");
        }
    }

    // Workaround since JsonUtility doesn't support Dictionary<string, int> directly
    [System.Serializable]
    private class Wrapper
    {
        public string[] keys;
        public int[] values;
    }
}
