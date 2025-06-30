using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] _music;
    private int _index = 0;
    private AudioSource _source;
    private bool _isFirstTrack = true;
    private readonly System.Random _random = Utils.CreateRandom();
    private GameSettings _settings;

    private void Awake()
    {
        _settings = FindAnyObjectByType<SettingsManager>().Settings;
        Assert.IsTrue(_music != null);
        Assert.IsTrue(_music.Length > 0);
        _source = GetComponent<AudioSource>();
        StartCoroutine(PlayMusic());
    }

    private void OnEnable()
    {
        _source.volume = 0;
    }

    private void Update()
    {
        _source.volume = _settings.MusicVolume;
    }

    private IEnumerator PlayMusic()
    {
        while (enabled)
        {
            // next track
            int previousIndex = _index;
            _index = _random.Next(_music.Length);
            if (_index == previousIndex && !_isFirstTrack) _index = (_index + 1) % _music.Length; // don't play same track twice unless only one track
            _isFirstTrack = false;

            // play
            _source.clip = _music[_index];
            _source.Play();
            yield return new WaitForSecondsRealtime(_music[_index].length);
        }
    }
}