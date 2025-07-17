using System.Collections;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    private AudioSource _source;
    private ObjectPoolRef _pool;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        _pool = GetComponent<ObjectPoolRef>();
    }

    public void Play(AudioClip clip, Transform parent, Vector3 offset)
    {
        transform.parent = parent;
        transform.localPosition = offset;
        _source.clip = clip;
        _source.Play();

        StartCoroutine(ReleaseAfterSoundFinishes());
    }

    private IEnumerator ReleaseAfterSoundFinishes()
    {
        yield return new WaitForSecondsRealtime(_source.clip.length);
        _pool.Pool.ReleaseObject(gameObject);
    }
}
