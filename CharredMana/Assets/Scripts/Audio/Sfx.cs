
using UnityEngine;
using UnityEngine.Assertions;

public class Sfx : MonoBehaviour
{
    private static Sfx _instance;

    [SerializeField] private GameObject _soundEffectPrefab;
    [SerializeField] private int _capacity;

    [Header("Sound Effect Clips")]
    [SerializeField] private AudioClip _click;
    [SerializeField] private AudioClip _playerMove;
    [SerializeField] private AudioClip _spellCast;
    [SerializeField] private AudioClip _playerDamaged;
    [SerializeField] private AudioClip _playerLevelUp;

    private static GameSettings _settings;
    private ObjectPool _sounds;

    private void Awake()
    {
        Assert.IsNull(_instance);
        Assert.IsNull(_settings);
        _instance = this;

        // If we play before settings are parsed, mute it
        _settings = new()
        {
            MusicVolume = 0,
            SoundVolume = 0
        };

        Assert.IsTrue(_soundEffectPrefab.TryGetComponent<SoundEffect>(out var _));
        _sounds = new(_soundEffectPrefab, _capacity, transform);
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

    private static void PlaySound(AudioClip clip)
    {
        if (clip == null || Camera.main == null || _instance._sounds.IsFull()) return;

        float volume = _settings.SoundVolume;
        Transform cameraTransform = Camera.main.transform;

        GameObject obj = _instance._sounds.ActivateObject();
        obj.GetComponent<SoundEffect>().Play(clip, cameraTransform, Vector3.zero);
    }

    public static void PlayClick() => PlaySound(_instance._click);
    public static void PlayPlayerMove() => PlaySound(_instance._playerMove);
    public static void PlaySpellCast() => PlaySound(_instance._spellCast);
    public static void PlayPlayerDamaged() => PlaySound(_instance._playerDamaged);
    public static void PlayPlayerLevelUp() => PlaySound(_instance._playerLevelUp);

}