using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TargetParticle : MonoBehaviour
{
    struct Circle
    {
        public OutlineCircle OutlineCircle;
        public SpriteRenderer Renderer;
    }

    [SerializeField] private float _duration = 1.0f;
    [SerializeField] private float _minRadiusSmallestCircle = 0.01f;
    [SerializeField] private float _maxRadiusSmallestCircle = 2.0f;
    [SerializeField] private float _circleMargin = 0.2f;
    [SerializeField] private Color _color = Color.white;
    private float _secondsElapsed = 0.0f;
    private  readonly List<Circle> _circles = new();
    private ObjectPoolRef _pool;

    private void Awake()
    {
        _pool= GetComponent<ObjectPoolRef>();
        Assert.IsNotNull(_pool);

        foreach (Transform child in transform)
        {
            Circle circle = new()
            {
                OutlineCircle = child.gameObject.GetComponent<OutlineCircle>(),
                Renderer = child.gameObject.GetComponent<SpriteRenderer>()
            };
            circle.Renderer.color = _color;

            _circles.Add(circle);
        }
    }

    private void OnEnable()
    {
        _secondsElapsed = 0;
        foreach (var c in _circles)
        {
            c.Renderer.color = new(1,1,1,0);
        }
    }

    private void Update()
    {
        _secondsElapsed += Time.deltaTime;
        if (_secondsElapsed >= _duration)
        {
            _pool.Pool.ReleaseObject(gameObject);
            return;
        }

        float t = Mathf.InverseLerp(0.0f, _duration, _secondsElapsed);
        float scale = Mathf.Lerp(_minRadiusSmallestCircle, _maxRadiusSmallestCircle, t);
        float alpha = 1 - t;
        foreach (var c in _circles)
        {
            c.OutlineCircle.transform.localScale = Vector3.one * scale;
            scale += _circleMargin * t;

            var color = c.Renderer.color;
            color.a = alpha;
            c.Renderer.color = color;
        }
    }
}
