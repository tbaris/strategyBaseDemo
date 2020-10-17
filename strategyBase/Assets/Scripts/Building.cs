using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : PlayableObject
{
    public bool isActive = false;
    public List<ProductionButton> products;

  

    public void SpawnUnit(GameObject spawnUnit)
    {
        
        GridCell nearestEmptyCell = GridManager.Instance.GetClosestEmptyPos(
            new Vector3(transform.position.x + (BaseSize.x/2) , transform.position.y + (BaseSize.y/2 ), transform.position.z));
        Vector3 spawnPos = GridManager.Instance.GetWorldPos(nearestEmptyCell);

        GameObject unit = Instantiate(spawnUnit.gameObject);
        nearestEmptyCell.IsEmpty = false;
        unit.transform.position = spawnPos;


    }


   
}
