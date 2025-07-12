using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class StaircaseHud : MonoBehaviour
{
    [SerializeField] private Camera _uiCamera;
    [SerializeField] private bool _rotate = false;
    [SerializeField] private float _borderSize = 50;
    [SerializeField] private Color _onScreenColor = Color.clear;
    [SerializeField] private Color _offScreenColor = new(1, 1, 1, 0.8f);

    private Image _image;
    private RectTransform _arrowRect;
    private void Awake()
    {
        _image = GetComponentInChildren<Image>();
        _arrowRect = _image.GetComponent<RectTransform>();
    }

    private void Update()
    {
        // find closest staircase
        float closestDistance = float.MaxValue;
        Staircase closestStaircase = null;
        foreach (Staircase s in Staircase.StaircasesInWorld)
        {
            float dist = Utils.XYDistanceSquared(s.transform.position, Camera.main.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestStaircase = s;
            }
        }

        // do we have a staircase
        if (closestStaircase == null)
        {
            _image.color = Color.clear;
            return;
        }

        _image.color = Color.white;

        // put it in correct position
        SetHudArrowPosition(_arrowRect, _image, closestStaircase.transform.position, Camera.main.transform.position, Camera.main, _uiCamera, _onScreenColor, _offScreenColor, _borderSize, _rotate);
    }

    // https://www.youtube.com/watch?v=dHzeHh-3bp4
    private static void SetHudArrowPosition(RectTransform hudArrow,
        Image hudArrowIcon,
        Vector2 destination,
        Vector2 currentPosition,
        Camera camera,
        Camera uiCamera,
        Color onScreenColor,
        Color offScreenColor,
        float borderSize,
        bool rotate)
    {
        if (rotate)
        {
            Vector2 dir = (destination - currentPosition).normalized;
            float angle = GetAngleFromVectorFloat(dir);
            hudArrow.rotation = Quaternion.Euler(0, 0, angle);
        }

        Vector2 destScreenPoint = camera.WorldToScreenPoint(destination);
        bool offScreen = destScreenPoint.x <= 0 || destScreenPoint.y <= 0 || destScreenPoint.x >= Screen.width || destScreenPoint.y >= Screen.height;
        hudArrowIcon.color = offScreen ? offScreenColor : onScreenColor;

        if (offScreen)
        {
            Vector2 clamped = destScreenPoint;
            clamped.x = Mathf.Clamp(clamped.x, borderSize, Screen.width - borderSize);
            clamped.y = Mathf.Clamp(clamped.y, borderSize, Screen.height - borderSize);

            hudArrow.position = (Vector2)uiCamera.ScreenToWorldPoint(clamped);

        }
        else
        {
            hudArrow.position = (Vector2)uiCamera.ScreenToWorldPoint(destScreenPoint);
        }
    }

    private static float GetAngleFromVectorFloat(Vector2 normalizedDirection)
    {
        float angle = Mathf.Atan2(normalizedDirection.y, normalizedDirection.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f;
        return angle;
    }
}
