using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RewardColors", menuName = "Scriptable Objects/Rewards/RewardColors")]
public class RewardColors : ScriptableObject
{
    [SerializeField] private SerializableEnumDictionary<Rarity, Color> _rarityColorsSerializable = new();
    private EnumDictionary<Rarity, Color> _rarityToColor;

    private void OnEnable()
    {
        Setup();
    }

    private void OnValidate()
    {
        Setup();
    }

    private void Setup()
    {
        _rarityToColor = _rarityColorsSerializable.ToEnumDictionary();
    }

    public Color GetColor(Rarity rarity)
    {
        return _rarityToColor[rarity];
    }
}
