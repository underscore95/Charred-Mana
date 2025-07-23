using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class VersionManager : MonoBehaviour
{
    [Serializable]
    struct PatchPatchVersion
    {
        public int Version;

        public PatchPatchVersion(int Version)
        {
            this.Version = Version;
        }
    }

    [SerializeField] private string _gameName = "Charred Mana";
    [SerializeField] private string _state = "Alpha";
    [SerializeField] private int _majorVersion = 0;
    [SerializeField] private int _minorVersion = 0;
    [SerializeField] private int _patchVersion = 0;
    [SerializeField] private PatchPatchVersion _patchPatchVersion = new(0); // generally unused
    [SerializeField] private TextMeshProUGUI _versionText;

    public string GameName { get { return _gameName; } }

    private void Awake()
    {
        Assert.IsTrue(_majorVersion >= 0);
        Assert.IsTrue(_majorVersion <= 256);
        Assert.IsTrue(_minorVersion >= 0);
        Assert.IsTrue(_minorVersion <= 256);
        Assert.IsTrue(_patchVersion >= 0);
        Assert.IsTrue(_patchVersion <= 256);
        Assert.IsTrue(_patchPatchVersion.Version >= 0);
        Assert.IsTrue(_patchPatchVersion.Version <= 256);

        _versionText.text = $"{_gameName} {GetLongVersion()}";
    }

    private void OnValidate()
    {
        if (_patchPatchVersion.Version != 0)
        {
            Debug.LogWarning($"Patch patch version is {_patchPatchVersion.Version}, generally not recommended to use!");
        }
    }

    public string GetVersion()
    {
        string version = $"v{_majorVersion}.{_minorVersion}.{_patchVersion}";
        if (_patchPatchVersion.Version > 0)
        {
            version += "." + _patchPatchVersion;
        }
        return version;
    }

    public string GetLongVersion()
    {
        return _state + " " + GetVersion();
    }

    public int GetVersionIdentifier()
    {
        int id = _majorVersion << 8;
        id += _minorVersion << 8;
        id += _patchVersion << 8;
        id += _patchPatchVersion.Version << 8;
        return id;
    }
}
