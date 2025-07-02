using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Biome : MonoBehaviour
{
    [SerializeField] private BiomeType _type;
    public BiomeType Type { get { return _type; } }


    [SerializeField] private List<TileBase> _decoration = new();
    public List<TileBase> Decoration { get { return _decoration; } }

    [SerializeField] private List<TileBase> _collisionDecorations = new();
    public List<TileBase> CollisionDecorations { get { return _collisionDecorations; } }

    [SerializeField] private TileBase _terrainTile;
    public TileBase Terrain { get { return _terrainTile; } }
}