using System;
using System.Collections;
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
        private bool _isSpawnTargetActive = false;


        
        

        public void setActive()
        {
            GameController.SelectedAObject += switchFlagOnOff;
            flagInstance = Instantiate(_flag, transform.position, Quaternion.identity);
            flagInstance.SetActive(false);
        }

        private void switchFlagOnOff(object sender, GameController.SelectedAObjectArgs e)
        {
            if (e.SelectedGameObject == this.gameObject && _isSpawnTargetActive)
            {
                flagInstance.SetActive(true);
            }
            else
            {
                flagInstance.SetActive(false);
            }
        }

        public void SpawnUnit(GameObject spawnUnit) //spawn units to closest empty cell.
        {

            GridCell nearestEmptyCell = GridManager.Instance.GetClosestEmptyPos(
                new Vector3(transform.position.x + BaseSize.x / 2, transform.position.y + BaseSize.y / 2,
                    transform.position.z));


            Vector3 spawnPos = GridManager.Instance.GetWorldPos(nearestEmptyCell);

            GameObject unit = Instantiate(spawnUnit.gameObject, spawnPos, Quaternion.identity);
            unit.GetComponent<Unit>().enabled = true;
            
            if (_isSpawnTargetActive && nearestEmptyCell != _spawnTarget)
            {
                StartCoroutine(_setSpawnTarget(unit, _spawnTarget));
            }
            
        }

        private IEnumerator _setSpawnTarget(GameObject spawnUnit, GridCell spawnTarget)
        {
            yield return new WaitForEndOfFrame();
            spawnUnit.GetComponent<MovingUnits>().SetDestination(spawnTarget);
        }

        public override void SetDestination(GridCell target) //sets destination and gets a path to follow
        {


            base.SetDestination(target);
            
            if (GridManager.Instance.IsCellEmpty(target))
            {
                _spawnTarget = target;
                _isSpawnTargetActive = true;
                flagInstance.SetActive(true);
                flagInstance.transform.position = _spawnTarget.WorldPos;
            }

            
        }

        private void OnDestroy()
        {
            GameController.SelectedAObject -= switchFlagOnOff;
        }
    }
}
