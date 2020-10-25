using System;
using System.Collections.Generic;
using UnityEditor;
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
        private GridCell[,]  _gridCells;
    


        

        public List<GridCell> FindPath(GridCell start, GridCell target)//returns list of grid cells from start to target
        {


            _gridCells = cloneGrid(GridManager.Instance.GridCells);
            GridManager.Instance.ResetPathfindData(_gridCells);
            _tempPath = new List<GridCell>();
            _calculatedCells = new List<GridCell>();
            _visitedCells = new List<GridCell>();
            start = _gridCells[start.GridPos.x, start.GridPos.y];
            GridCell currentCell = start;
            
            
            //_visitedCells.Add(start);
          
            int whileKillLimit = 0;
            bool isAnyCellLeft = true;

            if (!GridManager.Instance.IsCellEmpty(target))
            {
                target = GridManager.Instance.GetClosestEmptyPos(target.WorldPos);
            }
            target = _gridCells[target.GridPos.x, target.GridPos.y];
            start.IsVisited = true;

            while (currentCell != target && whileKillLimit < 1000 && isAnyCellLeft)
            {
               List<GridCell> tempNeigbourCells = new List<GridCell>();
               tempNeigbourCells = GetNeighborCells(currentCell);
               if (tempNeigbourCells.Count > 0)
               {
                   CalculateCosts(tempNeigbourCells, target, currentCell);
                   isAnyCellLeft = _calculatedCells.Count > 0;
                   currentCell = FindNextCell();

                   _calculatedCells.Remove(currentCell);
                   currentCell.IsVisited = true;

                   _visitedCells.Add(currentCell);

               }
               else
               {
                   return null;
               }

               whileKillLimit++;
            }
            

            if (currentCell == target)
            {
                return MakePathList(currentCell, start);
               
            }
            else if (_visitedCells.Count > 0)
            {
               
                GridCell closestInVisited = GridManager.Instance.GetClosestEmptyPosFromList(target, _visitedCells);
                return MakePathList(closestInVisited, start);
            }
            
            
            return null;
           
        }

        

        private GridCell[,] cloneGrid(GridCell[,] gridTable)
        {

              _gridCells = new GridCell[gridTable.GetLength(0), gridTable.GetLength(1)];
             foreach (GridCell cell in gridTable)
             {
                 GridCell newCell = cell;
                 _gridCells[cell.GridPos.x, cell.GridPos.y] = newCell;
             }

             return _gridCells;
        }


        private List<GridCell> MakePathList(GridCell lastCell,GridCell start)//when path is found. makes a list starting from last cell and adds its previous cells
                                                                                 //and returns reversed version of this list
        {

            _tempPath = new List<GridCell>();
            
            int whileKillLimit = 0;

            _tempPath.Add(GridManager.Instance.GridCells[lastCell.GridPos.x,lastCell.GridPos.y]);
            while (lastCell != start && whileKillLimit<1000)
            {
               // Debug.Log(lastCell.GridPos + "  -  " +lastCell.PreviousCell.GridPos);
               if (lastCell.PreviousCell != start)
               {
                   _tempPath.Add(GridManager.Instance.GridCells[lastCell.PreviousCell.GridPos.x, lastCell.PreviousCell.GridPos.y]);
               }

               lastCell = lastCell.PreviousCell;
                whileKillLimit++;
               
            }

            _tempPath.Reverse();
            GridManager.Instance.ResetPathfindData(_gridCells);
            return _tempPath;

        }

        private List<GridCell> GetNeighborCells(GridCell centerCell)//find empty cells connected to given cell
        {
            List<GridCell> neighborCells = new List<GridCell>();

            for (int i = -1; i <= +1; i++)
            {
                for (int j = -1; j <= +1; j++)
                {
                    if (!(i == 0 && j == 0))
                    {
                        if (GridManager.Instance.IsCellEmpty(centerCell.GridPos.x + i, centerCell.GridPos.y + j,_gridCells))
                        {
                            neighborCells.Add(_gridCells[centerCell.GridPos.x + i, centerCell.GridPos.y + j]);
                        }
                    }
                }
            }
        
            return neighborCells;
        }

        private void CalculateCosts(List<GridCell> cellList, GridCell target, GridCell currentCell)//calculates path costs of given list of cells
        {
            foreach (GridCell cell in cellList)
            {
                if (!cell.IsVisited)
                {
                 
                    int gCost = CalculateDistance(cell, currentCell) + currentCell.GCost;
                    int hCost = CalculateDistance(cell, target);
                    int totalCost = gCost + hCost;

                    if (totalCost < cell.TotalCost || cell.TotalCost == 0) // updates cell costs if values better.
                    {

                        cell.GCost = gCost;
                        cell.HCost = hCost;
                        cell.TotalCost = totalCost;
                        cell.PreviousCell = currentCell;

                    }

                    if (!cell.IsCalculated)
                    {
                        
                        _calculatedCells.Add(cell);
                        cell.IsCalculated = true;
                    }
                   
                }
            }
        
        }

        private int CalculateDistance(GridCell a, GridCell b)//returns cell to cell distance
        {
            int xDist = Mathf.Abs(a.GridPos.x - b.GridPos.x);
            int yDist = Mathf.Abs(a.GridPos.y - b.GridPos.y);
       
            int diagonalPathCost = Mathf.Min(xDist, yDist) * _diagonalCost;
            int straightPathCost = Mathf.Abs(xDist - yDist) * _straightCost;

            // Debug.Log(xDist+"---"+ yDist+"---"+ straightPathCost + "---"+ diagonalPathCost);
            return diagonalPathCost + straightPathCost;

        }

        private GridCell FindNextCell()//finds next the lowest path cost from calculated cells list
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
