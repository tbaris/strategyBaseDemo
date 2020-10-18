using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingUnits : PlayableObject
{
    [SerializeField]private float speed;
   
    private Queue<GridCell> _pathQueue = new Queue<GridCell>();
    private bool isStoped = true;
    private GridCell nextStop;


    private void Awake()
    {
        _pathQueue = new Queue<GridCell>();
        nextStop = GridManager.Instance.GetCellAdress(transform.position);
    }

    public override void setDestination(GridCell target)
    {
        
        
        base.setDestination(target);
        Pathfind pathfind = GameObject.FindObjectOfType<Pathfind>();
        List<GridCell> path = pathfind.FindPath(GridManager.Instance.GetCellAdress(transform.position), target);
        for (int i = 0; i < path?.Count; i++)
        {
            _pathQueue.Enqueue(path[i]); 
        }
    }

    private void Update()
    {
        if (nextStop.WorldPos == transform.position)
        {
            GridManager.Instance.setObjectOnPos(this.gameObject);
            if (_pathQueue.Count > 0)
            {
           
                nextStop = _pathQueue.Dequeue();
                GridManager.Instance.removeObjectOnPos(this.gameObject);
            }
        }
        
        if(nextStop.WorldPos != transform.position)
        {
                transform.position = Vector2.MoveTowards(transform.position, nextStop.WorldPos, speed);
                
        }
           
            

        
    }

    private void move(Queue<GridCell> path)
    {
        if (path.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position,_pathQueue.Dequeue().WorldPos,speed);
        }
    }
}
