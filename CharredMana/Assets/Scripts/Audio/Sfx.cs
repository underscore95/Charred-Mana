
using UnityEngine;
using UnityEngine.Assertions;

public class Sfx : MonoBehaviour
{
    private static Sfx _instance;

    [SerializeField] private AudioClip _click;
    [SerializeField] private AudioClip _playerMove;
    [SerializeField] private AudioClip _spellCast;
    [SerializeField] private AudioClip _playerDamaged;

    private static GameSettings _settings;

    private void Awake()
    {
        Assert.IsNull(_instance);
        Assert.IsNull(_settings);
        _instance = this;
    }

    private void Start()
    {
        _settings = FindAnyObjectByType<SettingsManager>().Settings;
    }

    private void OnDestroy()
    {
        _instance = null;
        _settings = null;
    }

    public static void PlayClick()
    {
        Assert.IsNotNull(_instance);
        AudioSource.PlayClipAtPoint(_instance._click, Camera.main.transform.position, _settings.SoundVolume);
    }

    public static void PlayPlayerMove()
    {
        Assert.IsNotNull(_instance);
        AudioSource.PlayClipAtPoint(_instance._playerMove, Camera.main.transform.position, _settings.SoundVolume);
    }

    public static void PlaySpellCast()
    {
        Assert.IsNotNull(_instance);
        AudioSource.PlayClipAtPoint(_instance._spellCast, Camera.main.transform.position, _settings.SoundVolume);
    }

    public static void PlayPlayerDamaged()
    {
        Assert.IsNotNull(_instance);
        AudioSource.PlayClipAtPoint(_instance._playerDamaged, Camera.main.transform.position, _settings.SoundVolume);
    }
}