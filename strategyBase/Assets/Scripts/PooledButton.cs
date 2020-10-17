using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;




public class PooledButton : Button
{
   

    public GameObject building;
  
    public BuildingMenuPool myPool;

    protected override void Awake()
    {
        base.Awake();
        this.onClick.AddListener(sendTypeToSpawn);
         

    }

    private void sendTypeToSpawn()
    {
        GameController.Instance.StartBuilding(building);

    }

    public void returnToPool()
    {
        myPool.ReturnToPool(this);
    }




}
