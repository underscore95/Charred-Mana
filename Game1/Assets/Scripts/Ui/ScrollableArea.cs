using UnityEngine;
using UnityEngine.UI;

public class ScrollableArea : MonoBehaviour
{
    [SerializeField] private Scrollbar _scrollbar;
    [SerializeField] private Vector2 _itemOrigin = new(16, -16); // What position does the first item have?
    [SerializeField] private float _padding = 10;
    [SerializeField] private bool _hideScrollBarIfUnused = true; // if scroll bar isn't needed, hide it
    private RectTransform _rect;
    private float _contentsHeightLastFrame = -1;
    private float _contentsHeightLastFrameNoSubtractingRect = -1;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();

        if (_itemOrigin.x < 0) Debug.LogWarningFormat("ScrollableArea ({0}) item origin x is {1} but recommended to >= 0", _itemOrigin.x);
        if (_itemOrigin.y > 0) Debug.LogWarningFormat("ScrollableArea ({0}) item origin x is {1} but recommended to <= 0", _itemOrigin.x);
    }

    private void Update()
    {
        HandleChildrenPositions();

        _scrollbar.size = Mathf.Max(0.2f, _rect.sizeDelta.y / _contentsHeightLastFrameNoSubtractingRect);

        // if scroll bar isn't needed, hide it
        if (_hideScrollBarIfUnused)
        {
            if (_scrollbar.gameObject.activeInHierarchy && _scrollbar.size >= 0.99f)
            {
                _scrollbar.gameObject.SetActive(false);
                _rect.sizeDelta -= Vector2.right * _scrollbar.GetComponent<RectTransform>().sizeDelta.y;
            }
            else if (!_scrollbar.gameObject.activeInHierarchy && _scrollbar.size < 0.99f)
            {
                _scrollbar.gameObject.SetActive(true);
                _rect.sizeDelta += Vector2.right * _scrollbar.GetComponent<RectTransform>().sizeDelta.y;
            }
        }

        if (!RectTransformUtility.RectangleContainsScreenPoint(_rect, Input.mousePosition)) return;
        _scrollbar.value -= 0.1f * Input.mouseScrollDelta.y;
        _scrollbar.value = Mathf.Clamp01(_scrollbar.value);
    }

    private void HandleChildrenPositions()
    {
        float scrollAmount = _contentsHeightLastFrame < 0 ? 0 : _contentsHeightLastFrame * _scrollbar.value;
        Vector3 pos = new(_itemOrigin.x, _itemOrigin.y, 0);
        foreach (Transform t in transform)
        {
            if (t is not RectTransform rect)
            {
                Debug.LogWarningFormat("Non-rect transform was a child of scrollable area: {} ({})", t, t.gameObject.name);
                continue;
            }

            rect.localPosition = pos
                + scrollAmount * Vector3.up
                + 0.5f * new Vector3(-_rect.sizeDelta.x, _rect.sizeDelta.y, 0)
                + rect.sizeDelta.x * 0.5f * Vector3.right
               + rect.sizeDelta.y * 0.5f * Vector3.down;
            pos.y -= _padding;
            pos.y -= rect.sizeDelta.y;
        }

        _contentsHeightLastFrameNoSubtractingRect = Mathf.Abs(pos.y);
        pos.y = Mathf.Max(0, _contentsHeightLastFrameNoSubtractingRect - _rect.sizeDelta.y);
        bool contentsHeightChanged = !Mathf.Approximately(pos.y, _contentsHeightLastFrame);
        _contentsHeightLastFrame = pos.y;
        if (contentsHeightChanged && _scrollbar.value > 0) HandleChildrenPositions(); // Contents height changed, need to redo the positions
    }
}
