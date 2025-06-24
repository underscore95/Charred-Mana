using UnityEngine;

public class OutlineCircle : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private float _radius;
    public float Radius
    {
        get { return _radius; }
        set
        {
            _radius = value;
            _sprite.material.SetFloat("_CircleRadius", _radius);
        }
    }

    [SerializeField] private float _outlineWidth;
    public float OutlineWidth
    {
        get { return _outlineWidth; }
        set
        {
            _outlineWidth = value;
            _sprite.material.SetFloat("_CircleOutlineWidth", _outlineWidth);
        }
    }

    private void Awake()
    {
        // Send values to GPU
        Radius = Radius;
        OutlineWidth = OutlineWidth;
    }
}
