using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class FadeOutCanvasGroup : MonoBehaviour
{
    [SerializeField] private float _stayDuration = 4.0f;
    [SerializeField] private float _fadeDuration = 1.0f;
    [SerializeField] private List<GameObject> _destroyObjectsAfterFading = new();
    [SerializeField] private bool _destroyOwnGameObjectAfterFading = true;
    private float _seconds = 0;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        _seconds += Time.deltaTime;
        if (_seconds < _stayDuration) return;
        float t = Mathf.InverseLerp(_stayDuration, _stayDuration + _fadeDuration, _seconds);
        _canvasGroup.alpha = 1 - t;
        if (t >= 1)
        {
            if (_destroyOwnGameObjectAfterFading) Destroy(gameObject);
            else Destroy(this);

            foreach (GameObject go in _destroyObjectsAfterFading)
            {
                Assert.IsTrue(go != gameObject);
                Assert.IsTrue(go != null);
                Destroy(go);
            }
        }
    }
}
