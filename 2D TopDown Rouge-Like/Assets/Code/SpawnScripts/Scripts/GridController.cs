using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{

    [Serializable]
    public struct Grid
    {
        public int collumns, rows;

        public float verticalOffset, horizontalOffset;
    }

    public Room room;
    public Grid grid;
    public GameObject gridTile;
    public List<Vector2> availablePoints = new List<Vector2>();

    private void Awake()
    {
        room = GetComponentInParent<Room>();
        grid.collumns = room.Width - 4;
        grid.rows = room.Height - 4;
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
        if (GetComponentInParent<EnemySpawner>() != null)
        {
            GetComponentInParent<EnemySpawner>().InitializeObjectSpawning();
        }
        else
        {
            Debug.Log("ObjectToRoomSpawner was null");
        }

    }
}
