using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SaveTileMaps : MonoBehaviour
{
    public bool save;
    public bool loadIn0Position;
    public Tilemap map;
    public TileMapPaterns_So tileMapPaterns_So;

    public Transform roomPosition;

    void Start()
    {
        if (save)
        {
            GetTilesInMap();
        }
        else
        {
            if (loadIn0Position)
            {
                LoadTilesSoInGrid();
            }
            else
            {
                LoadTilesSoInGridSpecificPosition();
            }
        }
    }

    private void GetTilesInMap()
    {
        Tilemap tilemap = map;
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
    
        SaveTilesInSO(allTiles, bounds);
    }
    
    private void SaveTilesInSO(TileBase[] alltiles, BoundsInt bounds)
    {
        // Initialize a list to store the tiles and their positions
        List<Tiles> tiles = new List<Tiles>();
    
        // Loop through the alltiles array
        for (int x = 0; x < alltiles.Length; x++)
        {
            TileBase tile = alltiles[x];
            if (tile != null)
            {
                // Create a Tiles object and set its properties
                Tiles t = new Tiles();
                t.tile = tile;
                int posX = x % bounds.size.x + bounds.xMin;
                int posY = x / bounds.size.x + bounds.yMin;
                t.position = new Vector2Int(posX, posY); // save the position using the position in the grid
                tiles.Add(t);
            }
        }
    
        // Create a TileMapPatern object and set its properties
        TileMapPatern tileMapPatern = new TileMapPatern();
        tileMapPatern.name = "TileMapPatern";
        tileMapPatern.tiles = tiles;
    
        // Add the TileMapPatern to the tileMapPaterns_So list
        tileMapPaterns_So.tileMapPaterns = tileMapPatern;
    }

    private void LoadTilesSoInGrid()
    {
        foreach (Tiles tile in tileMapPaterns_So.tileMapPaterns.tiles)
        {
            map.SetTile(new Vector3Int(tile.position.x, tile.position.y, 0), tile.tile);
        }
    }

    private void LoadTilesSoInGridSpecificPosition()
    {
        // Declare and initialize the 'grid' variable
        Grid grid = FindObjectOfType<Grid>();
    
        // Calculate the position of the room inside the grid
        Vector3Int roomGridPosition = grid.WorldToCell(roomPosition.position);
        Debug.Log("roomGridPosition" + roomGridPosition + " roomPosition" + transform.position);
    
        // Start a foreach loop to go through the tiles in the SO
        foreach (Tiles tile in tileMapPaterns_So.tileMapPaterns.tiles)
        {
            // Use the roomGridPosition to set the position of the tiles in the grid
            Vector3Int gridPosition = new Vector3Int(tile.position.x + roomGridPosition.x, tile.position.y + roomGridPosition.y, 0);
            //Debug.Log("grid position" + gridPosition + " tile position" + tile.position + " room position" + roomGridPosition);
            map.SetTile(gridPosition, tile.tile);
        }
    }

    public void SpawnTilesInGrid(Vector3Int roomPosition, TileMapPaterns_So roomPatern)
    {
        Grid grid = FindObjectOfType<Grid>();
    
        Vector3Int roomGridPosition = grid.WorldToCell(roomPosition);
    
        foreach (Tiles tile in roomPatern.tileMapPaterns.tiles)
        {
            Vector3Int gridPosition = new Vector3Int(tile.position.x + roomGridPosition.x, tile.position.y + roomGridPosition.y, 0);

            map.SetTile(gridPosition, tile.tile);
        }
    }
}

/*List<Tiles> tiles = new List<Tiles>();
        
        for (int x = 0; x < alltiles.Length; x++)
        {
            TileBase tile = alltiles[x];
            if (tile != null)
            {
                Tiles t = new Tiles();
                t.tile = tile;
                t.position = new Vector2Int(x % 10, x / 10);
                tiles.Add(t);
            }
        }
        TileMapPatern tileMapPatern = new TileMapPatern();
        tileMapPatern.name = "TileMapPatern";
        tileMapPatern.tiles = tiles;
        tileMapPaterns_So.tileMapPaterns.Add(tileMapPatern);*/
