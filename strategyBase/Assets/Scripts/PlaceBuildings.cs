using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBuildings : MonoBehaviour
{
    public GameObject currentBuilding;
    private Building currentBuildingInfo;

    public static PlaceBuildings Instance;

    [SerializeField] private PooledObject squareRed;
    private GameObjectPool squareRedPool;
    private GameObjectPool squareGreenPool;
    [SerializeField] private PooledObject squareGreen;
    private List<PooledObject> activeIndicatorSquares;

    private void Awake()
    {
        squareRedPool = new GameObjectPool(squareRed);
        squareGreenPool = new GameObjectPool(squareGreen);
        activeIndicatorSquares =  new List<PooledObject>();
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
            currentBuildingInfo = currentBuilding.GetComponent<Building>();
        }
        else
        {
            Destroy(currentBuilding);
            currentBuilding = Instantiate(building);
            currentBuildingInfo = currentBuilding.GetComponent<Building>();
        }
        
    }

    private void MoveObjectWithCursor()
    {
        currentBuilding.transform.position = MouseControl.Instance.getGridPosOfCursor().worldPos;
        returnIndicatorsToPool();
        
        isPlacementFieldEmpty();
    }

    public void CancelBuilding()
    {
        Destroy(currentBuilding);
    }

    private void returnIndicatorsToPool()
    {
        if (activeIndicatorSquares.Count > 0)
        {
            foreach (PooledObject square in activeIndicatorSquares)
            {
                square.returnToPool();
            }
        }
        activeIndicatorSquares.Clear();
    }
    
    private bool isPlacementFieldEmpty()
    {
       

        int redCount = 0;
        if(currentBuilding)
        for (int i = 0; i < currentBuildingInfo.baseSize.x; i++)
        {
            for (int j = 0; j < currentBuildingInfo.baseSize.y; j++)
            {
                
                GridCell cellAtCoord = MouseControl.Instance.getGridPosOfCursor();

                cellAtCoord = GridManager.Instance.gridCells[cellAtCoord.gridPos.x + i, cellAtCoord.gridPos.y + j];
               
                if (GridManager.Instance.isCellEmpty(cellAtCoord))
                {
                     PooledObject green = squareGreenPool.Get();
                     green.transform.position = GridManager.Instance.getWorldPos(cellAtCoord);
                     green.gameObject.SetActive(true);
                     green.transform.SetParent(transform);;
                     activeIndicatorSquares.Add(green);
                     green.myPool = squareGreenPool;
                }
                else
                {
                        PooledObject red = squareRedPool.Get();
                        red.transform.position = GridManager.Instance.getWorldPos(cellAtCoord);
                        red.gameObject.SetActive(true);
                        red.transform.SetParent(transform); ;
                        redCount++;
                        activeIndicatorSquares.Add(red);
                        red.myPool = squareRedPool;
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

    private void fillGridCells()
    {
        if (currentBuilding)
        {

            for (int i = 0; i < currentBuildingInfo.baseSize.x; i++)
            {
                for (int j = 0; j < currentBuildingInfo.baseSize.y; j++)
                {
                    GridCell cellAtCoord = MouseControl.Instance.getGridPosOfCursor();
                    cellAtCoord = GridManager.Instance.gridCells[cellAtCoord.gridPos.x + i, cellAtCoord.gridPos.y + j];
                    GridManager.Instance.setObjectOnPos(cellAtCoord, currentBuilding);

                }
            }
        }
    }
    public void PlaceBuilding()
    {
        if (isPlacementFieldEmpty())
        {
            fillGridCells();
            returnIndicatorsToPool();
            currentBuildingInfo.isActive = true;
            currentBuilding = null;
        }
    }
}
