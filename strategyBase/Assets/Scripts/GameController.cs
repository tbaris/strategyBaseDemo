using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [SerializeField]private Vector2Int gridSize = Vector2Int.one;
    [SerializeField] private float cellSize = 1.0f; private GridManager grid;

 //   public static event Action<GameObject> SelectedAObject; 
    
    private void Awake()
    {
        
        
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        grid = new GridManager(gridSize.x, gridSize.y, cellSize);
     
       

    }

    public void OnLeftClick()
    {
        if (PlaceBuildings.Instance.currentBuilding != null)
        {
            PlaceBuildings.Instance.PlaceBuilding();
        }else if (MouseControl.Instance.getGridPosOfCursor().gameObjectOnPos)
        {
          //  SelectedAObject(MouseControl.Instance.getGridPosOfCursor().gameObjectOnPos);
            Debug.Log(MouseControl.Instance.getGridPosOfCursor().gameObjectOnPos);
        }
    }
    public void OnRightClick()
    {
        if (PlaceBuildings.Instance.currentBuilding != null)
        {
            PlaceBuildings.Instance.CancelBuilding();
        }
    }
    public void OnMiddleClick()
    {
        Debug.Log("middle");
    }

    public void StartBuilding(GameObject building)
    {

        PlaceBuildings.Instance.SpawnBuilding(building);
    }
  

    private void OnDrawGizmos()
    {
        if (grid != null)
        {
            foreach (var cell in grid.gridCells)
            {
                Color gridColor = new Color();
                if (cell.isEmpty)
                {
                    gridColor = Color.green;
                }
                else
                {
                    gridColor = Color.red;
                }

                Gizmos.color = gridColor;
                Gizmos.DrawCube(cell.worldPos, Vector3.one / 4);
            }
        }

    }
}
