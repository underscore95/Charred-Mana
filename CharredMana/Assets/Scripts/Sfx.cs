
using UnityEngine;
using UnityEngine.Assertions;

public class Sfx : MonoBehaviour
{
    private static Sfx _instance;
    private static readonly float _volume = 1.0f;

    [SerializeField] private AudioClip _click;
    [SerializeField] private AudioClip _playerMove;
    [SerializeField] private AudioClip _spellCast;
    [SerializeField] private AudioClip _playerDamaged;

    private void Awake()
    {
        Assert.IsNull(_instance);
        _instance = this;
    }

    public static void PlayClick()
    {
        Assert.IsNotNull(_instance);
        AudioSource.PlayClipAtPoint(_instance._click, Camera.main.transform.position, _volume);
    }

    public static void PlayPlayerMove()
    {
        Assert.IsNotNull(_instance);
        AudioSource.PlayClipAtPoint(_instance._playerMove, Camera.main.transform.position, _volume);
    }

    public static void PlaySpellCast()
    {
        Assert.IsNotNull(_instance);
        AudioSource.PlayClipAtPoint(_instance._spellCast, Camera.main.transform.position, _volume);
    }

    public static void PlayPlayerDamaged()
    {
        Assert.IsNotNull(_instance);
        AudioSource.PlayClipAtPoint(_instance._playerDamaged, Camera.main.transform.position, _volume);
    }
}