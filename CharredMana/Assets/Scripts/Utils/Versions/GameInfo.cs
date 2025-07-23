using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameInfo", menuName = "Scriptable Objects/Game/Game Info")]
public class GameInfo : ScriptableObject
{
    [Serializable]
    public struct PatchPatchVersion
    {
        public uint Version;

        public PatchPatchVersion(uint version)
        {
            Version = version;
        }
    }

    [SerializeField] private string _gameName = "Charred Mana";
    [SerializeField] private string _state = "Alpha";
    [SerializeField] private uint _majorVersion = 0;
    [SerializeField] private uint _minorVersion = 0;
    [SerializeField] private uint _patchVersion = 0;
    [SerializeField] private PatchPatchVersion _patchPatchVersion = new(0);

    public string GameName => _gameName;

    private void OnEnable()
    {
        ValidateVersionNumbers();
    }

    private void OnValidate()
    {
        if (_patchPatchVersion.Version != 0)
        {
            Debug.LogWarning($"Patch patch version is {_patchPatchVersion.Version}, generally not recommended to use!", this);
        }

        ValidateVersionNumbers();
    }

    private void ValidateVersionNumbers()
    {
        Debug.Assert(_majorVersion >= 0 && _majorVersion <= 256);
        Debug.Assert(_minorVersion >= 0 && _minorVersion <= 256);
        Debug.Assert(_patchVersion >= 0 && _patchVersion <= 256);
        Debug.Assert(_patchPatchVersion.Version >= 0 && _patchPatchVersion.Version <= 256);
    }

    public string GetShortVersion()
    {
        string version = $"v{_majorVersion}.{_minorVersion}.{_patchVersion}";
        if (_patchPatchVersion.Version > 0)
        {
            version += "." + _patchPatchVersion.Version;
        }
        return version;
    }

    public string GetLongVersion()
    {
        return $"{_state} {GetShortVersion()}";
    }

    public uint GetVersionIdentifier()
    {
        return (_majorVersion << 24) |
               (_minorVersion << 16) |
               (_patchVersion << 8) |
               _patchPatchVersion.Version;
    }
}
