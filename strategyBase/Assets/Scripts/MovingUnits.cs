using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{

    public class MovingUnits : PlayableObject
    {
        [SerializeField]private float speed;
   
        private Queue<GridCell> _pathQueue = new Queue<GridCell>();
        private bool _isStoped = true;
        private GridCell _nextStop;


        private void Awake()
        {
            _pathQueue = new Queue<GridCell>();
            _nextStop = GridManager.Instance.GetCellAdress(transform.position);
        }

        public override void SetDestination(GridCell target)//sets destination and gets a path to follow
        {
        
        
            base.SetDestination(target);
            _pathQueue.Clear();

            Pathfind pathfinder = GameObject.FindObjectOfType<Pathfind>();
            List<GridCell> path = pathfinder.FindPath(GridManager.Instance.GetCellAdress(transform.position), target);
            for (int i = 0; i < path?.Count; i++)
            {
                _pathQueue.Enqueue(path[i]); 
            }

        }


      
      

        private void Update()//moves unit along to path  ---- extract this to another method 
        {
            if (_nextStop.WorldPos == transform.position)
            {
                GridManager.Instance.setObjectOnPos(this.gameObject);
                if (_pathQueue.Count > 0)
                {
                    _nextStop = _pathQueue.Dequeue();
                    GridManager.Instance.removeObjectOnPos(this.gameObject);
                }
            }
        
            if(_nextStop.WorldPos != transform.position)
            {
                transform.position = Vector2.MoveTowards(transform.position, _nextStop.WorldPos, speed* Time.deltaTime);
                
            }

        }

    }
}
