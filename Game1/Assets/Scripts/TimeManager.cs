using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private Spline _enemy;
    private int _minutes = 0;
    private int _seconds = 0;
    public float TotalSeconds { get; private set; } = 0;

    private void Awake()
    {
        UpdateText();
    }

    private void Update()
    {
        IncrementTime();
    }

    private void IncrementTime()
    {
        int old = (int)TotalSeconds;
        TotalSeconds += Time.deltaTime;
        if (old < (int)TotalSeconds)
        {
            // second changed
            _seconds++;
            for (; _seconds >= 60; _seconds -= 60)
            {
                _minutes++;
            }

            UpdateText();
        }
    }

    private void UpdateText()
    {
        StringBuilder sb = new();
        if (_minutes < 10) sb.Append("0");
        sb.Append(_minutes);
        sb.Append(":");
        if (_seconds < 10) sb.Append("0");
        sb.Append(_seconds);

        _timeText.text = sb.ToString();
    }
}
