using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class productionButton : MonoBehaviour
{
    public GameObject prefab;
    public GameObject spawnBuilding;
    private void Awake()
    {
        this.GetComponent<Button>().onClick.AddListener(orderSpawn);
    }

    private void orderSpawn()
    {
        spawnBuilding.GetComponent<Building>()?.spawnUnit(prefab);
    }
}
