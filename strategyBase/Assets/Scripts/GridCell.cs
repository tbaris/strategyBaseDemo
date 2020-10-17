using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell 
{
    public Vector3 worldPos;
    public Vector2Int gridPos;
    public bool isEmpty = true;
    public GameObject gameObjectOnPos = null;

    public int gCost = 0;
    public int hCost = 0;
    public int totalCost = 0;


}
