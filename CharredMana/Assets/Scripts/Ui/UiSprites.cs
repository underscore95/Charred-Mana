using System;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Assertions;

public class UiSprites : MonoBehaviour
{
    [SerializeField] private TextAsset _spriteToIndexMappingJsonFile;
    [SerializeField] private TextAsset _spriteKeyToPathJsonFile;
    [SerializeField] private string _spriteTextureAssetName = "TMP_SpriteTextureAtlas";
    private Dictionary<string, int> _spriteToIndexMapping = null;
    private Dictionary<string, string> _spriteKeyToPathMapping = null;

    private void Awake()
    {
        if (_spriteToIndexMapping == null)
        {
            LoadSpriteIndexMap();
        }
    }

    private void OnDestroy()
    {
        // Save sprite to path to disk
#if UNITY_EDITOR
        if (_spriteKeyToPathMapping != null && _spriteKeyToPathMapping.Count > 0 && _spriteKeyToPathJsonFile != null)
        {
            var serializable = new StringStringDict
            {
                keys = new string[_spriteKeyToPathMapping.Count],
                values = new string[_spriteKeyToPathMapping.Count]
            };

            int i = 0;
            foreach (var kvp in _spriteKeyToPathMapping)
            {
                serializable.keys[i] = kvp.Key;
                serializable.values[i] = kvp.Value;
                i++;
            }

            string json = JsonUtility.ToJson(serializable, true);

            string assetPath = AssetDatabase.GetAssetPath(_spriteKeyToPathJsonFile);
            if (!string.IsNullOrEmpty(assetPath))
            {
                System.IO.File.WriteAllText(assetPath, json);
                AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogError("Could not resolve asset path to write JSON.");
            }
        }
#endif
    }

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

        string path = GetSpritePathRelativeToTextures(sprite);
        int index = _spriteToIndexMapping[path];

        return $"<sprite=\"{_spriteTextureAssetName}\" index={index}>";
    }

    private string GetSpritePathRelativeToTextures(Sprite sprite)
    {
        if (sprite == null)
            return null;

        string spriteKey = $"{sprite.texture.name}_{sprite.textureRect.x}_{sprite.textureRect.y}_{sprite.textureRect.size.x}_{sprite.textureRect.size.y}".ToLower();

#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(sprite).ToLower();
        int startIndex = "assets/textures/".Length;
        Assert.IsTrue(path.StartsWith("assets/textures/"));
        path = path.Substring(startIndex).ToLower();
        _spriteKeyToPathMapping[spriteKey] = path;
        return path;
#else
        if (_spriteKeyToPathMapping.TryGetValue(spriteKey, out string path)) return path;
        return null;
#endif
    }

    public void LoadSpriteIndexMap()
    {
        _spriteToIndexMapping = new();
        _spriteKeyToPathMapping = new();

        // path to index
        if (_spriteToIndexMappingJsonFile == null)
        {
            Debug.LogError("Sprite to index mapping TextAsset is null.");
        }
        else
        {
            var rawIndexMap = JsonUtility.FromJson<StringIntDict>(_spriteToIndexMappingJsonFile.text);
            if (rawIndexMap != null && rawIndexMap.keys != null && rawIndexMap.values != null)
            {
                for (int i = 0; i < rawIndexMap.keys.Length && i < rawIndexMap.values.Length; i++)
                {
                    string normalizedKey = rawIndexMap.keys[i].Replace('\\', '/').ToLower();
                    _spriteToIndexMapping[normalizedKey] = rawIndexMap.values[i];
                }
            }
            else
            {
                Debug.LogError("Failed to parse sprite index mapping JSON.");
            }
        }

        // sprite to path
#if !UNITY_EDITOR
        if (_spriteKeyToPathJsonFile == null)
        {
            Debug.LogError("Sprite key to path mapping TextAsset is null.");
        }
        else
        {
            var rawPathMap = JsonUtility.FromJson<StringStringDict>(_spriteKeyToPathJsonFile.text);
            if (rawPathMap != null && rawPathMap.keys != null && rawPathMap.values != null)
            {
                for (int i = 0; i < rawPathMap.keys.Length && i < rawPathMap.values.Length; i++)
                {
                    string normalizedKey = rawPathMap.keys[i].ToLower();
                    string normalizedPath = rawPathMap.values[i].Replace('\\', '/').ToLower();
                    _spriteKeyToPathMapping[normalizedKey] = normalizedPath;
                }
            }
            else
            {
                Debug.LogError("Failed to parse sprite key to path mapping JSON.");
            }
        }
#endif
    }

    [System.Serializable]
    private class StringStringDict
    {
        public string[] keys;
        public string[] values;
    }

    [System.Serializable]
    private class StringIntDict
    {
        public string[] keys;
        public int[] values;
    }
}
