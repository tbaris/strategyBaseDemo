using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class GridManager
{
    private int diagonalCost = 14;
    private int straightCost = 10;

    public static GridManager Instance;

    private int columns;
    private int rows;
    private float gridSize;
    public Sprite cellSprite;

   


    public GridCell[,] gridCells;

    
    public GridManager(int col, int row, float size)
    {
        if (Instance != null && Instance != this)
        {
            return;
        }
        Instance = this;
        
        columns = col;
        rows = row;
        gridSize = size;

        gridCells = new GridCell[columns,rows];
        for (int x = 0; x < gridCells.GetLength(0); x++)
        {
            for (int y = 0; y < gridCells.GetLength(1); y++)
            {
                gridCells[x,y] = new GridCell();
                gridCells[x, y].isEmpty = true;
                gridCells[x, y].gridPos = new Vector2Int(x, y);
                gridCells[x, y].worldPos = getWorldPos(gridCells[x, y]);
                
            }
        }

    }

    public bool isCellEmpty(GridCell cell)
    {
       
            return cell.isEmpty;
        
       
    }

   

    public void setObjectOnPos(GridCell cell,GameObject go)
    {
      
            cell.isEmpty = false;
            cell.gameObjectOnPos = go;
        
    }

    public void setCellsEmpty(int col, int row)
    {
        if (col < columns && col >= 0 && row < rows && col >= 0)
        {
            gridCells[col, row].isEmpty = true;
        }
    }

    public GridCell getCellAdress(Vector3 worldPos)
    {
        if (worldPos.x >= 0 && worldPos.y >= 0 && worldPos.x < columns * gridSize && worldPos.y < rows*gridSize)
        {
            return  gridCells[Mathf.FloorToInt(worldPos.x / gridSize) , Mathf.FloorToInt(worldPos.y / gridSize)];
        }
        else
        {
            return gridCells[0, 0];

        }
    }

    public Vector3 getWorldPos(GridCell a)
    {
        
            return new Vector3(a.gridPos.x*gridSize, a.gridPos.y* gridSize);
       
    }

    public GridCell getClosestEmptyPos(Vector3 target)
    {
        float ClosestCellDistance= Int32.MaxValue;
        GridCell ClosestCell = new GridCell();
        GridCell startingCell = getCellAdress(target);
        foreach (GridCell cell in gridCells)
        {
            if (cell.isEmpty)
            {
                float distance = (cell.worldPos - target).magnitude;
                if (distance < ClosestCellDistance)
                {
                    ClosestCell = cell;
                    ClosestCellDistance = distance;
                }
                
            }

        }
        
        return ClosestCell;


    }

    private int DistanceBetween(GridCell a, GridCell b)
    {
        int xdistance = Mathf.Abs(a.gridPos.x - b.gridPos.x);
        int ydistance = Mathf.Abs(a.gridPos.y - b.gridPos.y);
        int straightPath = Mathf.Abs(xdistance - ydistance);
        int diagolanPath = Mathf.Min(xdistance, ydistance);

        return diagonalCost * diagolanPath + straightPath * straightCost;

    }

}
