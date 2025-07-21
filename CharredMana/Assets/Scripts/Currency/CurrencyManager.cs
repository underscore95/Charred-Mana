using UnityEngine;
using UnityEngine.Assertions;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField] private SerializableEnumDictionary<CurrencyType, CurrencyInfo> _currencyInfo;

    private EnumDictionary<CurrencyType, CurrencyInfo> _currencyInfoDeserialized;
    private SaveManager _saveManager;
    private UiSprites _uiSprites;

    private EnumDictionary<CurrencyType, float> Currencies
    {
        get
        {
            Assert.IsNotNull(_saveManager, "Save file was accessed it was after destroyed");
            Assert.IsNotNull(_saveManager.Save, "Save file was accessed before it was loaded");
            return _saveManager.Save.Currencies;
        }
    }

    private void Awake()
    {
        _uiSprites = FindAnyObjectByType<UiSprites>();
        _saveManager = FindAnyObjectByType<SaveManager>();
        Assert.IsNotNull(_saveManager, "Save manager doesn't exist");

        _currencyInfoDeserialized = _currencyInfo.ToEnumDictionary();
        SetCurrencySpriteTags();

        _saveManager.RunAfterSaveLoaded(() =>
        {
            foreach (var (type, info) in _currencyInfoDeserialized)
            {
                if (!Currencies.ContainsKey(type))
                {
                    Currencies[type] = info.StartingAmount;
                }
            }
        });
    }

    private void SetCurrencySpriteTags()
    {
        foreach (var (type, info) in _currencyInfoDeserialized)
        {
            info.IconSpriteTag = _uiSprites.CreateSpriteTag(info.Icon);
            Assert.IsTrue(_uiSprites.IsValidTag(info.IconSpriteTag), $"Currency {type} has invalid sprite tag '{info.IconSpriteTag}'");
        }
    }

    public CurrencyInfo GetCurrencyInfo(CurrencyType currencyType)
    {
        return _currencyInfoDeserialized[currencyType];
    }

    public float Get(CurrencyType currencyType)
    {
        return Currencies[currencyType];
    }

    public void Set(CurrencyType currencyType, float amount)
    {
        if (amount < 0)
        {
            amount = 0;
            Debug.LogWarning($"Attempted to set {currencyType} to {amount}, had to clamp to 0.");
        }
        Currencies[currencyType] = amount;
    }

    public void Add(CurrencyType currencyType, float amount)
    {
        Set(currencyType, Get(currencyType) + amount);
    }

    public void Remove(CurrencyType currencyType, float amount)
    {
        Add(currencyType, -amount);
    }

    public bool Has(CurrencyType currencyType, float amount)
    {
        return Get(currencyType) >= amount;
    }
}
