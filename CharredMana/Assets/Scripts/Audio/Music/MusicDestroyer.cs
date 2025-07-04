using UnityEngine;
using UnityEngine.Assertions;

public class MusicDestroyer : MonoBehaviour
{
    public static int NumberMusicInProcessOfBeingDestroyed = 0;

    public float Duration = -1;
    private float _secondsElapsed = 0;
    private AudioSource _source;
    private GameSettings _settings;

    private void Start()
    {
        if (Duration == 0)
        {
            Destroy(gameObject);
            return;
        }

        NumberMusicInProcessOfBeingDestroyed++;
        _source = GetComponent<AudioSource>();
        _settings = FindAnyObjectByType<SettingsManager>().Settings;
        Assert.IsTrue(Duration > 0);
    }

    private void OnDestroy()
    {
        if (Duration != 0)
        {
            NumberMusicInProcessOfBeingDestroyed--;
        }
    }

    private void Update()
    {
        _secondsElapsed += Time.deltaTime;
        _source.volume = Mathf.InverseLerp(0, Duration, _secondsElapsed) * _settings.MusicVolume;

        if (_secondsElapsed > Duration)
        {
            Destroy(gameObject);
        }
    }
}
