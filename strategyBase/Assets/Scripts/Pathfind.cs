using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfind : MonoBehaviour
{

    private int _diagonalCost = 14;
    private int _straightCost = 10;
    private List<GridCell> _tempPath;
    private List<GridCell> _calculatedCells;

    public List<GridCell> FindPath(GridCell start, GridCell tartget)
    {


        return null;

    }

    private List<GridCell> GetNeighborCells(GridCell centerCell)
    {
        return null;
    }

    private void CalculateCosts(List<GridCell> cellList,GridCell start, GridCell target)
    {
        foreach (GridCell cell in cellList)
        {

            cell.GCost = CalculateDistance(cell,start);
            cell.HCost = CalculateDistance(cell,target);
            cell.TotalCost = cell.GCost + cell.HCost;
            _calculatedCells.Add(cell);
        }
    }

    private int CalculateDistance(GridCell a, GridCell b)
    {
        int xDist = Mathf.Abs(a.GridPos.x - b.GridPos.x);
        int yDist = Mathf.Abs(a.GridPos.y - b.GridPos.y);
        int straightPath = Mathf.Abs(xDist - yDist);
        int diagonalPathCost = Mathf.Min(xDist, yDist) * _diagonalCost;
        int straightPathCost = straightPath * _straightCost;
        return diagonalPathCost + straightPathCost;
    }

    private GridCell FindNextCell()
    {
        GridCell nextCell = new GridCell();
        nextCell.TotalCost = Int32.MaxValue;
        foreach (GridCell cell in _calculatedCells)
        {
            if (cell.TotalCost < nextCell.TotalCost)
            {
                nextCell = cell;
            }else if (cell.TotalCost == nextCell.TotalCost)
            {
                if(cell.HCost<nextCell.HCost)
                {
                    nextCell = cell;
                }
            }
        }

        return nextCell;
    }









}
