using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class GameObjectPool : ObjectPooler<PooledObject>
{

    public GameObjectPool(PooledObject poolingObject)
    {
        Prefab = poolingObject;

    }

    



}
