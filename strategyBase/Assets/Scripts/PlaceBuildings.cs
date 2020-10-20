using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlaceBuildings : MonoBehaviour
    {
        public GameObject currentBuilding;
        private Building _currentBuildingInfo;

        public static PlaceBuildings Instance;

        [SerializeField] private PooledObject squareRed;
        private GameObjectPool _squareRedPool;
        private GameObjectPool _squareGreenPool;
        [SerializeField] private PooledObject squareGreen;
        private List<PooledObject> _activeIndicatorSquares;

        private void Awake()
        {
            _squareRedPool = new GameObjectPool(squareRed);
            _squareGreenPool = new GameObjectPool(squareGreen);
            _activeIndicatorSquares =  new List<PooledObject>();
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        void Update()
        {
            if (currentBuilding != null)//if there is a building moves it with mouse pos
            {
                MoveObjectWithCursor();
            }
        }

        public void SpawnBuilding(GameObject building)//spawn build gonna be placed
        {
            if (currentBuilding == null)
            {

                currentBuilding = Instantiate(building);
                _currentBuildingInfo = currentBuilding.GetComponent<Building>();
            }
            else
            {
                Destroy(currentBuilding);
                currentBuilding = Instantiate(building);
                _currentBuildingInfo = currentBuilding.GetComponent<Building>();
            }
        
        }

        private void MoveObjectWithCursor()// move building with cursor and check placement pos availability
        {
            currentBuilding.transform.position = MouseControl.Instance.GetGridPosOfCursor().WorldPos;
            ReturnIndicatorsToPool();
        
            IsPlacementFieldEmpty();
        }

        public void CancelBuilding()//cancels building placement.
        {
            Destroy(currentBuilding);
            ReturnIndicatorsToPool();
        }

        private void ReturnIndicatorsToPool()
        {
            if (_activeIndicatorSquares.Count > 0)
            {
                foreach (PooledObject square in _activeIndicatorSquares)
                {
                    square.ReturnToPool();
                }
            }
            _activeIndicatorSquares.Clear();
        }
    
        private bool IsPlacementFieldEmpty()//checks placement pos and show pos status with overlaying squares
        {
       

            int redCount = 0;
            if(currentBuilding)
                for (int i = 0; i < _currentBuildingInfo.BaseSize.x; i++)
                {
                    for (int j = 0; j < _currentBuildingInfo.BaseSize.y; j++)
                    {

                        GridCell mouseOnCell = MouseControl.Instance.GetGridPosOfCursor();

                        //cellAtCoords = GridManager.Instance.GridCells[cellAtCoords.GridPos.x + i, cellAtCoords.GridPos.y + j];
                        Vector2Int cellAtCoords = new Vector2Int(mouseOnCell.GridPos.x + i, mouseOnCell.GridPos.y + j);
               
                        if (GridManager.Instance.IsCellEmpty(cellAtCoords.x,cellAtCoords.y))
                        {
                            PooledObject green = _squareGreenPool.Get();
                            green.transform.position = GridManager.Instance.GetWorldPos(cellAtCoords);
                            green.gameObject.SetActive(true);
                            green.transform.SetParent(transform);
                            _activeIndicatorSquares.Add(green);
                            green.MyPool = _squareGreenPool;
                        }
                        else
                        {
                            PooledObject red = _squareRedPool.Get();
                            red.transform.position = GridManager.Instance.GetWorldPos(cellAtCoords);
                            red.gameObject.SetActive(true);
                            red.transform.SetParent(transform);
                            redCount++;
                            _activeIndicatorSquares.Add(red);
                            red.MyPool = _squareRedPool;
                        }
                
                
                    }
                }

            if (redCount > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
       
        
        }

        private void FillGridCells()//updates cells status with placed building info
        {
            if (currentBuilding)
            {

                GridManager.Instance.setObjectOnPos(currentBuilding);
            }
        }
        public void PlaceBuilding()//places building to mouse pos.
        {
            if (IsPlacementFieldEmpty())
            {
                FillGridCells();
                ReturnIndicatorsToPool();
                _currentBuildingInfo.isActive = true;
                currentBuilding = null;
            }
        }
    }
}
