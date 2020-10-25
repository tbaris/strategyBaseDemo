using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Assets.Scripts
{

    public class MovingUnits : PlayableObject
    {
        [SerializeField] private float speed;

        private Queue<GridCell> _pathQueue = new Queue<GridCell>();
      
        private GridCell _currentCell;
        private GridCell _nextStop;
        private Pathfind pathfinder;
      
        private GridCell _newTargetCell;
        private bool _isMoving = false;


        private void Awake()
        {
            _pathQueue = new Queue<GridCell>();

        }

        private void findPathToTarget(GridCell newTarget)
        {
            _newTargetCell = newTarget;
            StopCoroutine("moveToNextCell");
         

            if (this.GetComponent<Pathfind>())
            {
                pathfinder = this.GetComponent<Pathfind>();
            }
            else
            {
                pathfinder = this.gameObject.AddComponent<Pathfind>();
            }

            _currentCell = GridManager.Instance.GetCellAdress(this);

            List<GridCell> path = pathfinder.FindPath(_currentCell, _targetCell);
           
            _pathQueue.Clear();


            for (int i = 0; i < path?.Count; i++)
            {
                _pathQueue.Enqueue(path[i]);
            }

            if (_pathQueue.Count > 0)
            {
                StartMove();
            }

        }



        public override void SetDestination(GridCell target)//sets destination and gets a path to follow
        {
            
            _newTargetCell = target;
            if (!_isMoving)
            {
                base.SetDestination(target);
                findPathToTarget(_newTargetCell);
            }

        }




        private void StartMove()
        {
           
            StopCoroutine("moveToNextCell");
            GetNextCell();
        }

        private void GetNextCell()
        {

            _nextStop = _pathQueue.Dequeue();

            if (GridManager.Instance.canMoveObjectToCell(this, _nextStop))
            {
                StopCoroutine("moveToNextCell");
                StartCoroutine(moveToNextCell(_nextStop));
            }
            else
            {
                SetDestination(_targetCell);
            }

        }
        private IEnumerator moveToNextCell(GridCell nextCell)
        {
           

            while (nextCell.WorldPos != transform.position)
            {
                _isMoving = true;
                transform.position = Vector2.MoveTowards(transform.position, nextCell.WorldPos, speed * Time.deltaTime);
               

                yield return null;
            }

            _isMoving = false;

            _currentCell = nextCell;

            if (_newTargetCell != _targetCell)
            {
                SetDestination(_newTargetCell);
            }
            else if (_pathQueue.Count > 0)
            {
                GetNextCell();
            }

        }




    }
}
