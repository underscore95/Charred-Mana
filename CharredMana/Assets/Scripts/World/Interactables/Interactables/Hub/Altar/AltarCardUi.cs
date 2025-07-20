using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AltarCardUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _descText;
    [SerializeField] private Button _buyButton;
    [SerializeField] private TextMeshProUGUI _buyButtonText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Image _splashImage;
    [SerializeField] private Color _buyButtonTextColor = Color.black;
    [SerializeField] private Color _buyButtonTextColorTooExpensive = Color.darkRed;
    [SerializeField] private float _partiallyTransparentAlpha = 0.6f;
    [SerializeField] private float _partiallyTransparentScale = 0.75f;
    [SerializeField] private string _buyButtonTextContents = "Purchase for {0} {1}";

    private CurrencyManager _currencyManager;
    private int _cost = int.MaxValue;
    private CanvasGroup _canvasGroup;
    private void Awake()
    {
        _currencyManager = FindAnyObjectByType<CurrencyManager>();
        _buyButtonText = _buyButton.GetComponentInChildren<TextMeshProUGUI>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        UpdateCostDisplay();
    }

    public void SetStat(
        string title,
        string desc,
        Sprite splash,
        UnityAction onBuy
        )
    {
        _titleText.text = title;
        _descText.text = desc;
        _splashImage.sprite = splash;
        _buyButton.onClick.RemoveAllListeners();
        _buyButton.onClick.AddListener(onBuy);
    }

    public void SetCost(int cost, int level)
    {
        _levelText.text = "LV" + level;
        _buyButtonText.text = string.Format(_buyButtonTextContents, cost, _currencyManager.GetCurrencyInfo(CurrencyType.Essence).Name);
        _cost = cost;
        UpdateCostDisplay();
    }

    private void UpdateCostDisplay()
    {
        _buyButton.interactable = _cost <= _currencyManager.Get(CurrencyType.Essence);
        Color c = _cost <= _currencyManager.Get(CurrencyType.Essence) ? _buyButtonTextColor : _buyButtonTextColorTooExpensive;
        //  c.a = _buyButtonText.color.a;
        _buyButtonText.color = c;
    }

    public enum DisplayState
    {
        VISIBLE, PARTIALLY_TRANSPARENT, HIDDEN
    }
    public void SetDisplayState(DisplayState displayState)
    {
        transform.localScale = displayState == DisplayState.PARTIALLY_TRANSPARENT ? _partiallyTransparentScale * Vector3.one : Vector3.one;
        switch (displayState)
        {
            case DisplayState.VISIBLE:
                _canvasGroup.interactable = true;
                _canvasGroup.alpha = 1.0f;
                break;
            case DisplayState.PARTIALLY_TRANSPARENT:
                _canvasGroup.interactable = false;
                _canvasGroup.alpha = _partiallyTransparentAlpha;
                break;
            case DisplayState.HIDDEN:
                _canvasGroup.interactable = false;
                _canvasGroup.alpha = 0.0f;
                break;
        }
    }
}
