﻿using UnityEngine;

namespace Assets.Scripts
{
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

        public GridCell(bool _isGround)
        {
            IsGround = _isGround;
        }

    }
}
