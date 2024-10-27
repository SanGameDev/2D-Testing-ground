using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateTileRoom : MonoBehaviour
{
    public TileBase floorTile;
    public TileBase doorTile;
    public TileBase wallTile;

    public Tilemap wallsMap;
    public Tilemap floorsMap;
    public Tilemap doorsMap;

    
    private Vector2Int roomCenter;
    public Vector2Int roomPosition;
    public Vector2Int roomSize;
    
    public int pathsWidth;
    public int pathLength;
    
    private void Start() 
    {
        roomCenter = new Vector2Int(roomPosition.x - roomSize.x / 2, roomPosition.y - roomSize.y / 2);
        GenerateRoom();
    }
    
    public void GenerateRoom()
    {
        for (int x = roomCenter.x; x < roomCenter.x + roomSize.x; x++)
        {
            for (int y = roomCenter.y; y < roomCenter.y + roomSize.y; y++)
            {
                if (x == roomCenter.x || x == roomCenter.x + roomSize.x - 1 || y == roomCenter.y || y == roomCenter.y + roomSize.y - 1)
                {
                    wallsMap.SetTile(new Vector3Int(x, y, 0), wallTile);
                }
                else
                {
                    floorsMap.SetTile(new Vector3Int(x, y, 0), floorTile);
                }
            }
        }
    
        // Remove wall tiles and place door tiles based on pathsWidth
        for (int i = 0; i < pathsWidth; i++)
        {
            // Bottom door
            Vector3Int bottomDoorPosition = new Vector3Int(roomCenter.x + roomSize.x / 2 - pathsWidth / 2 + i, roomCenter.y, 0);
            floorsMap.SetTile(bottomDoorPosition, floorTile);
            wallsMap.SetTile(bottomDoorPosition, null);
            doorsMap.SetTile(bottomDoorPosition, doorTile);
            CreatePath(new Vector3Int(bottomDoorPosition.x, bottomDoorPosition.y - 1, 0), Vector3Int.down);
            CreatePathWalls(new Vector3Int(bottomDoorPosition.x, bottomDoorPosition.y - 1, 0), Vector3Int.down);

            // Top door
            Vector3Int topDoorPosition = new Vector3Int(roomCenter.x + roomSize.x / 2 - pathsWidth / 2 + i, roomCenter.y + roomSize.y - 1, 0);
            floorsMap.SetTile(topDoorPosition, floorTile);
            wallsMap.SetTile(topDoorPosition, null);
            doorsMap.SetTile(topDoorPosition, doorTile);
            CreatePath(new Vector3Int(topDoorPosition.x, topDoorPosition.y + 1, 0), Vector3Int.up);
            CreatePathWalls(new Vector3Int(topDoorPosition.x, topDoorPosition.y + 1, 0), Vector3Int.up);

            // Left door
            Vector3Int leftDoorPosition = new Vector3Int(roomCenter.x, roomCenter.y + roomSize.y / 2 - pathsWidth / 2 + i, 0);
            floorsMap.SetTile(leftDoorPosition, floorTile);
            wallsMap.SetTile(leftDoorPosition, null);
            doorsMap.SetTile(leftDoorPosition, doorTile);
            CreatePath(new Vector3Int(leftDoorPosition.x - 1, leftDoorPosition.y, 0), Vector3Int.left);
            CreatePathWalls(new Vector3Int(leftDoorPosition.x - 1, leftDoorPosition.y, 0), Vector3Int.left);

            // Right door
            Vector3Int rightDoorPosition = new Vector3Int(roomCenter.x + roomSize.x - 1, roomCenter.y + roomSize.y / 2 - pathsWidth / 2 + i, 0);
            floorsMap.SetTile(rightDoorPosition, floorTile);
            wallsMap.SetTile(rightDoorPosition, null);
            doorsMap.SetTile(rightDoorPosition, doorTile);
            CreatePath(new Vector3Int(rightDoorPosition.x + 1, rightDoorPosition.y, 0), Vector3Int.right);
            CreatePathWalls(new Vector3Int(rightDoorPosition.x + 1, rightDoorPosition.y + 1, 0), Vector3Int.right);
        }
    }
    
    private void CreatePath(Vector3Int startPosition, Vector3Int direction)
    {
        for (int i = 0; i < pathLength; i++)
        {
            Vector3Int position = startPosition + direction * i;
            floorsMap.SetTile(position, floorTile);
            wallsMap.SetTile(position, null);
        }
    }

    private void CreatePathWalls(Vector3Int startPosition, Vector3Int direction)
    {
        for (int i = 0; i < pathLength; i++)
        {
            Vector3Int position = startPosition + direction * i;
            
            Debug.Log(position);

            Vector3Int leftWallPosition = position + new Vector3Int(-direction.y, direction.x, 0);
            Vector3Int rightWallPosition = position + new Vector3Int(direction.y, -direction.x, 0);

            if (floorsMap.GetTile(leftWallPosition) == null)
            {
                wallsMap.SetTile(leftWallPosition, wallTile);
            }
            if (floorsMap.GetTile(rightWallPosition) == null)
            {
                wallsMap.SetTile(rightWallPosition, wallTile);
            }
        }
    }
}
