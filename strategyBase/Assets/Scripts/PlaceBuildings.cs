using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (currentBuilding != null)
        {
            MoveObjectWithCursor();
        }
    }

    public void SpawnBuilding(GameObject building)
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

    private void MoveObjectWithCursor()
    {
        currentBuilding.transform.position = MouseControl.Instance.GetGridPosOfCursor().WorldPos;
        ReturnIndicatorsToPool();
        
        İsPlacementFieldEmpty();
    }

    public void CancelBuilding()
    {
        Destroy(currentBuilding);
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
    
    private bool İsPlacementFieldEmpty()
    {
       

        int redCount = 0;
        if(currentBuilding)
        for (int i = 0; i < _currentBuildingInfo.BaseSize.x; i++)
        {
            for (int j = 0; j < _currentBuildingInfo.BaseSize.y; j++)
            {
                
                GridCell cellAtCoord = MouseControl.Instance.GetGridPosOfCursor();

                cellAtCoord = GridManager.Instance.GridCells[cellAtCoord.GridPos.x + i, cellAtCoord.GridPos.y + j];
               
                if (GridManager.Instance.İsCellEmpty(cellAtCoord))
                {
                     PooledObject green = _squareGreenPool.Get();
                     green.transform.position = GridManager.Instance.GetWorldPos(cellAtCoord);
                     green.gameObject.SetActive(true);
                     green.transform.SetParent(transform);;
                     _activeIndicatorSquares.Add(green);
                     green.MyPool = _squareGreenPool;
                }
                else
                {
                        PooledObject red = _squareRedPool.Get();
                        red.transform.position = GridManager.Instance.GetWorldPos(cellAtCoord);
                        red.gameObject.SetActive(true);
                        red.transform.SetParent(transform); ;
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

    private void FillGridCells()
    {
        if (currentBuilding)
        {

            for (int i = 0; i < _currentBuildingInfo.BaseSize.x; i++)
            {
                for (int j = 0; j < _currentBuildingInfo.BaseSize.y; j++)
                {
                    GridCell cellAtCoord = MouseControl.Instance.GetGridPosOfCursor();
                    cellAtCoord = GridManager.Instance.GridCells[cellAtCoord.GridPos.x + i, cellAtCoord.GridPos.y + j];
                    GridManager.Instance.setObjectOnPos(cellAtCoord, currentBuilding);

                }
            }
        }
    }
    public void PlaceBuilding()
    {
        if (İsPlacementFieldEmpty())
        {
            FillGridCells();
            ReturnIndicatorsToPool();
            _currentBuildingInfo.isActive = true;
            currentBuilding = null;
        }
    }
}
