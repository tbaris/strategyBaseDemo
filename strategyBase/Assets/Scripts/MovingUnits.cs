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
            _pathQueue.Clear();

            if (this.GetComponent<Pathfind>())
            {
                pathfinder = this.GetComponent<Pathfind>();
            }
            else
            {
                pathfinder = this.gameObject.AddComponent<Pathfind>();
            }

            List<GridCell> path = pathfinder.FindPath(GridManager.Instance.GetCellAdress(transform.position), _targetCell);
            for (int i = 0; i < path?.Count; i++)
            {
                _pathQueue.Enqueue(path[i]);
            }

            _currentCell = GridManager.Instance.GetCellAdress(transform.position);
            if (_pathQueue.Count > 0)
            {
                StartMove();
            }


        }

        private void StartMove()
        {
            GridManager.Instance.removeObjectOnCell(this.gameObject, _currentCell);
            StopCoroutine("moveToNextCell");
            _nextStop = _pathQueue.Dequeue();
            if (GridManager.Instance.canMoveObjectToCell(this.gameObject,_currentCell, _nextStop))
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
                transform.position = Vector2.MoveTowards(transform.position, _nextStop.WorldPos, speed * Time.deltaTime);
                yield return null;
            }

            _currentCell = _nextStop;

            if (_pathQueue.Count > 0)
            {
                GetNextCell();
            }
        }

        private void GetNextCell()
        {
            _nextStop = _pathQueue.Dequeue();

            if (GridManager.Instance.canMoveObjectToCell(this.gameObject, _currentCell, _nextStop))
            {
                StartCoroutine(moveToNextCell(_nextStop));
               
            }
            else
            {
                SetDestination(_targetCell);
            }

        }


    }
}
