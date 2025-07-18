using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class SpriteBar : MonoBehaviour
{
    [SerializeField] private int _max = 10;
    [SerializeField] private int _current = 0;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private Color _activeColor = Color.white;
    [SerializeField] private Color _color = Color.white;
    [SerializeField] private Vector2 _size = Vector2.one * 25;
    [SerializeField] private float _padding = 5;

    private readonly List<Image> _images = new();

    private void Awake()
    {
        SetMax(_max);
    }

    public void SetMax(int max)
    {
        Assert.IsTrue(max >= 0);
        _max = max;

        // add new images if we don't have enough
        while (_images.Count < _max)
        {
            var imageObj = new GameObject();
            var image = imageObj.AddComponent<Image>();
            image.sprite = _sprite;
            image.color = _color;

            var rectTransform = image.GetComponent<RectTransform>();
            rectTransform.SetParent(transform, false);
            rectTransform.sizeDelta = _size;

            _images.Add(image);
        }

        // remove images if we have too many
        if (_images.Count > _max)
        {
            int start = _max;
            int count = _images.Count - start;

            for (int i = start; i < _images.Count; i++)
                Destroy(_images[i].gameObject);

            _images.RemoveRange(start, count);
        }

        // fix positions
        float spacing = _size.x + _padding;
        float startOffset = (_images.Count - 1) / 2f * -spacing;
        float x = startOffset;
        foreach (Image image in _images)
        {
            image.transform.localPosition = Vector3.right * x;
            x += spacing;
        }

        if (_current >= _max) _current = _max - 1;
        SetCurrent(_current);
    }

    public void SetCurrent(int current)
    {
        Assert.IsTrue(current >= 0);
        Assert.IsTrue(current <= _images.Count);
        Assert.IsTrue(_max == _images.Count);

        if (_current >= 0 && _current < _images.Count)
        {
            _images[_current].sprite = _sprite;
            _images[_current].color = _color;
        }

        _current = current;
        _images[_current].sprite = _activeSprite;
        _images[_current].color = _activeColor;
    }
}
