using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class PooledButton : Image
{

    public BuildingMenuPool myPool;




    public void returnToPool()
    {
        myPool.ReturnToPool(this);
    }

   
}
