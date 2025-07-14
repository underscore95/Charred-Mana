using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyDisplay : MonoBehaviour
{
    [SerializeField] private bool _includeCurrencyName = false;
    [SerializeField] private StringUtils.NumberFormat _format = StringUtils.NumberFormat.COMMA_SEPERATED;
    [SerializeField] private CurrencyType _currencyType;

    private CurrencyManager _currencyManager;
    private CurrencyInfo _currencyInfo;
    private Image _image;
    private TextMeshProUGUI _text;
    private float _amountLastUpdate = float.MinValue;

    private void Start()
    {
        _image = GetComponentInChildren<Image>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _currencyManager = FindAnyObjectByType<CurrencyManager>();
        _currencyInfo = _currencyManager.GetCurrencyInfo(_currencyType);

        _image.sprite = _currencyInfo.Icon;
    }

    private void Update()
    {
        float amount = _currencyManager.Get(_currencyType);
        if (Mathf.Approximately(_amountLastUpdate, amount)) return;

        _text.text = (_includeCurrencyName ? _currencyInfo.Name + ": " : "") + StringUtils.FormatNumber(amount, _format);
        _amountLastUpdate = amount;
    }
}
