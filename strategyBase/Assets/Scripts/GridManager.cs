using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class GridManager
    {

        public static GridManager Instance;

        private int _columns;
        private int _rows;
        private float _gridSize;
        private int _borderLine;
        [SerializeField] private GameObject grassTile;

        public GridCell[,] GridCells;

        public Vector2 gridUpRightCorner { get; private set; }


        public GridManager(int col, int row, float size, int borderLine)
        {
            if (Instance != null && Instance != this)
            {
                return;
            }
            Instance = this;

            _columns = col + borderLine * 2;
            _rows = row + borderLine * 2;
            _gridSize = size;
            _borderLine = borderLine;
            gridUpRightCorner = new Vector2(_rows * _gridSize, _columns * _gridSize);
            GridCells = new GridCell[_columns, _rows];
            for (int x = 0; x < GridCells.GetLength(0); x++)
            {
                for (int y = 0; y < GridCells.GetLength(1); y++)
                {
                    if (x >= _borderLine && x < GridCells.GetLength(0) - _borderLine && y >= borderLine && y < GridCells.GetLength(1) - borderLine)
                    {
                        GridCells[x, y] = new GridCell(true);

                    }
                    else
                    {
                        GridCells[x, y] = new GridCell(false);
                    }

                    GridCells[x, y].IsEmpty = true;
                    GridCells[x, y].GridPos = new Vector2Int(x, y);
                    GridCells[x, y].WorldPos = GetWorldPos(GridCells[x, y]);

                }
            }

        }

        public bool IsCellEmpty(GridCell cell)
        {
            return cell.IsGround && cell.IsEmpty;
        }
        public bool IsCellEmpty(int x, int y, GridCell[,] gridTable)
        {
            if (x >= 0 && x < _columns && y >= 0 && y < _rows)
            {
                return gridTable[x, y].IsEmpty && gridTable[x, y].IsGround;
            }
            else
            {
                return false;
            }


        }
        public bool IsCellEmpty(int x, int y)
        {
            if (x >= 0 && x < _columns && y >= 0 && y < _rows)
            {
                return GridCells[x, y].IsEmpty && GridCells[x, y].IsGround;
            }
            else
            {
                return false;
            }


        }



        public void setObjectOnPos(GameObject go)
        {

            if (go.GetComponent<PlayableObject>())
            {

                Vector2Int bSize = go.GetComponent<PlayableObject>().BaseSize;
                for (int i = 0; i < bSize.x; i++)
                {
                    for (int j = 0; j < bSize.y; j++)
                    {
                        GridCell cellAtCoord = GetCellAdress(go.transform.position);
                        cellAtCoord = GridManager.Instance.GridCells[cellAtCoord.GridPos.x + i, cellAtCoord.GridPos.y + j];
                        cellAtCoord.GameObjectOnPos = go.gameObject;
                        cellAtCoord.IsEmpty = false;


                    }
                }
            }

        }

        public bool canMoveObjectToCell(GameObject go, GridCell from, GridCell to)
        {
            if (IsCellEmpty(to))
            {
                setObjectOnCell(go, to);
                removeObjectOnCell(go, from);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void setObjectOnCell(GameObject go, GridCell cell)
        {

            if (go.GetComponent<PlayableObject>())
            {

                Vector2Int bSize = go.GetComponent<PlayableObject>().BaseSize;
                for (int i = 0; i < bSize.x; i++)
                {
                    for (int j = 0; j < bSize.y; j++)
                    {
                        GridCell cellAtCoord = cell;
                        cellAtCoord =
                            GridManager.Instance.GridCells[cellAtCoord.GridPos.x + i, cellAtCoord.GridPos.y + j];
                        cellAtCoord.GameObjectOnPos = go.gameObject;
                        cellAtCoord.IsEmpty = false;


                    }
                }
            }


        }
        public void removeObjectOnPos(GameObject go)
        {

            if (go.GetComponent<PlayableObject>())
            {

                Vector2Int bSize = go.GetComponent<PlayableObject>().BaseSize;
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
        public void removeObjectOnCell(GameObject go, GridCell cell)
        {

            if (go.GetComponent<PlayableObject>())
            {

                Vector2Int bSize = go.GetComponent<PlayableObject>().BaseSize;
                for (int i = 0; i < bSize.x; i++)
                {
                    for (int j = 0; j < bSize.y; j++)
                    {
                        GridCell cellAtCoord = cell;
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
            if (worldPos.x >= 0 && worldPos.y >= 0 && worldPos.x < (_columns * _gridSize) && worldPos.y < (_rows * _gridSize))
            {

                return GridCells[Mathf.FloorToInt(worldPos.x / _gridSize), Mathf.FloorToInt(worldPos.y / _gridSize)];
            }
            else
            {

                return GridCells[0, 0];

            }
        }

        public Vector3 GetWorldPos(GridCell a)
        {

            return new Vector3(a.GridPos.x * _gridSize, a.GridPos.y * _gridSize);

        }
        public Vector3 GetWorldPos(Vector2Int coorsOfCell)
        {

            return new Vector3(coorsOfCell.x * _gridSize, coorsOfCell.y * _gridSize);

        }

        public GridCell GetClosestEmptyPos(Vector3 target)
        {
            float closestCellDistance = Int32.MaxValue;
            GridCell closestCell = new GridCell(true);

            foreach (GridCell cell in GridCells)
            {
                if (IsCellEmpty(cell))
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
        public GridCell GetClosestEmptyPosFromList(GridCell target, List<GridCell> cellList)
        {
            float closestCellDistance = Int32.MaxValue;
            GridCell closestCell = cellList[0];

            foreach (GridCell cell in cellList)
            {
                if (IsCellEmpty(cell))
                {
                    float distance = (cell.WorldPos - target.WorldPos).magnitude;
                    if (distance < closestCellDistance && cell.GridPos != Vector2Int.zero)
                    {
                        closestCell = cell;
                        closestCellDistance = distance;

                    }

                }

            }

            if (IsCellEmpty(closestCell))
            {
                return closestCell;
            }

            return cellList[0];


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
        public void ResetPathfindData(GridCell[,] gridTable)
        {
            foreach (GridCell cell in gridTable)
            {
                cell.GCost = 0;
                cell.HCost = 0;
                cell.TotalCost = 0;
                cell.PreviousCell = null;
                cell.IsVisited = false;
                cell.IsCalculated = false;
            }
        }

    }
}
