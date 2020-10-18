using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class GridManager
{
    private int _diagonalCost = 14;
    private int _straightCost = 10;

    public static GridManager Instance;

    private int _columns;
    private int _rows;
    private float _gridSize;
    public Sprite CellSprite;

   


    public GridCell[,] GridCells;

    
    public GridManager(int col, int row, float size)
    {
        if (Instance != null && Instance != this)
        {
            return;
        }
        Instance = this;
        
        _columns = col;
        _rows = row;
        _gridSize = size;

        GridCells = new GridCell[_columns,_rows];
        for (int x = 0; x < GridCells.GetLength(0); x++)
        {
            for (int y = 0; y < GridCells.GetLength(1); y++)
            {
                GridCells[x,y] = new GridCell();
                GridCells[x, y].IsEmpty = true;
                GridCells[x, y].GridPos = new Vector2Int(x, y);
                GridCells[x, y].WorldPos = GetWorldPos(GridCells[x, y]);
                
            }
        }

    }

    public bool IsCellEmpty(GridCell cell)
    {
       
            return cell.IsEmpty;
        
       
    }
    public bool IsCellEmpty(int x, int y)
    {
        if (x >= 0 && x < _columns && y >= 0 && y < _rows)
        {
            return GridCells[x, y].IsEmpty;
        }
        else
        {
            return false;
        }

       
    }



    public void setObjectOnPos(GameObject go)
    {
        Debug.Log(go.name);
        if (go.GetComponent<Unit>())
        {
            Debug.Log(go.GetType());
            Vector2Int bSize = go.GetComponent<Unit>().BaseSize;
            for (int i = 0; i < bSize.x; i++)
            {
                for (int j = 0; j < bSize.y; j++)
                {
                    GridCell cellAtCoord = GetCellAdress(go.transform.position);
                    cellAtCoord = GridManager.Instance.GridCells[cellAtCoord.GridPos.x + i, cellAtCoord.GridPos.y + j];
                    cellAtCoord.GameObjectOnPos = go.gameObject;
                    cellAtCoord.IsEmpty = false;
                    Debug.Log(cellAtCoord.GridPos);

                }
            }
        }

    }
    public void removeObjectOnPos(GameObject go)
    {
        Debug.Log(go.name);
        if (go.GetComponent<Unit>())
        {
            Debug.Log(go.GetType());
            Vector2Int bSize = go.GetComponent<Unit>().BaseSize;
            for (int i = 0; i < bSize.x; i++)
            {
                for (int j = 0; j < bSize.y; j++)
                {
                    GridCell cellAtCoord = GetCellAdress(go.transform.position);
                    cellAtCoord = GridManager.Instance.GridCells[cellAtCoord.GridPos.x + i, cellAtCoord.GridPos.y + j];
                    cellAtCoord.GameObjectOnPos = null;
                    cellAtCoord.IsEmpty = true;
                    

                }
            }
        }

    }

    public void SetCellsEmpty(int col, int row)
    {
        if (col < _columns && col >= 0 && row < _rows && row >= 0)
        {
            GridCells[col, row].IsEmpty = true;
        }
    }

    public GridCell GetCellAdress(Vector3 worldPos)
    {
        if (worldPos.x >= 0 && worldPos.y >= 0 && worldPos.x < (_columns * _gridSize) && worldPos.y < (_rows*_gridSize))
        {
           
            return  GridCells[Mathf.FloorToInt(worldPos.x / _gridSize) , Mathf.FloorToInt(worldPos.y / _gridSize)];
        }
        else
        {
            Debug.Log(worldPos.x +"---" +( _columns * _gridSize) + "---" + worldPos.y +"---" +(_rows * _gridSize));
            return GridCells[0, 0];

        }
    }

    public Vector3 GetWorldPos(GridCell a)
    {
        
            return new Vector3(a.GridPos.x*_gridSize, a.GridPos.y* _gridSize);
       
    }

    public GridCell GetClosestEmptyPos(Vector3 target)
    {
        float closestCellDistance= Int32.MaxValue;
        GridCell closestCell = new GridCell();
        GridCell startingCell = GetCellAdress(target);
        foreach (GridCell cell in GridCells)
        {
            if (cell.IsEmpty)
            {
                float distance = (cell.WorldPos - target).magnitude;
                if (distance < closestCellDistance)
                {
                    closestCell = cell;
                    closestCellDistance = distance;
                }
                
            }

        }
        
        return closestCell;


    }

    private int DistanceBetween(GridCell a, GridCell b)
    {
        int xdistance = Mathf.Abs(a.GridPos.x - b.GridPos.x);
        int ydistance = Mathf.Abs(a.GridPos.y - b.GridPos.y);
        int straightPath = Mathf.Abs(xdistance - ydistance);
        int diagolanPath = Mathf.Min(xdistance, ydistance);

        return _diagonalCost * diagolanPath + straightPath * _straightCost;

    }

    public void ResetPathfindData()
    {
        foreach (GridCell cell in GridCells)
        {
            cell.GCost = 0;
            cell.HCost = 0;
            cell.TotalCost = 0;
            cell.PreviousCell = null;
        }
    }

}
