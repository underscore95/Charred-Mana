using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingsControls : MonoBehaviour
{
    [SerializeField] private Slider _soundVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;

    private GameSettings _settings;

    private void Awake()
    {
        _settings = FindAnyObjectByType<SettingsManager>().Settings;
    }

    private void Start()
    {
        _soundVolumeSlider.value = _settings.SoundVolume;
        _soundVolumeSlider.onValueChanged.AddListener(v => _settings.SoundVolume = v);

        _musicVolumeSlider.value = _settings.MusicVolume;
        _musicVolumeSlider.onValueChanged.AddListener(v => _settings.MusicVolume = v);
    }
}
