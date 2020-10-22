using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{

    public class MovingUnits : PlayableObject
    {
        [SerializeField] private float speed;

        private Queue<GridCell> _pathQueue = new Queue<GridCell>();
        private bool _isStoped = true;
        private GridCell _currentCell;
        private GridCell _nextStop;
        private Pathfind pathfinder;


        private void Awake()
        {
            _pathQueue = new Queue<GridCell>();
            _nextStop = GridManager.Instance.GetCellAdress(transform.position);
        }

        public override void SetDestination(GridCell target)//sets destination and gets a path to follow
        {

           
            base.SetDestination(target);
           

            if (this.GetComponent<Pathfind>())
            {
                pathfinder = this.GetComponent<Pathfind>();
            }
            else
            {
                pathfinder = this.gameObject.AddComponent<Pathfind>();
            }
            _currentCell = GridManager.Instance.GetCellAdress(transform.position);
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


        private void StartMove()
        {
            StopCoroutine("moveToNextCell");
            GetNextCell();
        }

        private void GetNextCell()
        {
            _nextStop = _pathQueue.Dequeue();

            if (GridManager.Instance.canMoveObjectToCell(this.gameObject, _nextStop))
            {
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
                transform.position = Vector2.MoveTowards(transform.position, nextCell.WorldPos, speed * Time.deltaTime);
                yield return null;
            }
            GridManager.Instance.removeObjectOnCell(this.gameObject,_currentCell);
            _currentCell = nextCell;
           
            if (_pathQueue.Count > 0 )
            {
                GetNextCell();
            }
            
        }

     


    }
}
