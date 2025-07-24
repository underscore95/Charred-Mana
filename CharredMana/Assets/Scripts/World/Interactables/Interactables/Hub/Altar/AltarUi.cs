using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class AltarUi : MonoBehaviour
{
    [SerializeField] private GameObject _altarCardPrefab;
    [SerializeField] private float _paddingBetweenCards = 25;
    [SerializeField] private Vector3 _cardCenter = Vector3.zero;
    [SerializeField] private InputActionReference _scrollInput;
    [SerializeField] private InputActionReference _closeUiInput;
    [SerializeField] private string _altarCardDesc = "Permanently increases your {0} by {1} ({2} -> {3})"; // 0 = stat name, 1 = amount, 2 = current, 3 = new)

    private readonly EnumDictionary<StatType, AltarCardUi> _altarCards = new();
    private readonly List<StatType> _statTypesWithPrayers = new();

    private SpriteBar _navBar;
    private StatTypes _statTypes;
    private AltarPrayerManager _altarPrayerManager;
    private CurrencyManager _currencyManager;
    private int _currentCard = 0;
    private float _padding;

    private void Start()
    {
        _navBar = GetComponentInChildren<SpriteBar>();
        _statTypes = FindAnyObjectByType<StatTypes>();
        _altarPrayerManager = FindAnyObjectByType<AltarPrayerManager>();
        _currencyManager = FindAnyObjectByType<CurrencyManager>();

        foreach (var (type, _) in _statTypes.AltarStatInfo)
        {
            _statTypesWithPrayers.Add(type);

            AltarCardUi card = Instantiate(_altarCardPrefab, transform).GetComponent<AltarCardUi>();
            _altarCards.Add(type, card);
        }
        UpdateCardsText();

        _navBar.SetMax(_statTypesWithPrayers.Count);

        _padding = _altarCards.First().Value.GetComponent<RectTransform>().sizeDelta.x + _paddingBetweenCards;

        UpdateCards();
    }

    private void UpdateCardsText()
    {
        foreach (var (type, info) in _statTypes.AltarStatInfo)
        {
            AltarCardUi card = _altarCards[type];
            int currentLevel = _altarPrayerManager.GetPrayerLevel(type);
            StatModifier currentModifier = StatModifier.MergeN(info.StatBoostPerPrayerLevel, currentLevel);
            StatModifier nextModifier = new StatModifier(currentModifier);
            nextModifier.Merge(info.StatBoostPerPrayerLevel);

            string desc = string.Format(
               _altarCardDesc,
              StatTypes.ToString(type),
              info.StatBoostPerPrayerLevel.ToString(1, true),
              currentModifier.ToString(1, true),
              nextModifier.ToString(1, true)
           );

            card.SetStat(
           "Prayer of\n" + info.Title,
              desc,
               info.SplashImage,
                () => OnBuy(type)
            );
            UpdateCost(card, type);
        }
    }

    private void Update()
    {
        if (_closeUiInput.action.WasPerformedThisFrame())
        {
            gameObject.SetActive(false);
        }

        var val = _scrollInput.action.ReadValue<Vector2>();
        if (val.y > 0) SwitchCardRight();
        else if (val.y < 0) SwitchCardLeft();
    }

    private void OnEnable()
    {
        UIState.IsSpellSelectUiOpen = true;
    }

    private void OnDisable()
    {
        UIState.IsSpellSelectUiOpen = false;
    }

    // Player clicked buy button of stat (won't be called if player doesn't have enough currency)
    private void OnBuy(StatType type)
    {
        AltarCardUi card = _altarCards[type];

        int currentLevel = _altarPrayerManager.GetPrayerLevel(type);
        int cost = GetCost(currentLevel + 1);
        _currencyManager.Remove(CurrencyType.Essence, cost);
        _altarPrayerManager.UpgradePrayerLevel(type);

        UpdateCardsText();
    }

    // Get cost of upgrading to next level
    // nextLevel >= 1
    private int GetCost(int nextLevel)
    {
        return 2 + nextLevel + nextLevel / 5;
    }

    private void UpdateCost(AltarCardUi card, StatType type)
    {
        int currentLevel = _altarPrayerManager.GetPrayerLevel(type);
        int cost = GetCost(currentLevel + 1);
        card.SetCost(cost, currentLevel);
    }

    public void SwitchCardRight()
    {
        _currentCard++;
        UpdateCards();
    }

    public void SwitchCardLeft()
    {
        _currentCard--;
        UpdateCards();
    }

    private void UpdateCards()
    {
        while (_currentCard < 0) _currentCard += _statTypesWithPrayers.Count;
        _currentCard %= _statTypesWithPrayers.Count;

        _navBar.SetCurrent(_currentCard);

        for (int i = 0; i < _statTypesWithPrayers.Count; ++i)
        {
            var card = GetCard(i);
            card.SetDisplayState(AltarCardUi.DisplayState.HIDDEN);
            int offsetFromCenter = i - _currentCard;

            // make it wrap
            if (_currentCard == 0 && i == _statTypesWithPrayers.Count - 1) offsetFromCenter = -1;
            else if (_currentCard == _statTypesWithPrayers.Count - 1 && i == 0) offsetFromCenter = 1;

            // set position
            card.transform.localPosition = _cardCenter + Vector3.right * (offsetFromCenter * _padding);
        }

        GetCard(_currentCard - 1).SetDisplayState(AltarCardUi.DisplayState.PARTIALLY_TRANSPARENT);
        // GetCard(_currentCard - 1).SetDisplayState(AltarCardUi.DisplayState.VISIBLE);
        GetCard(_currentCard).SetDisplayState(AltarCardUi.DisplayState.VISIBLE);
        // GetCard(_currentCard + 1).SetDisplayState(AltarCardUi.DisplayState.VISIBLE);
        GetCard(_currentCard + 1).SetDisplayState(AltarCardUi.DisplayState.PARTIALLY_TRANSPARENT);
    }

    private AltarCardUi GetCard(int cardPos)
    {
        while (cardPos < 0) cardPos += _statTypesWithPrayers.Count;
        StatType type = _statTypesWithPrayers[cardPos % _statTypesWithPrayers.Count];
        return _altarCards[type];
    }
}
