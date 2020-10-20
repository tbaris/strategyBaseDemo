using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Building : PlayableObject
    {
        public bool isActive;
        public List<ProductionButton> products;

  

        public void SpawnUnit(GameObject spawnUnit)//spawn units to closest empty cell.
        {
        
            GridCell nearestEmptyCell = GridManager.Instance.GetClosestEmptyPos(
                new Vector3(transform.position.x + BaseSize.x / 2, transform.position.y + BaseSize.y / 2,
                    transform.position.z));

        
            Vector3 spawnPos = GridManager.Instance.GetWorldPos(nearestEmptyCell);

            GameObject unit = Instantiate(spawnUnit.gameObject,spawnPos,Quaternion.identity);
            GridManager.Instance.setObjectOnPos(unit);

        }


   
    }
}
