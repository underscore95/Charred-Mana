using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private float _fadeDuration = 1.5f;
    [SerializeField] private AudioClip[] _music;

    private int _index = 0;
    private AudioSource _source;
    private bool _hasNotPlayedMusic = true;
    private readonly System.Random _random = Utils.CreateRandom();
    private GameSettings _settings;
    private float _secondsSinceFadeInStart;
    private float _secondsSinceFadeOutStart;

    private void Awake()
    {
        _settings = FindAnyObjectByType<SettingsManager>().Settings;
        Assert.IsTrue(_music != null);
        Assert.IsTrue(_music.Length > 0);
        _source = new GameObject("MusicPlayerSource").AddComponent<AudioSource>();
        _source.volume = 0;
        DontDestroyOnLoad(_source);
    }

    private void OnDestroy()
    {
        if (_source != null)
        {
            // can be null if game stopping
            _source.gameObject.AddComponent<MusicDestroyer>().Duration = _fadeDuration;
        }
    }

    private void Update()
    {
        if (_hasNotPlayedMusic && MusicDestroyer.NumberMusicInProcessOfBeingDestroyed <= 0)
        {
            PlayNextMusic();
        }

        _secondsSinceFadeInStart += Time.deltaTime;
        if (_secondsSinceFadeInStart <= _fadeDuration)
        {
            // fading in
            _source.volume = Mathf.InverseLerp(0, _fadeDuration, _secondsSinceFadeInStart) * _settings.MusicVolume;
            return;
        }

        if (_secondsSinceFadeOutStart >= 0)
        {
            // fading out
            _secondsSinceFadeOutStart += Time.deltaTime;
            if ( _secondsSinceFadeOutStart <= _fadeDuration)
            {
                _source.volume = 1 - Mathf.InverseLerp(0, _fadeDuration, _secondsSinceFadeOutStart) * _settings.MusicVolume;
                return;
            }
        } else
        {
            _source.volume = _settings.MusicVolume;
        }
    }

    public void PlayNextMusic()
    {
        StartCoroutine(FadeOutMusicThenStartNext());
    }

    private IEnumerator FadeOutMusicThenStartNext()
    {
        // fade out
        if (!_hasNotPlayedMusic)
        {
            _secondsSinceFadeOutStart = 0;
            yield return new WaitForSecondsRealtime(_fadeDuration);
        }

        // next track
        int previousIndex = _index;
        _index = _random.Next(_music.Length);
        if (_index == previousIndex && !_hasNotPlayedMusic) _index = (_index + 1) % _music.Length; // don't play same track twice unless only one track
        _hasNotPlayedMusic = false;

        // play
        _source.clip = _music[_index];
        _source.Play();
        _secondsSinceFadeInStart = 0;
        _secondsSinceFadeOutStart = -(_music[_index].length - _fadeDuration);
    }
}