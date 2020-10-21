using UnityEngine;

namespace Assets.Scripts
{
    //class for cell props.
    public class GridCell 
    {
        public Vector3 WorldPos;
        public Vector2Int GridPos;
        public bool IsEmpty = true;
        public GameObject GameObjectOnPos = null;
        public bool IsGround { get; private set; }


        //data for pathfinder
        public int GCost = 0;
        public int HCost = 0;
        public int TotalCost = 0;
        public GridCell PreviousCell;
        public bool IsVisited = false;
        public bool IsCalculated = false;

        public GridCell(bool _isGround)
        {
            IsGround = _isGround;
        }

    }
}
