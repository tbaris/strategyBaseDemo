﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : PlayableObject
{
    public bool isActive = false;
    public List<productionButton> products;

  

    public void spawnUnit(GameObject spawnUnit)
    {
        
        GridCell nearestEmptyCell = GridManager.Instance.getClosestEmptyPos(
            new Vector3(transform.position.x + (baseSize.x/2) , transform.position.y + (baseSize.y/2 ), transform.position.z));
        Vector3 spawnPos = GridManager.Instance.getWorldPos(nearestEmptyCell);

        GameObject unit = Instantiate(spawnUnit.gameObject);
        nearestEmptyCell.isEmpty = false;
        unit.transform.position = spawnPos;


    }


   
}
