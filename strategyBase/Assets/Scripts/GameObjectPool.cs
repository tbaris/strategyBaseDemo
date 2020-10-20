using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;


public class GameObjectPool : ObjectPooler<PooledObject>
{

    //pool class for other objects

    public GameObjectPool(PooledObject poolingObject)
    {
        Prefab = poolingObject;

    }

    



}
