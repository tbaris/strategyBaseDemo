using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingMenuPool : ObjectPooler<PooledButton>
{
    public BuildingMenuPool(PooledButton poolingObject,BuildingMenuPoolHolder pHolder )
    {
        prefab = poolingObject;
        poolHolder = pHolder;
    }

   
}