using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [SerializeField]private Vector2Int _gridSize = Vector2Int.one;
    [SerializeField] private float cellSize = 1.0f; private GridManager _grid;

    public static event EventHandler<SelectedAObjectArgs> SelectedAObject; 
    public class SelectedAObjectArgs : EventArgs
    {
        public GameObject SelectedGameObject;
    }
    
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

        _grid = new GridManager(_gridSize.x, _gridSize.y, cellSize);
     
       

    }

    public void OnLeftClick()
    {
        if (PlaceBuildings.Instance.currentBuilding != null)
        {
            PlaceBuildings.Instance.PlaceBuilding();
        }

        SelectedAObject?.Invoke(this, new SelectedAObjectArgs{SelectedGameObject = MouseControl.Instance.GetGridPosOfCursor().GameObjectOnPos });
        Debug.Log(MouseControl.Instance.GetGridPosOfCursor().GameObjectOnPos);
        
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
        if (_grid != null)
        {
            foreach (var cell in _grid.GridCells)
            {
                Color gridColor = new Color();
                if (cell.IsEmpty)
                {
                    gridColor = Color.green;
                }
                else
                {
                    gridColor = Color.red;
                }

                Gizmos.color = gridColor;
                Gizmos.DrawCube(cell.WorldPos, Vector3.one / 4);
            }
        }

    }
}
