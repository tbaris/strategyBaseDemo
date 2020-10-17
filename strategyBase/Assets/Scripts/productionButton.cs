using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductionButton : MonoBehaviour
{
    public GameObject prefab;
    public GameObject spawnBuilding;
    private void Awake()
    {
        this.GetComponent<Button>().onClick.AddListener(OrderSpawn);
    }

    private void OrderSpawn()
    {
        spawnBuilding.GetComponent<Building>()?.SpawnUnit(prefab);
    }
}
