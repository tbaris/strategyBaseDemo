using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance;

        [SerializeField]private Vector2Int _gridSize = Vector2Int.one;
        [SerializeField]private float cellSize = 1.0f;
        [SerializeField] private int borderLineWitdh = 10;
        [SerializeField] private GameObject grassTile;
        [SerializeField] private GameObject waterTile;
        private GridManager _grid;

        public static event EventHandler<SelectedAObjectArgs> SelectedAObject; 
        public class SelectedAObjectArgs : EventArgs
        {
            public GameObject SelectedGameObject;
        }

 

        private GameObject _selectedGo;
    
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

            _grid = new GridManager(_gridSize.x, _gridSize.y, cellSize,borderLineWitdh);
            foreach (GridCell cell in GridManager.Instance.GridCells)
            {
                if (cell.IsGround)
                {
                    GameObject grass = Instantiate(grassTile, cell.WorldPos, Quaternion.identity);
                    grass.transform.SetParent(transform);
                }
                else
                {
                    GameObject water = Instantiate(waterTile, cell.WorldPos, Quaternion.identity);
                    water.transform.SetParent(transform);
                }
            }

        }

        public void OnLeftClick()
        {
            if (PlaceBuildings.Instance.currentBuilding != null)
            {
                PlaceBuildings.Instance.PlaceBuilding();
            }

            if (MouseControl.Instance.GetGridPosOfCursor() != null)
            {
                _selectedGo = MouseControl.Instance.GetGridPosOfCursor().GameObjectOnPos;
                SelectedAObject?.Invoke(this, new SelectedAObjectArgs { SelectedGameObject = _selectedGo });
            }
        
        
        
        }
        public void OnRightClick()
        {
            if (PlaceBuildings.Instance.currentBuilding != null)
            {
                PlaceBuildings.Instance.CancelBuilding();
            }

            if (_selectedGo != null)
            {
                if (_selectedGo.GetComponent<PlayableObject>())
                {
                    _selectedGo.GetComponent<PlayableObject>().SetDestination(MouseControl.Instance.GetGridPosOfCursor());
                }
            
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
                    var gridColor = GridManager.Instance.IsCellEmpty(cell)? Color.green : Color.red;

                    Gizmos.color = gridColor;
                    Gizmos.DrawCube(cell.WorldPos, Vector3.one / 4);
                }
            }

        }
    }
}
