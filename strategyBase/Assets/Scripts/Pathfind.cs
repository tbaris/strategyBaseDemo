﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Pathfind : MonoBehaviour
    {

        private int _diagonalCost = 14;
        private int _straightCost = 10;
        private List<GridCell> _tempPath;
        private List<GridCell> _calculatedCells;
        private List<GridCell> _visitedCells;
    

        public List<GridCell> FindPath(GridCell start, GridCell target)
        {
            _tempPath = new List<GridCell>();
            _calculatedCells = new List<GridCell>();
            _visitedCells = new List<GridCell>();
            GridCell currentCell = start;
            _visitedCells.Add(start);
            int whileKillLimit = 0;
            bool isAnyCellLeft = true;

            

            while (currentCell != target && whileKillLimit < 10000 && isAnyCellLeft)
            {

                CalculateCosts(GetNeighborCells(currentCell),  target, currentCell);
                isAnyCellLeft = _calculatedCells.Count > 0;
                currentCell = FindNextCell();

                _calculatedCells.Remove(currentCell);
                _visitedCells.Add(currentCell);

                whileKillLimit++;
            }
            Debug.Log("No Path found \n" + whileKillLimit +"  "+ _calculatedCells.Count +"  "+ _visitedCells.Count);



            if (currentCell == target)
            {
                return MakePathList(currentCell, start);
            }
            GridManager.Instance.ResetPathfindData();

            return null;
        }


        private List<GridCell> MakePathList(GridCell lastCell,GridCell firstCell)
        {
            _tempPath = new List<GridCell>();
            _tempPath.Add(lastCell);
            int whileKillLimit = 0;
            while (lastCell != firstCell && whileKillLimit<100)
            {

                _tempPath.Add(lastCell.PreviousCell);
                lastCell = lastCell.PreviousCell;
                whileKillLimit++;
            }
            Debug.Log(whileKillLimit);
            _tempPath.Add(lastCell);
            _tempPath.Reverse();
            GridManager.Instance.ResetPathfindData();
            return _tempPath;

        }

        private List<GridCell> GetNeighborCells(GridCell centerCell)
        {
            List<GridCell> neighborCells = new List<GridCell>();

            for (int i = -1; i <= +1; i++)
            {
                for (int j = -1; j <= +1; j++)
                {
                    if (!(i == 0 && j == 0))
                    {
                        if (GridManager.Instance.IsCellEmpty(centerCell.GridPos.x + i, centerCell.GridPos.y + j))
                        {
                            neighborCells.Add(GridManager.Instance.GridCells[centerCell.GridPos.x + i, centerCell.GridPos.y + j]);
                        }
                    }
                }
            }
        
            return neighborCells;
        }

        private void CalculateCosts(List<GridCell> cellList, GridCell target, GridCell currentCell)
        {
            foreach (GridCell cell in cellList)
            {

                int gCost = CalculateDistance(cell, currentCell) + currentCell.GCost;
                int hCost = CalculateDistance(cell,target);
                int totalCost = gCost + hCost;

                if (totalCost < cell.TotalCost || cell.TotalCost == 0)
                {

                    cell.GCost = gCost;
                    cell.HCost = hCost;
                    cell.TotalCost = totalCost;
                    cell.PreviousCell = currentCell;

                }

                if (!_calculatedCells.Contains(cell)&&!_visitedCells.Contains(cell))
                {
                    _calculatedCells.Add(cell);
                    Debug.Log("add" + "  "+ cell.GridPos);
                }
                else
                {
                    Debug.Log("not add" + "  " + cell.GridPos);
                }
            }
        
        }

        private int CalculateDistance(GridCell a, GridCell b)
        {
            int xDist = Mathf.Abs(a.GridPos.x - b.GridPos.x);
            int yDist = Mathf.Abs(a.GridPos.y - b.GridPos.y);
       
            int diagonalPathCost = Mathf.Min(xDist, yDist) * _diagonalCost;
            int straightPathCost = Mathf.Abs(xDist - yDist) * _straightCost;

            // Debug.Log(xDist+"---"+ yDist+"---"+ straightPathCost + "---"+ diagonalPathCost);
            return diagonalPathCost + straightPathCost;

        }

        private GridCell FindNextCell()
        {
            GridCell nextCell = new GridCell(true);
            nextCell.TotalCost = Int32.MaxValue;
            nextCell.HCost = Int32.MaxValue;
            foreach (GridCell testCell in _calculatedCells)
            {

                //  Debug.Log(cell.TotalCost+"+++"+ nextCell.TotalCost);
                if (nextCell.TotalCost > testCell.TotalCost)
                {
               
                    nextCell = testCell ;

                }else if (nextCell.TotalCost == testCell.TotalCost)
                {
                    if(nextCell.HCost > testCell.HCost)
                    {
                   
                        nextCell = testCell;
                    }
                }
            }

            return nextCell;
        }









    }
}
