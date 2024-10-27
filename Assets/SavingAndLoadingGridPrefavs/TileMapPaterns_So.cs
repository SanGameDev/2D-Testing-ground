using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileMapPaterns_So", menuName = "ScriptableObjects/TileMapPaterns_So", order = 1)]
public class TileMapPaterns_So : ScriptableObject
{
    public TileMapPatern tileMapPaterns;
}

[System.Serializable]
public class TileMapPatern
{
    public string name;
    //public Vector2Int tilemapSize;
    public List<Tiles> tiles;
}

[System.Serializable]
public class Tiles
{
    public TileBase tile;
    public Vector2Int position;
}
