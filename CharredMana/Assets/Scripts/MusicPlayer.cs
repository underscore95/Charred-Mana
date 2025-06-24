using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class MusicPlayer : MonoBehaviour
{
    private static float _volume = 0.75f;
    [SerializeField] private AudioClip[] _music;
    private int _index = 0;
    private AudioSource _source;
    private bool _isFirstTrack = true;
    private readonly System.Random _random = Utils.CreateRandom();

    private void Awake()
    {
        Assert.IsTrue(_music != null);
        Assert.IsTrue(_music.Length > 0);
        _source = GetComponent<AudioSource>();
        StartCoroutine(PlayMusic());
    }

    private void OnEnable()
    {
        _source.volume = _volume;
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

    public static void SetVolume(float volume)
    {
        Assert.IsTrue(volume >= 0);
        Assert.IsTrue(volume <= 1);
        _volume = volume;

        MusicPlayer[] musicPlayers = FindObjectsByType<MusicPlayer>(FindObjectsSortMode.None);
        foreach (MusicPlayer player in musicPlayers)
        {
            player._source.volume = volume;
        }
    }
}