using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class Building : PlayableObject
    {
        private bool isActive;
        public List<ProductionButton> products;
        private GridCell _spawnTarget;
        [SerializeField] private GameObject _flag;
        private GameObject flagInstance;


      

        public void setActive()
        {
            _spawnTarget = GridManager.Instance.GetClosestEmptyPos(
                new Vector3(transform.position.x + BaseSize.x / 2, transform.position.y + BaseSize.y / 2,
                    transform.position.z));
            flagInstance = Instantiate(_flag, _spawnTarget.WorldPos, Quaternion.identity);
            isActive = true;
        }

        public void SpawnUnit(GameObject spawnUnit) //spawn units to closest empty cell.
        {

            GridCell nearestEmptyCell = GridManager.Instance.GetClosestEmptyPos(
                new Vector3(transform.position.x + BaseSize.x / 2, transform.position.y + BaseSize.y / 2,
                    transform.position.z));


            Vector3 spawnPos = GridManager.Instance.GetWorldPos(nearestEmptyCell);

            GameObject unit = Instantiate(spawnUnit.gameObject, spawnPos, Quaternion.identity);
            GridManager.Instance.setObjectOnPos(unit);
            if (nearestEmptyCell != _spawnTarget)
            {
                if (GridManager.Instance.IsCellEmpty(_spawnTarget))
                {
                    unit.GetComponent<MovingUnits>().SetDestination(_spawnTarget);
                }
                else
                {
                    unit.GetComponent<MovingUnits>().SetDestination(GridManager.Instance.GetClosestEmptyPos(_spawnTarget.WorldPos));
                }
                    
                       
            }

        }

        public override void SetDestination(GridCell target) //sets destination and gets a path to follow
        {


            base.SetDestination(target);

           
            if (GridManager.Instance.IsCellEmpty(target))
            {
                _spawnTarget = target;
                flagInstance.transform.position = _spawnTarget.WorldPos;
            }

            
        }
    }
}
