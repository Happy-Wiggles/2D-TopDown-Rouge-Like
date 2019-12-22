using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Room room;

    [Serializable]
    public struct Grid
    {
        public int collumns, rows;

        public float verticalOffset, horizontalOffset;
    }

    public Grid grid;

    public GameObject gridTile;

    public List<Vector2> availablePoints = new List<Vector2>();

    private void Awake()
    {
        room = GetComponentInParent<Room>();
        grid.collumns = room.Width - 3;
        grid.rows = room.Height - 3;
        grid.verticalOffset = 8;
        grid.horizontalOffset = 8;
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        grid.verticalOffset += room.transform.localPosition.y;
        grid.horizontalOffset += room.transform.localPosition.x;

        for (var y = 0; y <= grid.rows; y++)
        {
            for (var x = 0; x <= grid.collumns; x++)
            {
                GameObject gridTileGo = Instantiate(gridTile, transform);
                gridTileGo.transform.position = new Vector2(x - (grid.collumns - grid.horizontalOffset), y - (grid.rows - grid.verticalOffset));
                gridTileGo.name = "X: " + x + ", Y: " + y;
                availablePoints.Add(gridTileGo.transform.position);
            }
        }
        if (GetComponentInParent<ObjectToRoomSpawner>() != null)
        {
            GetComponentInParent<ObjectToRoomSpawner>().InitializeObjectSpawning();
        }
        else
        {
            Debug.Log("ObjectToRoomSpawner was null");
        }

    }
}
