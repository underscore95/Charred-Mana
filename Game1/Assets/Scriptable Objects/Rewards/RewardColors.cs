using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RewardColors", menuName = "Scriptable Objects/Rewards/RewardColors")]
public class RewardColors : ScriptableObject
{
    [SerializeField] private SerializableEnumDictionary<Rarity, Color> _rarityColorsSerializable = new();
    private EnumDictionary<Rarity, Color> _rarityToColor;
    private static RewardColors _instance;

    private void OnEnable()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogErrorFormat("Multiple RewardColor scriptable objects found, only one should exist");
            return;
        }
        _instance = this;
        _rarityToColor = _rarityColorsSerializable;
    }

    public static Color GetColor(Rarity rarity)
    {
        return _instance._rarityToColor[rarity];
    }
}
