﻿
using UnityEngine;

namespace Assets.Scripts
{

    //base unit properties
    public abstract class Unit : MonoBehaviour
    {
        public new string name;
        public Vector2Int BaseSize;
        public Sprite sprite;



        private void Start()
        {

            if (!GridManager.Instance.AddGoOnGrid(this))
            {
                Destroy(this.gameObject);
            }
           
        }
    }
}
