
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WorldGenSettings
{
    [SerializeField] private WorldBiomesType _biomes;
    public IWorldBiomes WorldBiomes { get { return WorldBiomesRegistry.Get(_biomes); } }
}